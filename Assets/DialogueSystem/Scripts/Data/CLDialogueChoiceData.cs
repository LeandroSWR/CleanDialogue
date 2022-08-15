using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleanDialogue.Data
{
    using ScriptableObjects;

    [Serializable]
    public class CLDialogueChoiceData
    { 
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public CLDialogueSO NextDialogue { get; set; }
    }
}
