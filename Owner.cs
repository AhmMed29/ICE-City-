using IceCity.Services;
using System.Xml.Linq;

public class Owner : IPrintReports
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

    public delegate void printOwnerData(Owner owner);

    // the house is required for each house,
    // so we use null! to indicate that it will be initialized later
    private List<House> houses { get; set; } = null!;

    public void PrintMonthlyReport()
    {

        //Owner.printOwnerData printOwnerDataDelegate = (m) => Console.WriteLine("-----------Owner Report-------------");
        //printOwnerDataDelegate += printOwnerName;

        //foreach (var owner in Owners)
        //{
        //    printOwnerDataDelegate(owner);
        //}
    }
}
