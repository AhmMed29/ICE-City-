using IceCity;

public class Heater
{
    public int? HeaterId { get; set; }
    public int? houseID { get; set; }

    private House? House { get; set; }
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

    private EnumHeaterType _heaterType;
    public EnumHeaterType heaterType
    {
        get => _heaterType;
        set
        {
            if (value == EnumHeaterType.Gas || value == EnumHeaterType.Electric)
            {
                _heaterType = value;
            }
            else
            {
                throw new ArgumentException("No other Heater Types Available Only Gas Or Electric");
            }
        }
    }

    public readonly DailyUsage _dailyUsage;
    public Heater(DailyUsage dailyUsage)
    {
        _dailyUsage = dailyUsage;
    }
    
    private DateTime? _lastOpenedDate;
    public DateTime? LastOpenedDate { get => _lastOpenedDate; }

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
}
