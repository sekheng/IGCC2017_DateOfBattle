using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGUI))]
public class GridEditor : Editor {

    GridGUI theGridsGameVisual;

    void OnEnable()
    {
        theGridsGameVisual = target as GridGUI;
    }

    public override void OnInspectorGUI()
    {
        // Create the inspector UI slider for easy visualization
        theGridsGameVisual.m_gridWidth = createTileSlider("Grid Width", theGridsGameVisual.m_gridWidth);
        theGridsGameVisual.m_gridHeight = createTileSlider("Grid Height", theGridsGameVisual.m_gridHeight);
        theGridsGameVisual.m_mapWidth = createTileSlider("Map Width", theGridsGameVisual.m_mapWidth, 256f, 100000);
        theGridsGameVisual.m_mapHeight = createTileSlider("Map Height", theGridsGameVisual.m_mapHeight, 256, 100000);

        if (GUILayout.Button("Open Grid Window"))
        {
            GridWindow theGridEditorWindow = EditorWindow.GetWindow<GridWindow>();
            theGridEditorWindow.init();
        }

    }

    float createTileSlider(string labelName, float sliderPosition, float minValue = 2f, float maxValue = 256f)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(labelName);
        sliderPosition = EditorGUILayout.Slider(sliderPosition, minValue, maxValue, null);
        GUILayout.EndHorizontal();

        return sliderPosition;
    }
}
