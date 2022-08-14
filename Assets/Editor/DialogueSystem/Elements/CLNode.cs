using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleanDialogue.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;

    public class CLNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }

        public CLDialogueType DialogueType { get; set; }
        public CLGroup Group { get; set; }

        private CLGraphView graphView;
        private Color defaultBackgroundColor;

        public virtual void Initialize(CLGraphView graphView, Vector2 position)
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue Text";

            this.graphView = graphView;

            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("cl-node__main-container");
            extensionContainer.AddToClassList("cl-node__extension-container");
        }

        public virtual void Draw()
        {
            // Title Container
            TextField dialogueNameTextField = CLElementUtilities.CreateTextField(DialogueName, null, callback => 
            {
                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);

                    DialogueName = callback.newValue;

                    graphView.AddUngroupedNode(this);
                }
                else
                {
                    CLGroup currentGroup = Group;

                    graphView.RemoveGroupedNode(this, Group);
                    
                    DialogueName = callback.newValue;

                    graphView.AddGroupedNode(this, currentGroup);
                }
            });

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

        #region Overrided Methods

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction(
                "Disconnect Input Ports", 
                actionEvent => DisconnectPorts(inputContainer)
            );
            evt.menu.AppendAction(
                "Disconnect Output Ports", 
                actionEvent => DisconnectPorts(outputContainer)
            );

            base.BuildContextualMenu(evt);
        }

        #endregion

        #region Utility Methods

        public void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (port.connected)
                {
                    graphView.DeleteElements(port.connections);
                }
            }
        }

        public void SetErrorStyle(Color color) =>
            mainContainer.style.backgroundColor = color;

        public void ResetStyle() =>
            mainContainer.style.backgroundColor = defaultBackgroundColor;

        #endregion
    }
}
