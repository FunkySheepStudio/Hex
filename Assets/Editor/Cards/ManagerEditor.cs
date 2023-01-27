#if UNITY_EDITOR
using Game.Board;
using UnityEditor;
using UnityEngine;

namespace Game.Cards
{
    [CustomEditor(typeof(Manager))]
    public class ManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Manager manager = (Manager)target;

            if (GUILayout.Button("Rotate Left"))
            {
                manager.RotateLeft();
            }

            if (GUILayout.Button("Rotate Right"))
            {
                manager.RotateRight();
            }
        }
    }
}
#endif