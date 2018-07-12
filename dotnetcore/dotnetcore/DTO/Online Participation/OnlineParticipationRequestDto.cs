using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop_TecomNetways.DTO
{
    public class OnlineParticipationRequestDto: ItemDto
    {
        public int UserID { get; set; }

        public int CountryID { get; set; }
    }
}