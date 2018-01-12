 public class TimeSpanRange
    {
        [DisplayName("From")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan FromTime { get; set; }

        [DisplayName("To")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan ToTime { get; set; }

        public TimeSpanRange(TimeSpan fromTime, TimeSpan toTime)
            : this(fromTime.Hours, fromTime.Minutes, toTime.Hours, toTime.Minutes)
        { }

        public TimeSpanRange(int fromHours, int fromMinutes, int toHours, int toMinutes)
        {
            if (fromHours < 0 || fromHours > 23)
            {
                throw new ArgumentOutOfRangeException("fromHours", "fromHours must be in the range 0 to 23 inclusive");
            }

            if (toHours < 0 || toHours > 23)
            {
                throw new ArgumentOutOfRangeException("toHours", "toHours must be in the range 0 to 23 inclusive");
            }

            if (fromMinutes < 0 || fromMinutes > 59)
            {
                throw new ArgumentOutOfRangeException("fromMinutes", "fromMinutes must be in the range 0 to 59 inclusive");
            }

            if (toMinutes < 0 || toMinutes > 59)
            {
                throw new ArgumentOutOfRangeException("toMinutes", "toMinutes must be in the range 0 to 59 inclusive");
            }

            this.FromTime = new TimeSpan(fromHours, fromMinutes, 0);
            this.ToTime = new TimeSpan(toHours, toMinutes, 0);
        }

        public bool IsWithinTimeSpanRange(TimeSpan timeOfDay)
        {
            if (this.ToTime >= timeOfDay && this.FromTime <= timeOfDay)
            {
                return true;
            }
            return false;
        }
    }