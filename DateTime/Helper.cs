 public static DateTime ApplyTimeZoneInfo(this DateTime dateTime, TimeZoneInfo timeZoneInfo)
         {
             if (dateTime.Kind == DateTimeKind.Utc)
             {
                 return TimeZoneInfo.ConvertTime(dateTime, timeZoneInfo);
             }

             return dateTime;
         }

		 public static bool IsTimeInRange(TimeRange timeRange, DateTime time)
        {
            return time.IsTimeInRange(timeRange.From, timeRange.To, timeRange.FromInclusive, timeRange.ToInclusive);
        }

        public static bool IsTimeInRange(this DateTime time, DateTime from, DateTime to, bool fromInclusive, bool toInclusive)
        {
            var inputTimeSpan = time.ToUniversalTime().TimeOfDay;
            var start = from.ToUniversalTime().TimeOfDay;
            var end = to.ToUniversalTime().TimeOfDay;

            return IsTimeBetween(inputTimeSpan, start, end, fromInclusive, toInclusive);
        }

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

        public static bool IsDateTimeBetween(DateTime input, DateTime start, DateTime end)
        {
            return input.Ticks > start.Ticks && input.Ticks < end.Ticks;
        }