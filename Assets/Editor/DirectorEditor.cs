using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Director))]
public class DirectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Director myTarget = (Director)target;

        EditorGUILayout.HelpBox(
            "░░░░░███████]▄▄▄▄▄▄▄▄▃\n" +
            "░░▂▄▅█████████▅▄▃▂\n" +
            "[███████████████████].\n" +
            "░◥⊙▲⊙▲⊙▲⊙▲⊙▲⊙▲⊙◤...\n"
            , MessageType.None);

        if (GUILayout.Button("¯\\_(ツ)_/¯"))
        {
            myTarget.EditorTest();
        }

        EditorGUILayout.HelpBox(
            "───────────████████\n" +
            "──────────███▄███████\n" +
            "──────────███████████\n" +
            "──────────███████████\n" +
            "──────────██████\n" +
            "──────────█████████\n" +
            "█───────███████\n" +
            "██────████████████\n" +
            "███──██████████──█\n" +
            "███████████████\n" +
            "███████████████\n" +
            "─█████████████\n" +
            "──███████████\n" +
            "────████████\n" +
            "─────███──██\n" +
            "─────██────█\n" +
            "─────█─────█\n" +
            "─────██────██", MessageType.None);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawDefaultInspector();
    }
}
