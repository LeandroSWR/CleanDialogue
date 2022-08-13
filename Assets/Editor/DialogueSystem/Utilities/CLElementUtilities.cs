using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace CleanDialogue.Utilities
{
    using Elements;

    public static class CLElementUtilities
    {
        public static Button CreateButton(string text, Action onClick = null) =>
            new Button(onClick)
            {
                text = text
            };

        public static Foldout CreateFoldout(string title, bool collapsed = false) =>
            new Foldout()
            {
                text = title,
                value = !collapsed
            };

        public static Port CreatePort(
            this CLNode node, string portName = "", 
            Orientation orientation = Orientation.Horizontal, 
            Direction direction = Direction.Output, 
            Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.portName = portName;

            return port;
        }

        public static TextField CreateTextField(string value = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value
            };

            if(onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, onValueChanged);

            textArea.multiline = true;

            return textArea;
        }
    }
}
