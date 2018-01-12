
    public class GeoPosition 
    {
        public GeoPosition()
        {
            Lat = 0;
            Lng = 0;
        }

        public GeoPosition(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }


        public GeoPosition(decimal lat, decimal lng)
            : this(Convert.ToDouble(lat), Convert.ToDouble(lng))
        {
            
        }

        public GeoPosition(double? lat, double? lng) : this()
        {
            if (lat.HasValue && lng.HasValue)
            {
                Lat = lat.Value;
                Lng = lng.Value;
            }
        }

        public double Lat { get; set; }
        public double Lng { get; set; }

        public bool IsValid()
        {
            if (Lat < 0 || Lat > 0)
            {
                return true;
            }

            if (Lng < 0 || Lng > 0)
            {
                return true;
            }

            return false;
        }
    }