namespace IceCity
{
    public class House
    {
        // Heaters may be initialized later after construction
        public List<Heater> Heaters { get; set; } = null!;
        public Owner Owner { get; set; } = null!;
    }
}
