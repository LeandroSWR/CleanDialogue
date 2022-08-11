using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace CleanDialogue.Windows
{
    public class CLEditorWindow : EditorWindow
    {
        [MenuItem("Window/CL/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<CLEditorWindow>("Dialogue Graph");
        }

        private void CreateGUI()
        {
            AddGraphView();

            AddStyles();
        }

        private void AddGraphView()
        {
            CLGraphView graphView = new CLGraphView();

            // Stretch the graph view to window size
            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
            
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/CLVariables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}
