using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeDataScript), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDrawerScript : Editor
{
    private ShapeDataScript ShapeScriptInstance => target as ShapeDataScript;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnInputFields();
        EditorGUILayout.Space();

        if(ShapeScriptInstance.board != null && ShapeScriptInstance.columns > 0 && ShapeScriptInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapeScriptInstance);
        }

    }

    private void ClearBoardButton()
    {
        if(GUILayout.Button("Clear Board"))
        {
            ShapeScriptInstance.Clear();
        }
    }

    private void DrawColumnInputFields()
    {
        var columnTemp = ShapeScriptInstance.columns;
        var rowTemp = ShapeScriptInstance.rows;

        ShapeScriptInstance.columns = EditorGUILayout.IntField("Columns", ShapeScriptInstance.columns);
        ShapeScriptInstance.rows = EditorGUILayout.IntField("Rows", ShapeScriptInstance.rows);


        if ((ShapeScriptInstance.columns != columnTemp || ShapeScriptInstance.rows != rowTemp) && ShapeScriptInstance.columns > 0 && ShapeScriptInstance.rows > 0)
        {
            ShapeScriptInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for(var row = 0; row < ShapeScriptInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);
            for(var column = 0; column < ShapeScriptInstance.columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapeScriptInstance.board[row].column[column], dataFieldStyle);
                ShapeScriptInstance.board[row].column[column] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
