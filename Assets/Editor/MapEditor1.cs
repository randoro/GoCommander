using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapGenerator1))]
public class NewBehaviourScript1 : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapGenerator1 mapGenerator = target as MapGenerator1;
        //mapGenerator1.GenerateMap();
    }
}

