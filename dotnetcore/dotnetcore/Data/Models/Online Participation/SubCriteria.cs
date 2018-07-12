
using System.Collections.Generic;

namespace dotnetcore.Data
{
    public class SubCriteria: Item
    {
        public string Comments { get; set; }

        public byte[] Document { get; set; }  // byte[] or file[] or filestream[]? limit on # of documents

        public int CriteriaID { get; set; }

        public virtual Criteria Criteria { get; set; }        

    }
}