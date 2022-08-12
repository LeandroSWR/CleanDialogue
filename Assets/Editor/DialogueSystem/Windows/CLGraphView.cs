using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleanDialogue.Windows
{
    using Elements;
    using Enumerations;

    public class CLGraphView : GraphView
    {
        public CLGraphView()
        {
            AddManipulators();

            AddGridBackground();

            AddStyles();
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", CLDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", CLDialogueType.MultipleChoice));
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, CLDialogueType dialogueType) =>
            new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    actionTitle, 
                    actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition)))
            );

        private CLNode CreateNode(CLDialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"CleanDialogue.Elements.CL{dialogueType}Node");
            CLNode node = (CLNode)Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            return node;
        }

        private void AddStyles()
        {
            StyleSheet graphViewStyles = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/CLGraphViewStyles.uss");
            StyleSheet nodeStyles = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/CLNodeStyles.uss");

            styleSheets.Add(graphViewStyles);
            styleSheets.Add(nodeStyles);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }
    }
}
