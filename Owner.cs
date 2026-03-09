public class Owner
{
    public string Name { get; set; }
    public Owner (string name)
    {
        Name = name;
    }

    // the house is required for each house,
    // so we use null! to indicate that it will be initialized later
    public List<House> houses { get; set; } = null!;
}
