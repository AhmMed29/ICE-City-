
public class House
{
    public int? HouseID { get; set; }
    private Owner Owner { get; set; } = null!;
    
    public List<Heater>? Heaters { get; set; } = null;

    internal void AddHeater(Heater heater)
    {
        Heaters ??= [];
        Heaters.Add(heater);
    }
}
