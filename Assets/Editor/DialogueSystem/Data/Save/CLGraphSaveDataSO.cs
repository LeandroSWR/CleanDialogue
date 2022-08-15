using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleanDialogue.Data.Save
{
    public class CLGraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<CLGroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<CLNodeSaveData> Nodes { get; set; }
        [field: SerializeField] public List<string> OldGroupNames { get; set; }
        [field: SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            Groups = new List<CLGroupSaveData>();
            Nodes = new List<CLNodeSaveData>();
        }
    }
}
