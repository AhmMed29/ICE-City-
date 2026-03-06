using IceCity;

// WHY CHANGED: Old properties used camelCase (heaterType, powerValue, house).
// C# convention for public properties is PascalCase. Violating this makes the
// code look inconsistent and confuses tools, auto-completion, and other developers.
public class Heater
{
    public House House { get; set; } = null!;
    public double PowerValue { get; set; }
    public HeaterType HeaterType { get; set; }
}
