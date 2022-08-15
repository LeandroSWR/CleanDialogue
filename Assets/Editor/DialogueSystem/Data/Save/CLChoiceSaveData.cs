using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleanDialogue.Data.Save
{
    [Serializable]
    public class CLChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}
