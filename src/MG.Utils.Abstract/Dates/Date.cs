using System;

namespace MG.Utils.Abstract.Dates
{
    /// <summary>
    /// THe class represents Date within the system.
    /// </summary>
    public class Date : IEquatable<Date>
    {
        /// <summary>
        /// Gets now.
        /// </summary>
        public static Date Now => new (DateTimeOffset.Now);

        /// <summary>
        /// Gets tomorrow.
        /// </summary>
        public static Date Tomorrow => new (new DateTimeOffset(DateTime.Today.AddDays(1)));

        /// <summary>
        /// Gets today.
        /// </summary>
        public static Date Today => new (new DateTimeOffset(DateTime.Today));

        /// <summary>
        /// Gets yesterday.
        /// </summary>
        public static Date Yesterday => new (new DateTimeOffset(DateTime.Today.AddDays(-1)));

        /// <summary>
        /// Gets month as int.
        /// </summary>
        public int Month => Source.Month;

        /// <summary>
        /// Gets year as int.
        /// </summary>
        public int Year => Source.Year;

        /// <summary>
        /// Gets day as int.
        /// </summary>
        public int Day => Source.Day;

        public DateTimeOffset Source { get; }

        public Date(DateTimeOffset source)
        {
            Source = source;
        }

        public Date(DateTimeOffset? source)
            : this(source!.Value)
        {
        }

        public Date(int year, int month, int day)
            : this(new DateTimeOffset(new DateTime(year, month, day)))
        {
        }

        public Date(int year, int month, int day, int hour, int minute, int seconds)
            : this(
                new DateTimeOffset(
                    new DateTime(
                        year: year,
                        month: month,
                        day: day,
                        hour: hour,
                        minute: minute,
                        second: seconds)))
        {
        }

        public Date AddDays(int days)
        {
            return new (Source.AddDays(days));
        }

        public Date AddWeeks(int weeks) => AddDays(weeks * 7);

        public Date SubtractDays(int days)
        {
            return new (Source.AddDays(-1 * days));
        }

        public bool IsFirstDayOfMonth()
        {
            return Source.Day == 1;
        }

        public bool IsPast()
        {
            return this.EarlierOrEqual(Yesterday);
        }

        public bool IsLastDayOfMonth()
        {
            return Day == DateTime.DaysInMonth(Year, Month);
        }

        // TODO Maxim: rename to Morning
        public DateTimeOffset StartOfTheDay()
        {
            return new (
                year: Source.Year,
                month: Source.Month,
                day: Source.Day,
                hour: 0,
                minute: 0,
                second: 0,
                offset: Source.Offset);
        }

        // TODO Maxim: rename to Evening
        public DateTimeOffset EndOfTheDay()
        {
            return new (
                year: Source.Year,
                month: Source.Month,
                day: Source.Day,
                hour: 23,
                minute: 59,
                second: 59,
                offset: Source.Offset);
        }

        public bool Weekend()
        {
            return Source.DayOfWeek == DayOfWeek.Saturday || Source.DayOfWeek == DayOfWeek.Sunday;
        }

        public Date WithTimezone(int hoursOffset)
        {
            var newDate = new Date(
                new DateTimeOffset(
                    ticks: Source.Ticks,
                    offset: TimeSpan.FromHours(hoursOffset)));

            return newDate;
        }

        public Month MonthAsEnum() => (Month)Month;

        public override string ToString() => ToString("yyyy-MM-dd");

        public string ToJiraIso() => ToString("yyyy-MM-ddTHH:mm:ss.ffzzzz");

        public string ToString(string format)
        {
            format.ThrowIfNullOrEmpty(nameof(format));

            return Source.ToString(format);
        }

        public Date Clone()
        {
            return new (Source);
        }

        public bool Equals(Date other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Source.Equal(other.Source);
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!(other is Date))
            {
                return false;
            }

            return Equals((Date)other);
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode();
        }

        public Date PreviousWeekStartDate()
        {
            const int daysTillToday = 6;
            return new Date(Source.AddDays(-daysTillToday));
        }

        public Date FirstDayOfMonth()
        {
            return new (Year, Month, 1);
        }

        public Date LastDayOfMonth()
        {
            return new (Year, Month, DateTime.DaysInMonth(Year, Month));
        }
    }
}