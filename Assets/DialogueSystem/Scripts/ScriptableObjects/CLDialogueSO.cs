using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleanDialogue.ScriptableObjects
{
    using Enumerations;
    using Data;

    public class CLDialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<CLDialogueChoiceData> Choices { get; set; }
        [field: SerializeField] public CLDialogueType DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, string text, List<CLDialogueChoiceData> choices, CLDialogueType dialogueType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            Text = text;
            Choices = choices;
            DialogueType = dialogueType;
            isStartingDialogue = IsStartingDialogue;
        }
    }
}
