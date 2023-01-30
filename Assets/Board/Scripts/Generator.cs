using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Board
{
    public class Generator : NetworkBehaviour
    {
        public List<GameObject> starts;
        public List<GameObject> trees;
        public List<GameObject> rocks;
        public List<GameObject> tiles;
        public int size;
        public NetworkVariable<int> seed = new NetworkVariable<int>();

        private void Awake()
        {
            if (IsServer)
            {
                seed.Value = Random.Range(0, 1000);
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                Generate();
                GetComponent<FogOfWar>().Generate(NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().id);
            }
            SetStartPosition();
        }

        public void Clear()
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        public void Generate()
        {
            List<Vector3Int> path = ComputePath(size);

            int owner = 0;

            for (int r = -size; r <= size; r++)
            {
                for (int s = -size; s <= size; s++)
                {
                    for (int q = -size; q <= size; q++)
                    {
                        if ((q + r + s) == 0)
                        {
                            Vector3 position = Vector3.left * r +
                            (Vector3.left * 0.5f + Vector3.forward * Mathf.Sqrt(3) / 2) * s;

                            GameObject go;

                            if (IsStartPosition(r, s, q, size))
                            {
                                go = GameObject.Instantiate(starts[0], position, Quaternion.identity, transform);
                                go.name = r + ":" + s + ":" + q;
                                go.GetComponent<Tile>().owner = owner;
                                owner += 1;
                            } else if (IsPath(r, s, q, path))
                            {
                                go = GameObject.Instantiate(tiles[0], position, Quaternion.identity, transform);
                                go.name = r + ":" + s + ":" + q;
                            } else if (IsRandom(r, s, q, size))
                            {
                                go = GameObject.Instantiate(trees[0], position, Quaternion.identity, transform);
                                go.name = r + ":" + s + ":" + q;
                            } else
                            {
                                go = GameObject.Instantiate(rocks[0], position, Quaternion.identity, transform);
                            }

                            int rotation = Random.Range(0, 6);
                            go.transform.Rotate(Vector3.up * rotation * 60);
                            go.name = r + ":" + s + ":" + q;

                            Tile tile = go.GetComponent<Tile>();
                            tile.position = new Vector3Int(r, s, q);

                            go.GetComponent<NetworkObject>().Spawn();
                            go.GetComponent<NetworkObject>().TrySetParent(gameObject);
                        }
                    }
                }
            }
        }

        public List<Vector3Int> ComputePath(int size)
        {
            Random.InitState(seed.Value);
            Vector3Int[] cube_direction_vectors = {
                new Vector3Int(1, 0, -1),
                new Vector3Int(0, 1, -1)
            };

            Vector3Int currentPoint = Vector3Int.zero;

            List<Vector3Int> points = new List<Vector3Int>();
            points.Add(currentPoint);

            while(true)
            {
                currentPoint += cube_direction_vectors[Random.Range(0, 2)];

                if (currentPoint.x >= size)
                {
                    currentPoint.x = size;
                }

                if (currentPoint.y >= size)
                {
                    currentPoint.y = size;
                }

                currentPoint.z = Mathf.Abs(currentPoint.x - currentPoint.y);
                points.Add(currentPoint);

                if (currentPoint.x == size && currentPoint.y == size)
                    break;
            }
            
            return points;
        }

        public bool IsPath(int r, int s, int q, List<Vector3Int> points)
        {
            Vector3Int point = new Vector3Int(Mathf.Abs(r), Mathf.Abs(s), Mathf.Abs(q));
            if (points.Contains(point))
                return true;

            point = new Vector3Int(Mathf.Abs(s), Mathf.Abs(q), Mathf.Abs(r));
            if (points.Contains(point))
                return true;

            point = new Vector3Int(Mathf.Abs(q), Mathf.Abs(r), Mathf.Abs(s));
            if (points.Contains(point))
                return true;

            return false;
        }

        public bool IsStartPosition(int r, int s, int q, int size)
        {
            return ((Mathf.Abs(r) + Mathf.Abs(s) + Mathf.Abs(q)) == (size * 2)) && (r == 0 || s == 0 || q == 0);
        }

        public bool IsStartPosition(Vector3Int position, int size)
        {
            return IsStartPosition(position.x, position.y, position.z, size);
        }

        public bool IsRandom(int r, int s, int q, int size)
        {
            Random.InitState(seed.Value + (Mathf.Abs(r) * Mathf.Abs(s) * Mathf.Abs(q)));
            return Random.Range(0, size) >= size / 2;
        }

        public bool IsRandomCircles(int r, int s, int q, int size)
        {
            Random.InitState(seed.Value + Mathf.Abs(r) + Mathf.Abs(s) + Mathf.Abs(q));
            return Random.Range(0, size) == 0;
        }

        public Vector3 GetPlayerStartPosition(int startPosition)
        {
            return Vector3.zero;
        }

        public void SetStartPosition()
        {
            Tile[] tiles = GetComponentsInChildren<Game.Board.Tile>();
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].owner == NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().id)
                {
                    NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Game.Player.Manager>().transform.position = tiles[i].transform.position;
                }
            }
        }
    }
}
