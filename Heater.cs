using IceCity;

public class Heater
{
    public int? HeaterId { get; set; }
    public int? houseID { get; set; }

    private House? House { get; set; }
    private List<Heater?> Heaters = new();

    private double _powerValue;
    public virtual double powerValue
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
    public virtual EnumHeaterType heaterType
    {
        get => _heaterType;
        set
        {
            if (value == EnumHeaterType.Gas || value == EnumHeaterType.Electric || value == EnumHeaterType.Solar)
            {
                _heaterType = value;
            }
            else
            {
                throw new ArgumentException("Invalid Heater Type");
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
    
    public event HeaterEventHandler OpenHeater;
    public event HeaterDurationHandler CloseHeater;
    private DateTime? _lastOpenTime;

    public void Open()
    {
        _lastOpenTime = DateTime.UtcNow;
        OpenHeater?.Invoke(this, new HeaterEventArgs
        {
            Date = _lastOpenTime.Value,
            PowerValue = powerValue,
            WorkingHours = 0
        });
    }

    public void Close()
    {
        if (_lastOpenTime.HasValue)
        {
            DateTime end = DateTime.UtcNow;
            double hours = (end - _lastOpenTime.Value).TotalHours;
            
            CloseHeater?.Invoke(this, new HeaterDurationEventArgs
            {
                StartTime = _lastOpenTime.Value,
                EndTime = end,
                HoursWorked = hours
            });
            _lastOpenTime = null;
        }
    }

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
