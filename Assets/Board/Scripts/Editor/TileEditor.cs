using UnityEditor;
using UnityEngine;

namespace Game.Board
{
    [CustomEditor(typeof(Tile))]
    public class TileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Tile tile = (Tile)target;

            if (GUILayout.Button("Select"))
            {
                tile.Select();
            }
        }
}
}
