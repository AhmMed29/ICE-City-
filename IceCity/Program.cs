using IceCity;
using IceCity.Services;

Calculations calculations = new();
Heater heater = new();
Report report = new(calculations);

Console.Write("Owner Name: ");
var ownerName = Console.ReadLine();
Owner owner = new(ownerName!);

Console.Write("Heater Power (Kilowatt): ");
heater.PowerValue = Convert.ToDouble(Console.ReadLine());

Console.Write("Heater Type (Gas OR Electric): ");
heater.HeaterType = Enum.Parse<HeaterType>(Console.ReadLine()!);

Console.Clear();

report.PrintOwnerDetails(owner);
Console.WriteLine();
report.PrintHeaterDetails(heater, owner);
