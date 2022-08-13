using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleanDialogue.Windows
{
    using Data.Error;
    using Elements;
    using Enumerations;
    using Utilities;

    public class CLGraphView : GraphView
    {
        private CLEditorWindow editorWindow;
        private CLSearchWindow searchWindow;

        private SerializableDictionary<string, CLNodeErrorData> ungroupedNodes;

        public CLGraphView(CLEditorWindow clEditorWindow)
        {
            editorWindow = clEditorWindow;

            ungroupedNodes = new SerializableDictionary<string, CLNodeErrorData>();

            AddManipulators();
            AddSeachWindow();
            AddGridBackground();

            OnElementsDeleted();

            AddStyles();
        }

        

        #region Overrided Methods

        // Port connections between nodes
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port || 
                startPort.node == port.node || 
                startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);

            });

            return compatiblePorts;
        }

        #endregion

        #region Manipulators

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", CLDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", CLDialogueType.MultipleChoice));

            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateGroupContextualMenu() =>
            new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    "Add Group",
                    actionEvent => AddElement(
                        CreateGroup(
                            "DialogueGroup", 
                            GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

        private IManipulator CreateNodeContextualMenu(string actionTitle, CLDialogueType dialogueType) =>
            new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    actionTitle, 
                    actionEvent => AddElement(
                        CreateNode(
                            dialogueType, 
                            GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

        #endregion

        #region Element Creation

        public Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }

        public CLNode CreateNode(CLDialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"CleanDialogue.Elements.CL{dialogueType}Node");
            CLNode node = (CLNode)Activator.CreateInstance(nodeType);

            node.Initialize(this, position);
            node.Draw();

            AddUngroupedNode(node);

            return node;
        }

        #endregion

        #region Callbacks
        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                for (int i = selection.Count - 1; i >= 0; i--)
                {
                    if (selection[i] is CLNode)
                    {
                        RemoveUngroupedNode((CLNode)selection[i]);

                        RemoveElement((CLNode)selection[i]);
                    }
                }
            };
        }
        #endregion

        #region Repeated Elements

        public void AddUngroupedNode(CLNode node)
        {
            string nodeName = node.DialogueName;

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                CLNodeErrorData nodeErrorData = new CLNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);

                return;
            }

            ungroupedNodes[nodeName].Nodes.Add(node);

            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if (ungroupedNodes[nodeName].Nodes.Count == 2)
            {
                ungroupedNodes[nodeName].Nodes[0].SetErrorStyle(errorColor);
            }

            Debug.LogWarning($"There's multiple nodes with the same name: \"{nodeName}\".");
        }

        public void RemoveUngroupedNode(CLNode node)
        {
            ungroupedNodes[node.DialogueName].Nodes.Remove(node);

            node.ResetStyle();

            if (ungroupedNodes[node.DialogueName].Nodes.Count == 1)
            {
                ungroupedNodes[node.DialogueName].Nodes[0].ResetStyle();
            }
            else if (ungroupedNodes[node.DialogueName].Nodes.Count == 0)
            {
                ungroupedNodes.Remove(node.DialogueName);
            }
        }

        #endregion

        #region Element Addition

        private void AddSeachWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<CLSearchWindow>();

                searchWindow.Initialize(this);
            }

            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "CLGraphViewStyles",
                "CLNodeStyles"
                );
        }

        #endregion

        #region Utilities

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        #endregion
    }
}
