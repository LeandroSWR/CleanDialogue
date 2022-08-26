using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace CleanDialogue.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;
    using Data.Save;

    public class CLSingleChoiceNode : CLNode
    {
        public override void Initialize(CLGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            DialogueType = CLDialogueType.SingleChoice;

            CLChoiceSaveData choiceData = new CLChoiceSaveData()
            {
                Text = "Next Dialogue"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            // Output Container

            foreach (CLChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
