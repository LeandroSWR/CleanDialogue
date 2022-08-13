using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace CleanDialogue.Windows
{
    using Utilities;

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

#region Elements Addition

        private void AddGraphView()
        {
            CLGraphView graphView = new CLGraphView(this);

            // Stretch the graph view to window size
            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
            
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("CLVariables");
        }

#endregion

    }
}
