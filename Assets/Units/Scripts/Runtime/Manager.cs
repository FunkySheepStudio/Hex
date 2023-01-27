using Game.Board;
using System.Collections.Generic;
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
            GetComponent<MeshRenderer>().material.color = Player.Manager.Instance.Color(owner);
            transform.parent.GetComponent<Game.Board.Tile>().RemoveFogOfWar();
        }

        public void OnSelect(GameObject tileGo)
        {
            if (transform.parent.gameObject == tileGo)
            {
                Game.Board.Tile tile = tileGo.GetComponent<Tile>();
                if (tile.owner == Player.Manager.Instance.id)
                {
                    transform.parent.GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 1);
                    transform.parent.GetComponent<MeshRenderer>().materials[1].SetColor("_Color", Player.Manager.Instance.Color());

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

        public void OnSelectStopped(GameObject tile)
        {
            transform.parent.GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 0);

            for (int i = 0; i < neighbors.Count; i++)
            {
                neighbors[i].GetComponent<MeshRenderer>().materials[1].SetInt("_Selected", 0);
            }
            neighbors.Clear();
            selected = false;
        }
    }
}
