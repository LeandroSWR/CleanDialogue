using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace CleanDialogue.Elements
{
    using Enumerations;
    
    public class CLSingleChoiceNode : CLNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            DialogueType = CLDialogueType.SingleChoice;

            Choices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();

            // Output Container

            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

                choicePort.portName = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
