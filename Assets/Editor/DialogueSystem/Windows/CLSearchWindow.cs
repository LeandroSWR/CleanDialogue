using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CleanDialogue.Windows
{
    using Enumerations;
    using Elements;

    public class CLSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private CLGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(CLGraphView cLGraphView)
        {
            graphView = cLGraphView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", indentationIcon))
                {
                    level = 2,
                    userData = CLDialogueType.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", indentationIcon))
                {
                    level = 2,
                    userData = CLDialogueType.MultipleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = 
                graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (SearchTreeEntry.userData)
            {
                // Create a single choice node and return true to close the search window
                case CLDialogueType.SingleChoice:
                    CLSingleChoiceNode singleChoiceNode = 
                        (CLSingleChoiceNode) graphView.CreateNode(
                            CLDialogueType.SingleChoice,
                            localMousePosition);
                    graphView.AddElement(singleChoiceNode);
                    return true;

                // Create a multiple choice node and return true to close the search window
                case CLDialogueType.MultipleChoice:
                    CLMultipleChoiceNode multipleChoiceNode =
                        (CLMultipleChoiceNode) graphView.CreateNode(
                            CLDialogueType.MultipleChoice,
                            localMousePosition);
                    graphView.AddElement(multipleChoiceNode);
                    return true;

                // Create a group and return true to close the search window
                case Group _:
                    Group group = graphView.CreateGroup("Dialogue Group", localMousePosition);
                    graphView.AddElement(group);
                    return true;

                default:
                    return false;
            }
        }
    }
}
