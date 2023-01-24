using System.Collections.Generic;
using UnityEngine;

namespace Game.Cards
{
    public class Manager : MonoBehaviour
    {
        public Camera cam;
        public GameObject template;
        public float rotationSpeed = 200;
        public List<Card> pool;
        List<GameObject> cards = new List<GameObject>();
        float targetRotation = 0;
        float currentRotation = 0;

        private void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                AddCard(pool[i]);
            }
        }

        private void Update()
        {
            if (targetRotation != 0)
            {
                currentRotation = Mathf.Sign(targetRotation) * Time.deltaTime * rotationSpeed;

                if (Mathf.Abs(targetRotation) <= Mathf.Abs(currentRotation))
                {
                    transform.Rotate(new Vector3(0, targetRotation, 0));
                    targetRotation = 0;
                    currentRotation = 0;
                } else
                {
                    transform.Rotate(new Vector3(0, currentRotation, 0));
                    targetRotation -= currentRotation;
                }
                Align();
            }
        }

        void AddCard(Card card)
        {
            GameObject cardGo = Instantiate(template, Vector3.zero, Quaternion.Euler(0, 180, 0),  transform);

            cardGo.GetComponent<MeshRenderer>().materials[0].SetFloat("_Level", card.level / 10f);
            cardGo.transform.Find("Background").GetComponent<MeshRenderer>().materials[0].SetTexture("_background", card.background);
            cards.Add(cardGo);
            Flush();
        }

        void Flush()
        {
            Vector3 center = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 35));
            transform.localPosition = center;

            float distance = Vector3.Distance(center, cam.transform.position) / 1.5f;

            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 position = GetPosition(i, center, distance);
                transform.GetChild(i).position = position;
            }

            Align();
        }

        Vector3 GetPosition(int index, Vector3 center, float distance)
        {
            float radians = 2 * Mathf.PI / transform.childCount * index;
            var x = Mathf.Sin(radians);
            var z = Mathf.Cos(radians);
            Vector3 spawnDir = new Vector3(x, 0, z);

            return center - spawnDir * distance;
        }

        public void Rotate(Vector2Int direction)
        {
            if (targetRotation == 0)
            {
                targetRotation = -direction.x * 360 / transform.childCount;
            }
        }

        public void RotateLeft()
        {
            if (targetRotation == 0)
            {
                targetRotation = 360 / transform.childCount;
            }
        }

        public void RotateRight()
        {
            if (targetRotation == 0)
            {
                targetRotation = -360 / transform.childCount;
            }
        }

        void Align()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                var lookPos = cam.transform.position - child.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                child.rotation = rotation;
            }
        }
    }
}
