using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace CleanDialogue.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;

    public class CLMultipleChoiceNode : CLNode
    {
        public override void Initialize(CLGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            DialogueType = CLDialogueType.MultipleChoice;

            Choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container
            Button addChoiceButton = CLElementUtilities.CreateButton("Add Choice", () =>
            {
                Port choicePort = CreateChoicePort("New Choice");

                Choices.Add("New Choice");

                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("cl-node__button");

            mainContainer.Insert(1, addChoiceButton);

            // Output Container

            foreach (string choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        #region Element Creation

        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort();

            Button deleteChoiceButton = CLElementUtilities.CreateButton("X");

            deleteChoiceButton.AddToClassList("cl-node__button");

            TextField choiceTextField = CLElementUtilities.CreateTextField(choice);

            choiceTextField.AddClasses(
                "cl-node__text-field",
                "cl-node__choice-text-field",
                "cl-node__text-field__hidden"
            );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }

        #endregion
    }
}
