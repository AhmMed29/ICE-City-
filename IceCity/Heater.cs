namespace IceCity
{
    public class Heater
    {
        public House House { get; set; } = null!;
        public double PowerValue { get; set; }
        public HeaterType HeaterType { get; set; }
    }
}
