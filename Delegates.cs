using System;

namespace IceCity
{
    public delegate void HeaterEventHandler(object sender, HeaterEventArgs e);
    public delegate void HeaterDurationHandler(object sender, HeaterDurationEventArgs e);
    public delegate void SaveDailyUsageDelegate(DailyUsage usage);
}