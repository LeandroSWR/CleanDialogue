using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CleanDialogue.Data.Error
{
    using Elements;

    public class CLGroupErrorData
    {
        public CLErrorData ErrorData { get; set; }
        public List<CLGroup> Groups { get; set; }

        public CLGroupErrorData()
        {
            ErrorData = new CLErrorData();
            Groups = new List<CLGroup>();
        }
    }
}
