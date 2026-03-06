using IceCity.Services;

namespace IceCity
{
    /// <summary>
    /// Prints owner and heater reports to the console.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Why constructor injection instead of <c>new Calculations()</c> inline?</b>
    /// </para>
    /// <para>
    /// The old approach was:
    /// <code>
    ///   Calculations calculations = new();   // created silently inside Report
    /// </code>
    /// That is called "tight coupling": <see cref="Report"/> hard-wires itself to one
    /// specific instance of <see cref="IceCity.Services.Calculations"/> that it creates on its own.
    /// You cannot swap it, share it, or replace it without editing the class.
    /// </para>
    /// <para>
    /// The current approach passes the dependency in from the outside (the caller):
    /// <code>
    ///   Calculations calculations = new();
    ///   Report report = new(calculations);   // same object reused / injected
    /// </code>
    /// This is called "constructor injection" (a form of Dependency Injection).
    /// Benefits:
    /// <list type="bullet">
    ///   <item>The same <c>Calculations</c> object can be shared between multiple classes.</item>
    ///   <item>In tests you can pass a fake/mock implementation instead of the real one.</item>
    ///   <item><see cref="Report"/> does not need to know <i>how</i> to build its dependency.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public class Report
    {
        private readonly Calculations _calculations;

        public Report(Calculations calculations)
        {
            _calculations = calculations;
        }

        public void PrintOwnerDetails(Owner owner)
        {
            Console.WriteLine("-----------Owner Report-------------");
            Console.WriteLine($"Owner Name: {owner.Name}");
            Console.WriteLine($"Total Working Hours For This Month: {_calculations.TotalWorkingTime()}");
            Console.WriteLine($"Average Heater Value: {_calculations.AverageHeaterValue()}");
            Console.WriteLine($"Average Cost For This Month: {_calculations.MonthlyAverageCost()}");
            Console.WriteLine("-----------------------------------------");
        }

        public void PrintHeaterDetails(Heater heater, Owner owner)
        {
            Console.WriteLine("-----------Heater Report------------");
            Console.WriteLine($"Owner Name: {owner.Name}");
            Console.WriteLine($"Heater Type: {heater.HeaterType}");
            Console.WriteLine($"Heater Power: {heater.PowerValue} KW");
            Console.WriteLine("-----------------------------------------");
        }
    }
}
