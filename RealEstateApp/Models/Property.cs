using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RealEstateApp.Models
{
    public class Property : BaseModel
    {
        private double? _latitude = 0;
        private double? _longitude = 0;
        private string _address;

        public Property()
        {
            Id = Guid.NewGuid().ToString();

            ImageUrls = new List<string>();
        }

        public string Id { get; set; }

        public string Address
        {
            get => _address;
            set => SetField(ref _address, value);
        }

        public int? Price { get; set; }
        public string Description { get; set; }
        public int? Beds { get; set; }
        public int? Baths { get; set; }
        public int? Parking { get; set; }
        public int? LandSize { get; set; }
        public string AgentId { get; set; }
        public List<string> ImageUrls { get; set; }

        public double? Latitude
        {
            get => _latitude;
            set => SetField(ref _latitude, value);
        }

        public double? Longitude
        {
            get => _longitude;
            set => SetField(ref _longitude, value);
        }

        public string MainImageUrl => ImageUrls?.FirstOrDefault() ?? GlobalSettings.Instance.NoImageUrl;
    }
}
