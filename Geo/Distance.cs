    public class Distance : IQueryPropertyBinder
    {
        public DistanceUnits Units { get; set; }
        public double Value { get; set; }

        
        public enum DistanceUnits
        {
            Meter,
            Km,
            Mile
        }

        private double Meters
        {
            get
            {
                switch (Units)
                {
                    case DistanceUnits.Km:
                        return Convert.ToInt32(Value * 1000);

                    case DistanceUnits.Meter:
                        return Convert.ToInt32(Value);

                    case DistanceUnits.Mile:
                        return Convert.ToInt32(Value * 1609.34);

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public Distance(DistanceUnits units, double value)
        {
            Units = units;
            Value = value;
        }

        public Distance ToUnits(DistanceUnits units)
        {
            switch (units)
            {
                case DistanceUnits.Km:
                    return new Distance(DistanceUnits.Km, Math.Round(Meters / 1000.0, 1));

                case DistanceUnits.Meter:
                    return new Distance(DistanceUnits.Meter, Meters);

                case DistanceUnits.Mile:
                    return new Distance(DistanceUnits.Mile, Math.Round(Meters * 0.000621371, 1));

                default:
                    throw new NotSupportedException();

            }
        }


        public Distance Km
        {
            get
            {
                return ToUnits(DistanceUnits.Km);    
            }
            
        }

        public Distance Meter
        {
            get
            {
                return ToUnits(DistanceUnits.Meter);    
            }
            
        }

        public Distance Mile
        {
            get
            {
                return ToUnits(DistanceUnits.Mile);    
            }
            
        }

        public static bool operator >(Distance x, Distance y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Meters > y.Meters;
        }

        public static bool operator >=(Distance x, Distance y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Meters >= y.Meters;
        }

        public static bool operator <(Distance x, Distance y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Meters < y.Meters;
        }

        public static bool operator <=(Distance x, Distance y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Meters <= y.Meters;
        }
        public static bool operator ==(Distance a, Distance b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Meters.Equals(b.Meters);
        }

        public static bool operator !=(Distance a, Distance b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var distance = (Distance) obj;
            return distance.Units == Units && distance.Value.Equals(Value);

        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Value, Units);
        }
    }