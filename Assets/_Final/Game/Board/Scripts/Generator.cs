using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Game.Board
{
    public class Generator : NetworkBehaviour
    {
        public int size;
        public NetworkVariable<int> seed = new NetworkVariable<int>(0);
        public List<GameObject> starts;
        public List<GameObject> trees;
        public List<GameObject> rocks;
        public List<GameObject> paths;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                size = Random.Range(5, 10);
                seed.Value = Random.Range(0, 1000);
                Generate();
                //GetComponent<FogOfWar>().Generate(NetworkManager.Singleton.LocalClient.ClientId);
                SetStartPosition();
            }
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

            int playerStartPosition = 0;

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
                            Vector3Int tilePosition = new Vector3Int(r, s, q);

                            Tile tile = null;

                            if (IsStartPosition(r, s, q, size))
                            {
                                ulong? playerId = GetPlayerIdFromStartPosition(playerStartPosition);

                                if (playerId != null)
                                {
                                    tile = Spawn(starts[0], position, tilePosition).GetComponent<Tile>();
                                    tile.GetComponent<NetworkObject>().ChangeOwnership(playerId.Value);
                                    tile.type.Value = TileType.Start;
                                } else
                                {
                                    tile = Spawn(paths[0], position, tilePosition).GetComponent<Tile>();
                                    tile.type.Value = TileType.Path;
                                    tile.GetComponent<NetworkObject>().ChangeOwnership(6);
                                }

                                playerStartPosition += 1;
                            } else if (IsPath(r, s, q, path))
                            {
                                tile = Spawn(paths[0], position, tilePosition).GetComponent<Tile>();
                                tile.type.Value = TileType.Path;
                                tile.GetComponent<NetworkObject>().ChangeOwnership(6);
                            } else if (IsRandom(r, s, q, size))
                            {
                                /*tile = Spawn(trees[0], position, tilePosition).GetComponent<Tile>();
                                tile.type.Value = TileType.Tree;
                                tile.GetComponent<NetworkObject>().ChangeOwnership(6);*/
                            } else
                            {
                                /*tile = Spawn(rocks[0], position, tilePosition).GetComponent<Tile>();
                                tile.type.Value = TileType.Rock;
                                tile.GetComponent<NetworkObject>().ChangeOwnership(6);*/
                            }
                            if (tile != null)
                            {
                                tile.position.Value = tilePosition;
                            }                                
                        }
                    }
                }
            }
        }

        public ulong? GetPlayerIdFromStartPosition(int startPosition)
        {
            ulong? playerId = null;

            for (int i = 0; i < Game.Manager.Instance.currentMode.playerSettings.Count; i++)
            {
                if (Game.Manager.Instance.currentMode.playerSettings[i].startPosition == startPosition)
                {
                    playerId = Game.Manager.Instance.currentMode.playerSettings[i].id;
                }
            }

            return playerId;
        }

        GameObject Spawn(GameObject prefab, Vector3 position, Vector3Int tilePosition)
        {
            GameObject go;
            go = GameObject.Instantiate(prefab, position, Quaternion.identity, transform);
            go.name = tilePosition.x + ":" + tilePosition.y + ":" + tilePosition.z;

            int rotation = Random.Range(0, 6);
            go.transform.Rotate(Vector3.up * rotation * 60);

            go.GetComponent<NetworkObject>().Spawn();
            go.GetComponent<NetworkObject>().TrySetParent(gameObject);

            return go;
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
                for (int j = 0; j < NetworkManager.ConnectedClients.Count; j++)
                {
                    if (tiles[i].OwnerClientId == NetworkManager.ConnectedClients[(ulong)j].ClientId)
                    {
                        NetworkManager.ConnectedClients[(ulong)j].PlayerObject.transform.position = tiles[i].transform.position;
                    }
                }
            }
        }
    }
}
