public class Owner
{
    private string _Name;
    public string Name
    {
        get => _Name; 
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Name Cannot Be Empty !");
            }
            _Name = value;
        }
    }

    public Owner (string name)
    {
        Name = name;
    }
    private List<House> houses { get; set; } = null!;
}
