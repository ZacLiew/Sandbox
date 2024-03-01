using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR // => Ignore from here to next endif if not in editor
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            mapGen.GenerateMap();
        }

        //if (GUILayout.Button("Generate"))
        //{
        //    mapGen.GenerateMap();
        //}
    }

}

#endif
