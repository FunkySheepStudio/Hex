using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
    public class Generator : MonoBehaviour
    {
        public GameObject prefab;
        public int size;
        public int seed;

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

            for (int r = -size; r <= size; r++)
            {
                for (int s = -size; s <= size; s++)
                {
                    for (int q = -size; q <= size; q++)
                    {
                        if ((q + r + s) == 0)
                        {
                            Vector3 position = Vector3.left * r +
                            (Vector3.left * 0.5f + Vector3.forward * 0.8f) * s;

                            GameObject go = GameObject.Instantiate(prefab, position, Quaternion.identity, transform);
                            go.name = r + ":" + s + ":" + q;

                            if (IsStartPosition(r, s, q, size))
                            {
                                go.GetComponent<MeshRenderer>().material.color = Color.green;
                                break;
                            } else if (IsPath(r, s, q, path))
                            {
                                go.GetComponent<MeshRenderer>().material.color = Color.blue;
                                break;
                            }

                            if (IsRandom(r, s, q, size))
                                go.GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                    }
                }
            }
        }

        public List<Vector3Int> ComputePath(int size)
        {
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
            Random.InitState(seed + (Mathf.Abs(r) * Mathf.Abs(s) * Mathf.Abs(q)));
            return Random.Range(0, size) >= size / 2;
        }

        public bool IsRandomCircles(int r, int s, int q, int size)
        {
            Random.InitState(seed + Mathf.Abs(r) + Mathf.Abs(s) + Mathf.Abs(q));
            return Random.Range(0, size) == 0;
        }
    }
}
