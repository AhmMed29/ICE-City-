
using System.Collections.Generic;
using IceCity;

public class House
{
    public int? HouseID { get; set; }
    private Owner Owner { get; set; } = null!;
    
    public List<Heater?>? Heaters { get; set; } = null;
    public List<DailyUsage> DailyUsages { get; set; } = new();

    internal void AddHeater(Heater? heater)
    {
        Heaters ??= new List<Heater?>();
        Heaters.Add(heater);
    }
}
