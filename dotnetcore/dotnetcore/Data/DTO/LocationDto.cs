namespace dotnetcore.Data
{
    public class LocationDto
    {
        public int ID { get; set;  }

        public string Country { get; set; }

        public string City { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int ContactUsID { get; set; }

        //public virtual ContactUs ContactUs { get; set; }

        //public virtual ICollection<Event> Events { get; set; }
    }
}