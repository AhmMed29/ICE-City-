namespace IceCity
{
    public class SolarHeater : Heater
    {
        public SolarHeater(DailyUsage dailyUsage) : base(dailyUsage)
        {
            this.heaterType = EnumHeaterType.Solar;
        }

        // Applying the 0.7 cost multiplier natively via the power value
        // so no existing calculation logic needs to change.
        public override double powerValue
        {
            get => base.powerValue * 0.7;
            set => base.powerValue = value;
        }
    }
}