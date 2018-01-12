    public struct Weight : IComparable<Weight>
    {
        public enum Units
        {
            Kg = 1, Lbs = 2
        }

        public double Value { get; set; }
        //TODO since we have shifted the weight to start from 1 all the existing 0 values in the DB are not adjusted any more.
        private Units _unit;

        /// <summary>
        /// Create weight object with Kg units
        /// </summary>
        /// <param name="weight"></param>
        public Weight(double weight)
            : this()
        {
            Value = weight;
            Unit = Units.Kg;
        }

        public Weight(Units unit, double weight)
            : this()
        {
            Value = weight;
            Unit = unit;
        }

        public Units Unit
        {
            get { return _unit; }
            set
            {
                if ((int)value == 0)
                {
                    _unit = Units.Kg;
                }
                else
                {
                    _unit = value;
                }
            }
        }

        private bool IsInitilized
        {
            get { return Unit > 0; }
        }



        public override string ToString()
        {
            return string.Format("{0} {1}", Value, Unit);
        }

        public Weight Max(Weight value)
        {
            if (!IsInitilized)
            {
                if (!value.IsInitilized)
                {
                    throw new NotSupportedException();
                }

                return value;
            }

            var local = ToUnits(value.Unit);

            if (local > value)
            {
                return local;
            }

            return value;
        }

        public Weight ToUnits(Units units)
        {
            if (units == Unit)
            {
                return this;
            }

            switch (units)
            {
                case Units.Kg:
                    return new Weight()
                        {
                            Unit = Units.Kg,
                            Value = Value * 0.453592
                        };

                case Units.Lbs:
                    return new Weight()
                        {
                            Unit = Units.Lbs,
                            Value = Value * 2.20462
                        };

                default:
                    throw new NotSupportedException();
            }
        }

        public Weight ToKg()
        {
            return this.ToUnits(Units.Kg);
        }

        public double Kg
        {
            get
            {
                return ToKg().Value;
            }
        }

        public double Lbs
        {
            get
            {
                return ToUnits(Units.Lbs).Value;
            }
        }

        public static bool operator >(Weight x, Weight y)
        {
            return x.ToKg().Value > y.ToKg().Value;
        }

        public static bool operator >=(Weight x, Weight y)
        {
            return x.ToKg().Value >= y.ToKg().Value;
        }

        public static bool operator <(Weight x, Weight y)
        {
            return x.ToKg().Value < y.ToKg().Value;
        }

        public static bool operator <=(Weight x, Weight y)
        {
            return x.ToKg().Value <= y.ToKg().Value;
        }

        public static bool operator ==(Weight a, Weight b)
        {

            return a.ToKg().Value.Equals(b.ToKg().Value);
        }

        public static bool operator !=(Weight a, Weight b)
        {
            return !(a == b);
        }

        public static bool operator ==(Weight a, int b)
        {

            return a.ToKg().Value.Equals(b);
        }

        public static bool operator !=(Weight a, int b)
        {
            return !(a == b);
        }

        public static Weight operator *(Weight a, int multiplier)
        {
            var cal = new Weight
                {
                    Unit = a.Unit,
                    Value = a.Value * multiplier
                };
            return cal;
        }

        public static Weight operator +(Weight a, Weight b)
        {
            if (a.Unit == 0)
            {
                a.Unit = b.Unit;
            }

            if (a.Unit != b.Unit)
            {
                throw new NotSupportedException();
            }

            var cal = new Weight
            {
                Unit = a.Unit,
                Value = a.Value + b.Value
            };
            return cal;
        }

        public static Weight operator *(int multiplier, Weight a)
        {
            return a * multiplier;
        }

        public bool Equals(Weight other)
        {
            return Value == other.Value && Unit.Equals(other.Unit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Weight && Equals((Weight)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ Unit.GetHashCode();
            }
        }

        public int CompareTo(Weight other)
        {
            return this.Kg.CompareTo(other.Kg);
        }
    }