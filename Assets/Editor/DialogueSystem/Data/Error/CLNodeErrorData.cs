using System.Collections.Generic;

namespace CleanDialogue.Data.Error
{
    using Elements;

    public class CLNodeErrorData
    {
        public CLErrorData ErrorData { get; set; }
        public List<CLNode> Nodes { get; set; }

        public CLNodeErrorData()
        {
            ErrorData = new CLErrorData();
            Nodes = new List<CLNode>();
        }
    }
}
