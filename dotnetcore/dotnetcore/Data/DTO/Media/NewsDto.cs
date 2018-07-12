using System.ComponentModel.DataAnnotations;

namespace dotnetcore.Data
{

    public class NewsDto : MediaItemDto
    {
        public string Details { get; set; }
    }
}