namespace IceCity
{
    // WHY CHANGED: "internal" → "public" for consistency with other model classes.
    // WHY CHANGED: Old property was "TimeOnly Day" — a "day of the month" is a calendar date,
    //   not a time-of-day. TimeOnly stores hours/minutes/seconds; DateOnly stores year/month/day.
    //   Using the wrong type makes comparisons and sorting incorrect or confusing.
    //   Renamed to "Date" to match the DateOnly type (Date = calendar day, not a clock time).
    // WHY CHANGED: "heaterValue" → "HeaterValue" — PascalCase for public properties.
    // SUGGESTED FEATURE: Add a Notes or Tags property for recording unusual usage events.
    public class DailyUsage
    {
        public DateOnly Date { get; set; }
        public double HeaterValue { get; set; }
    }
}
