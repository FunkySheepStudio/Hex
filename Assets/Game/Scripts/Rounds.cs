using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;
using TMPro;

namespace Game
{
    [Serializable]
    public class Phases
    {
        public string name;
        public float duration;
    }

    public class Rounds : NetworkBehaviour
    {
        public NetworkVariable<int> round = new NetworkVariable<int>(0);
        public TextMeshProUGUI roundTxt;
        public TextMeshProUGUI phaseTxt;
        public TextMeshProUGUI phaseTimeLeftTxt;

        public List<Phases> phases = new List<Phases>();
        int phase = 0;
        float time = 0;

        private void Update()
        {
            if (time >= phases[phase].duration)
            {
                time = 0;
                phase = (phase + 1) % phases.Count;
                phaseTxt.text = phases[phase].name;
                if (phase == 0)
                {
                    if (IsServer)
                        round.Value += 1;
                    roundTxt.text = round.Value.ToString();
                }
            }

            time += Time.deltaTime;

            phaseTimeLeftTxt.text = (phases[phase].duration - time).ToString();
        }
    }
}
