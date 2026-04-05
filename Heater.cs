using IceCity;
using IceCity.Services;
using System.Diagnostics.Metrics;

public class Heater : IPrintReports
{
    public int? HeaterId { get; set; }
    public int? houseID { get; set; }

    private House? House { get; set; } = null!;
    private List<Heater?> Heaters = new();

    private double _powerValue;
    public double powerValue
    {
        get => _powerValue;
        set
        {
            if (value > 0)
            {
                _powerValue = value;
            }
            else
            {
                throw new ArgumentException("Wrong Power Value");
            }
        }
    }

    private HeaterType _heaterType;
    public HeaterType heaterType
    {
        get => _heaterType;
        set
        {
            if (value == HeaterType.Gas || value == HeaterType.Electric)
            {
                _heaterType = value;
            }
            else
            {
                throw new ArgumentException("No other Heater Types Available Only Gas Or Electric");
            }
        }
    }

    private readonly DailyUsage _dailyUsage;
    public Heater(DailyUsage dailyUsage)
    {
        _dailyUsage = dailyUsage;
    }


    // This Should be in Report

    //public void getHeaterType(Heater heater)
    //{
    //    if (_heaterType == HeaterType.Gas)
    //    {
    //        Console.WriteLine("The Heater Type Is: Gas"); // Not in this place
    //    }
    //    else if (_heaterType == HeaterType.Electric)
    //    {
    //        Console.WriteLine("The Heater Type Is : Electric"); // Not in this place
    //    }
    //}

    //public void getHeaterPower(Heater heater)
    //{
    //    Console.WriteLine($"Heater Power Is: {heater.powerValue} KW");
    //}

    private DateTime? _lastOpenedDate;
    public DateTime? LastOpenedDate { get => _lastOpenedDate; }

    // This method for delegate
    public static void getLastOpenedDay(Heater heater)
    {
        if (heater._lastOpenedDate.HasValue)
            Console.WriteLine($"Last Opened Date : {heater._lastOpenedDate.Value:dd/MM/yyyy}");
        else
            Console.WriteLine("Last Opened Date : Not opened yet");
    }


    // Report Logic 

    //public delegate void heaterInfoDelegate(Heater heater);


    // old custom delegate

    //public delegate void HeaterOpenedDelegate(Heater heater);
    //public event HeaterOpenedDelegate? OnHeaterOpen;

    public event EventHandler<HeaterEventArgs> OnHeaterOpen;
    public void Open(DateTime date)
    {
        _lastOpenedDate = date;

        OnHeaterOpen?.Invoke(this, new HeaterEventArgs
        {
            Date = date,
            PowerValue = powerValue,
            WorkingHours = _dailyUsage.workingHours
        });
    }

    // Not Used

    //public void SubscribeToHeaterEvents(Heater heater)
    //{
    //    Heaters.Add(heater);
    //}

    public void PrintMonthlyReport() => _dailyUsage.PrintMonthlyReport();
}
