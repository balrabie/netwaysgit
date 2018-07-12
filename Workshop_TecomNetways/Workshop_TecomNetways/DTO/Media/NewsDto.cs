using System.ComponentModel.DataAnnotations;

namespace Workshop_TecomNetways.DTO
{

    public class NewsDto : MediaItemDto
    {
        public string Details { get; set; }
    }
}