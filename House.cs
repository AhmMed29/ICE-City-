using IceCity;

public class House
{
    public List<Heater>? Heaters { get; set; } = null;
    private Owner Owner { get; set; } = null!;
    
    public int? HouseID { get; set; }
    public void AddHeater(Heater heater)
    {
        Heaters ??= new List<Heater>();
        Heaters.Add(heater);
    }

    
}

