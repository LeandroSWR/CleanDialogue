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

        private CLGraphView graphView;
        private static TextField fileNameTextField;
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
            graphView = new CLGraphView(this);

            // Stretch the graph view to window size
            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
            
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = CLElementUtilities.CreateTextField(
                defaultFileName, 
                "File Name:", 
                callback => 
                {
                    fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                });

            saveButton = CLElementUtilities.CreateButton("Save", () => Save());

            Button clearButton = CLElementUtilities.CreateButton("Clear", () => Clear());
            Button resetButton = CLElementUtilities.CreateButton("Reset", () => ResetGraph());
            
            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);

            toolbar.AddStyleSheets("CLToolbarStyles");

            rootVisualElement.Add(toolbar);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("CLVariables");
        }

        #endregion

        #region Toolbar Actions

        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog(
                    "Invalid file name.",
                    "Please ensure the file name you've typed in is valid.",
                    "Ok."
                );

                return;
            }

            CLIOUtility.Initialize(graphView, fileNameTextField.value);
            CLIOUtility.Save();
        }

        private void Clear()
        {
            graphView.ClearGraph();
        }

        private void ResetGraph()
        {
            Clear();
            UpdateFileName(defaultFileName);
        }

        #endregion


        #region Utility Methods

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }

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
