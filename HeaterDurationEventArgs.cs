using System;

namespace IceCity
{
    public class HeaterDurationEventArgs : EventArgs
    {
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public double HoursWorked { get; init; }
    }
}