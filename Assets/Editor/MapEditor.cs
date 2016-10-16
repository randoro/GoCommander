using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapGenerator mapGenerator = target as MapGenerator;
        //mapGenerator.GenerateMap();
    }
}
