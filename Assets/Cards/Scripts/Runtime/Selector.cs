using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Game.Cards
{
    public class Selector : MonoBehaviour
    {
        public Camera cam;
        public Manager manager;

        private void Update()
        {
            if (Touch.activeFingers.Count == 1 && manager.targetRotation == 0)
            {
                Touch activeTouch = Touch.activeFingers[0].currentTouch;

                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(activeTouch.screenPosition);

                if (Physics.Raycast(ray, out hit) && activeTouch.began)
                {
                    if (hit.transform.gameObject == manager.currentCard)
                    {
                        manager.currentCard.GetComponent<MeshRenderer>().material.SetInt("_Selected", 1);
                    } else
                    {
                        manager.currentCard.GetComponent<MeshRenderer>().material.SetInt("_Selected", 0);
                    }
                } else
                {
                    manager.currentCard.GetComponent<MeshRenderer>().material.SetInt("_Selected", 0);
                }
            } else
            {
                manager.currentCard.GetComponent<MeshRenderer>().material.SetInt("_Selected", 0);
            }
        }
    }
}
