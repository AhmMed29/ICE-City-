using IceCity;

public class House
{
    // the heater is required for each house,
    // so we use null! to indicate that it will be initialized later
    private List<Heater> Heaters { get; set; } = null!;
    private Owner Owner { get; set; } = null!;
}

