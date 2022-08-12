using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace CleanDialogue.Elements
{
    using Enumerations;
    using UnityEngine.UIElements;

    public class CLMultipleChoiceNode : CLNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            DialogueType = CLDialogueType.MultipleChoice;

            Choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container
            Button addChoiceButton = new Button()
            {
                text = "Add Choice"
            };

            addChoiceButton.AddToClassList("cl-node__button");

            mainContainer.Insert(1, addChoiceButton);

            // Output Container

            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

                choicePort.portName = "";

                Button deleteChoiceButton = new Button()
                {
                    text = "X"
                };

                deleteChoiceButton.AddToClassList("cl-node__button");

                TextField choiceTextField = new TextField()
                {
                    value = choice
                };

                choiceTextField.AddToClassList("cl-node__text-field");
                choiceTextField.AddToClassList("cl-node__choice-text-field");
                choiceTextField.AddToClassList("cl-node__text-field__hidden");

                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
