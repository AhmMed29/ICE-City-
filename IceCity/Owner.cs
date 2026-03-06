namespace IceCity
{
    public class Owner
    {
        public string Name { get; set; }

        public Owner(string name)
        {
            Name = name;
        }

        // Houses must be set before use; initialized lazily after construction
        public List<House> Houses { get; set; } = null!;
    }
}
