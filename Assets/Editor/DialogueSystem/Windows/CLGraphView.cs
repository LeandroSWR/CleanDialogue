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

        private SerializableDictionary<string, CLGroupErrorData> groups;
        private SerializableDictionary<string, CLNodeErrorData> ungroupedNodes;
        private SerializableDictionary<Group, SerializableDictionary<string, CLNodeErrorData>> groupedNodes;

        private int repeatedNamesAmount;
        public int RepeatedNamesAmount
        {
            get => repeatedNamesAmount;
            set
            {
                repeatedNamesAmount = value;

                if (repeatedNamesAmount == 0)
                {
                    editorWindow.EnableSaving();
                }
                else
                {
                    editorWindow.DisableSaving();
                }
            }
        }

        public CLGraphView(CLEditorWindow clEditorWindow)
        {
            editorWindow = clEditorWindow;

            groups = new SerializableDictionary<string, CLGroupErrorData>();
            ungroupedNodes = new SerializableDictionary<string, CLNodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, CLNodeErrorData>>();

            AddManipulators();
            AddSeachWindow();
            AddGridBackground();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();

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
                    actionEvent => CreateGroup(
                        "DialogueGroup", 
                        GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
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

        public CLGroup CreateGroup(string title, Vector2 localMousePosition) 
        {
            CLGroup group = new CLGroup(title, localMousePosition);

            AddGroup(group);

            AddElement(group);

            foreach(GraphElement selectedElement in selection)
            {
                if (selectedElement is CLNode)
                {
                    group.AddElement((CLNode) selectedElement);
                }
            }

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
                Type groupType = typeof(CLGroup);
                Type edgeType = typeof(Edge);

                List<Edge> edgesToDelete = new List<Edge>();

                for (int i = selection.Count - 1; i >= 0; i--)
                {
                    if (selection[i] is CLNode)
                    {
                        if (((CLNode)selection[i]).Group != null)
                        {
                            ((CLNode)selection[i]).Group.RemoveElement((CLNode)selection[i]);
                        }

                        RemoveUngroupedNode((CLNode)selection[i]);

                        ((CLNode)selection[i]).DisconnectAllPorts();

                        RemoveElement((CLNode)selection[i]);
                    }
                    else if (selection[i].GetType() == groupType)
                    {
                        RemoveGroup((CLGroup)selection[i]);

                        List<CLNode> groupNodes = new List<CLNode>();

                        foreach (GraphElement groupElement in ((CLGroup) selection[i]).containedElements)
                        {
                            if (groupElement is CLNode)
                            {
                                groupNodes.Add((CLNode)groupElement);
                            }
                        }

                        ((CLGroup)selection[i]).RemoveElements(groupNodes);

                        RemoveElement((CLGroup)selection[i]);
                    }
                    else if (selection[i].GetType() == edgeType)
                    {
                        edgesToDelete.Add((Edge)selection[i]);
                    }
                }

                DeleteElements(edgesToDelete);
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is CLNode))
                    {
                        continue;
                    }

                    CLNode node = (CLNode) element;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, (CLGroup)group);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is CLNode))
                    {
                        continue;
                    }

                    CLNode node = (CLNode)element;

                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);
                }
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                ((CLGroup)group).title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

                RemoveGroup((CLGroup) group);

                ((CLGroup) group).OldTitle = ((CLGroup)group).title;

                AddGroup((CLGroup)group);
            };
        }

        #endregion

        #region Repeated Elements

        public void AddUngroupedNode(CLNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                CLNodeErrorData nodeErrorData = new CLNodeErrorData();

                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);

                return;
            }

            List<CLNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);

            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ++RepeatedNamesAmount;

                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }

            Debug.LogWarning($"There's multiple nodes with the same name: \"{node.DialogueName}\".");
        }

        public void RemoveUngroupedNode(CLNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            List<CLNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                --RepeatedNamesAmount;

                ungroupedNodesList[0].ResetStyle();
            }
            else if (ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }
        }

        private void AddGroup(CLGroup group)
        {
            string groupName = group.title.ToLower();

            if (!groups.ContainsKey(groupName))
            {
                CLGroupErrorData groupErrorData = new CLGroupErrorData();

                groupErrorData.Groups.Add(group);

                groups.Add(groupName, groupErrorData);

                return;
            }

            List<CLGroup> groupsList = groups[groupName].Groups;

            groupsList.Add(group);

            Color errorColor = groups[groupName].ErrorData.Color;
            group.SetErrorStyle(errorColor);

            if (groupsList.Count == 2)
            {
                ++RepeatedNamesAmount;

                groupsList[0].SetErrorStyle(errorColor);
            }

            Debug.LogWarning($"There's multiple groups with the same name: \"{group.title}\".");
        }

        private void RemoveGroup(CLGroup group)
        {
            string oldGroupName = group.OldTitle.ToLower();

            List<CLGroup> groupsList = groups[oldGroupName].Groups;

            groupsList.Remove(group);

            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --RepeatedNamesAmount;

                groupsList[0].ResetStyle();
            }
            else if (groupsList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }

        public void AddGroupedNode(CLNode node, CLGroup group)
        {
            string nodeName = node.DialogueName.ToLower();

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, CLNodeErrorData>());
            }

            node.Group = group;

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                CLNodeErrorData nodeErrorData = new CLNodeErrorData();
                nodeErrorData.Nodes.Add(node);
                groupedNodes[group].Add(nodeName, nodeErrorData);
                
                return;
            }

            CLNodeErrorData clNodeErrorData = groupedNodes[group][nodeName];

            clNodeErrorData.Nodes.Add(node);

            Color errorColor = clNodeErrorData.ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (clNodeErrorData.Nodes.Count == 2)
            {
                ++RepeatedNamesAmount;

                clNodeErrorData.Nodes[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(CLNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();

            List<CLNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Remove(node);

            node.Group = null;

            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                --RepeatedNamesAmount;

                groupedNodesList[0].ResetStyle();
            }
            else if (groupedNodesList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);

                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
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
