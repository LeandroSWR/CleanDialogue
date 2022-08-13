using UnityEditor;
using UnityEngine.UIElements;

namespace CleanDialogue.Utilities
{
    public static class CLStyleUtility
    {
        public static VisualElement AddClasses(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.AddToClassList(className);
            }

            return element;
        }

        public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
        {
            foreach (string styleSheetName in styleSheetNames)
            {
                StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load($"DialogueSystem/{styleSheetName}.uss");

                element.styleSheets.Add(styleSheet);
            }

            return element;
        }
    }
}
