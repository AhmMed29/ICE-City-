namespace IceCity
{
    public class HeaterEventArgs : EventArgs
    {
        public DateTime Date { get; init; }
        public double PowerValue { get; init; }
        public double WorkingHours { get; init; }
    }
}
