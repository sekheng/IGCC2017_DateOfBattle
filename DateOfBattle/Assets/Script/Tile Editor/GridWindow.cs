using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridWindow : EditorWindow {
    GridGUI theGridVisual;

    public void init()
    {
        theGridVisual = FindObjectOfType<GridGUI>();
    }
	
    void OnGUI()
    {
        theGridVisual.m_gridOutlineColor = EditorGUILayout.ColorField(theGridVisual.m_gridOutlineColor, GUILayout.Width(100));
    }
}
