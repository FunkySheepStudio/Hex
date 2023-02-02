using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Modes
{
    public class Manager : MonoBehaviour
    {
        public List<Mode> modes;

        public void SetCurrentMode(int index)
        {
            Game.Manager.Instance.currentMode = modes[index];
            SceneManager.LoadScene("_Final/Game/Netcode/Netcode", LoadSceneMode.Single);
        }
    }
}
