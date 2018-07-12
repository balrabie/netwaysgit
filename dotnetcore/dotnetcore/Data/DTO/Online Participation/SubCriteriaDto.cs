
namespace dotnetcore.Data
{

    public class SubCriteriaDto : ItemDto
    {
        public string Comments { get; set; } // limit on # of words

        public byte[] Document { get; set; }  // limit on # of documents

        public int CriteriaID { get; set; }

        public int UserID { get; set; }
    }
}