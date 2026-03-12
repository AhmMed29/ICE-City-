using IceCity;

public class House
{
    public int? HouseID { get; set; }
    private List<Heater> Heaters { get; set; } = null!;
    private Owner Owner { get; set; } = null!;
}

