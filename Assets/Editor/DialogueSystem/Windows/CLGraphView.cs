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

            this.AddManipulator(CreateNodeContextualMenu());
            this.AddManipulator(new ContentDragger());
        }

        private IManipulator CreateNodeContextualMenu() =>
            new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    "Add Node", 
                    actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition)))
            );

        private CLNode CreateNode(Vector2 position)
        {
            CLNode node = new CLNode();

            node.Initialize(position);
            node.Draw();

            return node;
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/CLGraphViewStyles.uss");

            styleSheets.Add(styleSheet);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }
    }
}
