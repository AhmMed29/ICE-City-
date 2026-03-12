using IceCity;
using System.Diagnostics.Metrics;

public class Heater
{
    public int? houseID { get; set; }
    private House? House { get; set; } = null!;

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

    public delegate void heaterInfoDelegate(Heater heater);


}
