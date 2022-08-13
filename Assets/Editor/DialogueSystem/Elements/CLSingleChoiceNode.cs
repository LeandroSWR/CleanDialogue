using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace CleanDialogue.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;

    public class CLSingleChoiceNode : CLNode
    {
        public override void Initialize(CLGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);

            DialogueType = CLDialogueType.SingleChoice;

            Choices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();

            // Output Container

            foreach (string choice in Choices)
            {
                Port choicePort = this.CreatePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
