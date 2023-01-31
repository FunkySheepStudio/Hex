using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Board
{
    public class FogOfWar : MonoBehaviour
    {
        public GameObject fogOfWarPrefab;
        public void Generate(ulong playerId)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Tile tile = transform.GetChild(i).GetComponent<Tile>();
                if (tile.GetComponent<NetworkBehaviour>().OwnerClientId != playerId)
                {
                    tile.fogOfWar = GameObject.Instantiate(fogOfWarPrefab, tile.transform.position + Vector3.up, Quaternion.identity, transform.GetChild(i).transform);
                }
            }
        }
    }
}
