using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace CleanDialogue.Windows
{
    using Utilities;

    public class CLEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "DialoguesFileName";

        private Button saveButton;

        [MenuItem("Window/CL/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<CLEditorWindow>("Dialogue Graph");
        }

        private void CreateGUI()
        {
            AddGraphView();
            AddToolbar();

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

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            TextField fileNameTextField = CLElementUtilities.CreateTextField(defaultFileName, "File Name:");

            saveButton = CLElementUtilities.CreateButton("Save");

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);

            toolbar.AddStyleSheets("CLToolbarStyles");

            rootVisualElement.Add(toolbar);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("CLVariables");
        }

        #endregion

        #region Utility Methods

        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }

        #endregion
    }
}
