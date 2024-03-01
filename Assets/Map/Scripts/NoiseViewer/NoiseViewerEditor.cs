using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR // => Ignore from here to next endif if not in editor
using UnityEditor;

[CustomEditor(typeof(NoiseViewer))]
public class NoiseViewerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseViewer mapGen = (NoiseViewer)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateNoiseMapViewer();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateNoiseMapViewer();
        }
    }
       
}

#endif
