
    public class TimeRange
    {
        public TimeRange()
        {
            FromInclusive = true;
            ToInclusive = true;
        }

        public TimeRange(DateTime from, DateTime to)
            : this()
        {
            From = from;
            To = to;
        }

        public TimeRange(DateTime from, DateTime to, bool fromInclusive, bool toInclusive) : this(from, to)
        {
            FromInclusive = fromInclusive;
            ToInclusive = toInclusive;
        }

        public TimeRange(TimeRange timeRange)
            : this(timeRange.From, timeRange.To, timeRange.FromInclusive, timeRange.ToInclusive)
        {
            
        }

        public TimeRange ApplyTimeZone(TimeZoneInfo timeZoneInfo)
        {
            return new TimeRange(this.From.ApplyTimeZoneInfo(timeZoneInfo), To.ApplyTimeZoneInfo(timeZoneInfo), FromInclusive, ToInclusive);
        }

        [DisplayName("From")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [DisplayName("Inclusive")]
        public bool FromInclusive { get; set; }

        [DisplayName("To")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }

        [DisplayName("Inclusive")]
        public bool ToInclusive { get; set; }

        public static bool IsTimeBetween(TimeSpan input, TimeSpan start, TimeSpan end, bool fromInclusice, bool toInclusive)
        {
            //http://stackoverflow.com/questions/592248/how-can-i-check-if-the-current-time-is-between-in-a-time-frame
            // see if start comes before end
            if (end < start)
            {
                return
                    ((toInclusive && (input <= end)) || (!toInclusive && (input < end)))
                    ||
                    ((fromInclusice && (input >= start)) || (!fromInclusice && (input > start)));
            }
            else
            {
                return
                    ((fromInclusice && (input >= start)) || (!fromInclusice && (input > start)))
                    &&
                    ((toInclusive && (input <= end)) || (!toInclusive && (input < end)));
            }


        }

        /// <summary>
        ///  current timeRange:
        ///    |(From)           |(To)
        /// cases consider intersected
        ///   case 1:
        /// |(expFrom)      |(expTo)
        ///   case 2:
        /// (expFrom)|     |(expTo)
        ///   case 3:
        ///           |(expFrom)       |(expTo)
        /// </summary>
        public IMongoQuery GetMongoQueryIntersectWith<TCollection>(
            Expression<Func<TCollection, DateTime>> fromExp, 
            Expression<Func<TCollection, DateTime>> toExp)
        {
            var rangeTo = Query.And(Query<TCollection>.GTE(toExp, To), Query<TCollection>.LTE(fromExp, To));
            var rangeFrom = Query.And(Query<TCollection>.GTE(toExp, From), Query<TCollection>.LTE(fromExp, From));

            var rangeQuery = Query.Or(rangeTo, rangeFrom, 
                Query.And(Query<TCollection>.GTE(fromExp, From),Query<TCollection>.LTE(toExp, To)));
            return rangeQuery;
        }

        public IMongoQuery GetMongoQueryIntersectWith(string fromExp, string toExp)
        {
            var rangeTo = Query.And(Query.GTE(toExp, To), Query.LTE(fromExp, To));
            var rangeFrom = Query.And(Query.GTE(toExp, From), Query.LTE(fromExp, From));

            var rangeQuery = Query.Or(rangeTo, rangeFrom,
                Query.And(Query.GTE(fromExp, From), Query.LTE(toExp, To)));
            return rangeQuery;
        }

        public virtual bool IsTimeInRange(DateTime time)
        {
            var inputTimeSpan = time.ToUniversalTime().TimeOfDay;
            var start = From.ToUniversalTime().TimeOfDay;
            var end = To.ToUniversalTime().TimeOfDay;

            return IsTimeBetween(inputTimeSpan, start, end, FromInclusive, ToInclusive);
        }

        public virtual bool IsDateTimeInRange(DateTime dateTime)
        {
            return From <= dateTime && dateTime <= To;
        }

        [BsonIgnore]
        public bool IsInitilized
        {
            get { return From != default(DateTime) && To != default(DateTime); }
        }

        public void ExtendBoundaries(DateTime from, DateTime to)
        {
            if (!IsInitilized)
            {
                From = from;
                To = to;
            }
            else
            {
                if (from.ToUniversalTime() < From.ToUniversalTime())
                {
                    From = from;
                }

                if (to.ToUniversalTime() > To.ToUniversalTime())
                {
                    To = to;
                }
            }
        }

        public void ExtendBoundaries(TimeRange timeRange)
        {
            ExtendBoundaries(timeRange.From, timeRange.To);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", From.ToUniversalTime().TimeOfDay, To.ToUniversalTime().TimeOfDay);
        }

        public string ToString(TimeZoneInfo timeZoneInfo)
        {
            return string.Format("{0} - {1}", From.ApplyTimeZoneInfo(timeZoneInfo).TimeOfDay, To.ApplyTimeZoneInfo(timeZoneInfo).TimeOfDay);
        }

        public string ToShortDate(TimeZoneInfo timeZoneInfo)
        {
            return string.Format("{0} - {1}", From.ApplyTimeZoneInfo(timeZoneInfo).ToShortTimeString(), To.ApplyTimeZoneInfo(timeZoneInfo).ToShortTimeString());
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var timeRange = (TimeRange)obj;

            return From.Equals(timeRange.From) && To.Equals(timeRange.To) &&
                   FromInclusive.Equals(timeRange.FromInclusive) && ToInclusive.Equals(timeRange.ToInclusive);


        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return string.Format("{0}_{1}", From, To).GetHashCode();
        }
    }