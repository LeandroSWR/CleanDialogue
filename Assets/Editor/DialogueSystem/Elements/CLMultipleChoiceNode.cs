using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace CleanDialogue.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class CLMultipleChoiceNode : CLNode
    {
        public override void Initialize(CLGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            DialogueType = CLDialogueType.MultipleChoice;

            CLChoiceSaveData choiceData = new CLChoiceSaveData()
            {
                Text = "New Choice"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container
            Button addChoiceButton = CLElementUtilities.CreateButton("Add Choice", () =>
            {
                CLChoiceSaveData choiceData = new CLChoiceSaveData()
                {
                    Text = "New Choice"
                };

                Choices.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("cl-node__button");

            mainContainer.Insert(1, addChoiceButton);

            // Output Container

            foreach (CLChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        #region Element Creation

        private Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            CLChoiceSaveData choiceData = (CLChoiceSaveData) userData;

            Button deleteChoiceButton = CLElementUtilities.CreateButton("X", () =>
            {
                if(Choices.Count > 1)
                {
                    if (choicePort.connected)
                    {
                        graphView.DeleteElements(choicePort.connections);
                    }

                    Choices.Remove(choiceData);

                    graphView.RemoveElement(choicePort);
                }
            });

            deleteChoiceButton.AddToClassList("cl-node__button");

            TextField choiceTextField = CLElementUtilities.CreateTextField(choiceData.Text, null, callback => 
            {
                choiceData.Text = callback.newValue;
            });

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
