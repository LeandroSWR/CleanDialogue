using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleanDialogue.Data.Save
{
    using Enumerations;

    [Serializable]
    public class CLNodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public List<CLChoiceSaveData> Choices { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public CLDialogueType DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}
