#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Board
{
    [CustomEditor(typeof(Generator))]
    public class GeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Generator generator = (Generator)target;

            if (GUILayout.Button("Generate"))
            {
                generator.Clear();
                generator.Generate();
            }
        }
}
}
#endif