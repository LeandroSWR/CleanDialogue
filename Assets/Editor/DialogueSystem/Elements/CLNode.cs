using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleanDialogue.Elements
{
    using Enumerations;
    using Utilities;

    public class CLNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public CLDialogueType DialogueType { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue Text";

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("cl-node__main-container");
            extensionContainer.AddToClassList("cl-node__extension-container");
        }

        public virtual void Draw()
        {
            // Title Container
            TextField dialogueNameTextField = CLElementUtilities.CreateTextField(DialogueName);

            dialogueNameTextField.AddClasses(
                "cl-node__text-field",
                "cl-node__filename-text-field",
                "cl-node__text-field__hidden"
            );

            titleContainer.Insert(0, dialogueNameTextField);

            // Input Container
            Port inputPort = this.CreatePort("Dialogue Connection",
                Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            // Extentions container
            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("cl-node__custom-data-container");

            Foldout textFoldout = CLElementUtilities.CreateFoldout("Dialogue Text");

            TextField textField = CLElementUtilities.CreateTextArea(Text);

            textField.AddClasses(
                "cl-node__text-field",
                "cl-node__quote-text-field"
            );

            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }
    }
}
