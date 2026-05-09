namespace IceCity
{
    public class DailyUsage
    {
        public DateTime Date { get; set; }
        public double HoursWorked { get; set; }
        public double HeaterValue { get; set; }

        public Dictionary<DateTime, (double WorkingHours, double Consumption)> dailyUsages = new();
         
        private double _workingHours;
        public double workingHours
        { 
          get => _workingHours;
          set
            {
                if (value >= 0)
                {
                    _workingHours = value;
                }
                else
                {
                    throw new ArgumentException("Wrong Hour Value !! (must be >= 0)");
                }
            }
        }

        public void RecordDailyUsage(DateTime date, double workingHours, double powerKW)
        {
            double consumption = powerKW * workingHours;
            dailyUsages.Add(date, (workingHours, consumption));
        }
    }
}
