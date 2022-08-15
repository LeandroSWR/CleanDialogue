using System.Collections.Generic;
using UnityEngine;

namespace CleanDialogue.ScriptableObjects
{
    public class CLDialogueContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public SerializableDictionary<CLDialogueGroupSO, List<CLDialogueSO>> DialogueGroups { get; set; }
        [field: SerializeField] public List<CLDialogueSO> UngroupedDialogues { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            DialogueGroups = new SerializableDictionary<CLDialogueGroupSO, List<CLDialogueSO>>();
            UngroupedDialogues = new List<CLDialogueSO>();
        }
    }
}
