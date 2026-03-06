using IceCity;
using IceCity.Services;

// WHY CHANGED: Removed "partial" — there is only one part of Program, partial is meaningless here.
// WHY CHANGED: Removed unused "using System.Runtime.InteropServices" and
//   "using System.Security.Cryptography.X509Certificates" — those namespaces have nothing
//   to do with this app and were likely copy-pasted noise. Dead imports confuse readers.
class Program
{
    // Number of days to collect data for — change once here, it applies everywhere.
    private const int DaysInMonth = 30;

    public static void Main()
    {
        // --- Owner information ---
        Console.Write("Owner Name: ");
        Owner owner = new(ReadRequiredString());

        // --- Heater information ---
        Console.Write("Heater Power (Kilowatt): ");
        double heaterPower = ReadPositiveDouble();

        HeaterType heaterType = ReadHeaterType();

        // WHY CHANGED: Property names are now PascalCase (PowerValue, HeaterType) to follow
        // C# conventions. The old names (powerValue, heaterType) were camelCase — fine for
        // local variables but wrong for public properties.
        Heater heater = new() { PowerValue = heaterPower, HeaterType = heaterType };

        Console.Clear();

        // WHY CHANGED: The old code had two large #region blocks of commented-out input loops.
        // Dead code is worse than no code — it suggests the feature "almost works" while
        // providing none of the functionality. The loops are now live, validated, and correct.
        List<double> workingHours = CollectDailyValues(
            "Working Hours",
            DaysInMonth,
            value => value >= 0 && value <= 24,
            "Enter a value between 0 and 24");

        Console.Clear();

        List<double> heaterValues = CollectDailyValues(
            "Heater Value",
            DaysInMonth,
            value => value >= 0,
            "Value must be 0 or greater");

        Console.Clear();

        // WHY CHANGED: Old code created "calculations1" but never connected it to Report.
        // Report created its own Calculations with hardcoded test data, so user input was
        // silently discarded. Now user data flows: input → Calculations → Report.
        Calculations calculations = new(workingHours, heaterValues);

        // Dependency injection: Report receives the Calculations it should use.
        Report report = new(calculations);

        report.PrintOwnerDetails(owner);
        Console.WriteLine();
        report.PrintHeaterDetails(heater, owner);
    }

    // Reads a non-empty string; keeps prompting until the user provides one.
    // WHY: Console.ReadLine() returns null (e.g., on EOF) and the old code passed that
    //   directly to the Owner constructor, causing a nullable warning and potential NullReferenceException.
    private static string ReadRequiredString()
    {
        while (true)
        {
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) return input.Trim();
            Console.Write("This field cannot be empty. Try again: ");
        }
    }

    // Reads a positive double; keeps prompting on invalid or non-positive input.
    // WHY: Convert.ToDouble() throws an exception on invalid input (e.g., letters).
    //   double.TryParse() is safe — it returns false instead of crashing the program.
    private static double ReadPositiveDouble()
    {
        while (true)
        {
            string? input = Console.ReadLine();
            if (double.TryParse(input, out double value) && value > 0)
                return value;
            Console.Write("Enter a valid positive number: ");
        }
    }

    // Reads a valid HeaterType enum value; keeps prompting on invalid input.
    // WHY: Enum.Parse() throws an exception on an unrecognised string.
    //   Enum.TryParse() is safe and ignoreCase: true avoids "gas" vs "Gas" failures.
    private static HeaterType ReadHeaterType()
    {
        Console.Write("Heater Type (Gas or Electric): ");
        while (true)
        {
            string? input = Console.ReadLine();
            if (Enum.TryParse<HeaterType>(input, ignoreCase: true, out HeaterType type))
                return type;
            Console.Write("Invalid type. Enter 'Gas' or 'Electric': ");
        }
    }

    // Collects one numeric value per day, re-prompting on invalid or out-of-range input.
    // WHY: The old loops used Convert.ToDouble() (crash on bad input) and a "break" on
    //   validation failure — which ended data collection after one bad entry instead of
    //   asking the user to correct it.
    private static List<double> CollectDailyValues(
        string label, int days, Func<double, bool> isValid, string validationHint)
    {
        List<double> data = new(days);
        Console.WriteLine($"Enter {label} for each of the {days} days:");

        for (int day = 1; day <= days; day++)
        {
            while (true)
            {
                string dayLabel = day == days ? $"[{day}/{days} — Last Day]" : $"[{day}/{days}]";
                Console.Write($"  {dayLabel} {label}: ");

                string? input = Console.ReadLine();
                if (double.TryParse(input, out double value) && isValid(value))
                {
                    data.Add(value);
                    break;
                }
                Console.WriteLine($"  Invalid — {validationHint}. Try again.");
            }
        }

        return data;
    }
}
