using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleanDialogue.Elements
{
    using Enumerations;

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
            TextField dialogueNameTextField = new TextField()
            {
                value = DialogueName
            };

            dialogueNameTextField.AddToClassList("cl-node__text-field");
            dialogueNameTextField.AddToClassList("cl-node__filename-text-field");
            dialogueNameTextField.AddToClassList("cl-node__text-field__hidden");

            titleContainer.Insert(0, dialogueNameTextField);

            // Input Container
            Port inputPort = InstantiatePort(
                Orientation.Horizontal, Direction.Input,
                Port.Capacity.Multi, typeof(bool));

            inputPort.portName = "Dialogue Connection";

            inputContainer.Add(inputPort);

            // Extentions container
            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("cl-node__custom-data-container");

            Foldout textFoldout = new Foldout()
            {
                text = "Dialogue Text"
            };

            TextField textField = new TextField()
            {
                value = Text
            };

            textField.AddToClassList("cl-node__text-field");
            textField.AddToClassList("cl-node__quote-text-field");

            textFoldout.Add(textField);
            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }
    }
}
