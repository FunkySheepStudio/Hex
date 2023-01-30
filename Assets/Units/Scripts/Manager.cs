using Game.Board;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Units
{
    public class Manager : MonoBehaviour
    {
        public int owner;
        public int moveRange = 1;
        List<Tile> neighbors = new List<Tile>();
        bool selected = false;

        private void Start()
        {
            GetComponent<MeshRenderer>().material.color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(owner);
            transform.parent.GetComponent<Game.Board.Tile>().RemoveFogOfWar();
        }

        public void OnSelect(GameObject tileGo)
        {
            if (transform.parent.gameObject == tileGo)
            {
                Game.Board.Tile tile = tileGo.GetComponent<Tile>();
                if (tile.owner == NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().id)
                {
                    NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Move(tileGo.transform.position);
                    transform.parent.GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
                    transform.parent.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.red);

                    neighbors = tile.FindNeighbors(moveRange);

                    for (int i = 0; i < neighbors.Count; i++)
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.white);
                    }

                    selected = true;
                }
            }
            if (selected)
            {
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (tileGo == neighbors[i].gameObject)
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.green);
                    }
                }
            }
        }

        public void OnDeSelect(GameObject tileGo)
        {
            if (selected)
            {
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (tileGo == neighbors[i].gameObject)
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Color.white);
                    }
                }
            }
        }

        public void OnSelectStopped(GameObject tileGo)
        {
            if (selected)
            {
                if (transform.parent.GetComponent<Tile>().owner == NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().id)
                {
                    transform.parent.GetComponent<MeshRenderer>().materials[1].color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color();
                } else
                {
                    transform.parent.GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 0);
                }

                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (neighbors[i].GetComponent<Tile>().owner != -1)
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color(neighbors[i].GetComponent<Tile>().owner);
                    } else
                    {
                        neighbors[i].GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 0);
                    }
                    if (neighbors[i].gameObject == tileGo)
                    {
                        Move(tileGo);
                    }
                }
                neighbors.Clear();
                selected = false;
            }
        }

        void Move(GameObject tileGo)
        {
            transform.parent.GetComponent<Tile>().unitManager = null;

            transform.parent = tileGo.transform;
            transform.position = tileGo.transform.position;
            tileGo.GetComponent<Tile>().owner = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().id;
            tileGo.GetComponent<MeshRenderer>().materials[1].color = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().Color();
            tileGo.GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
            tileGo.GetComponent<Tile>().unitManager = this;
            tileGo.GetComponent<Tile>().RemoveFogOfWar();
        }
    }
}
