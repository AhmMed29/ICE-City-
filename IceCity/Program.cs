using IceCity;
using IceCity.Services;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

partial class Program
{
    public static void Main()
    {
        Calculations calculations1 = new();
        Heater heater = new();
        Report report = new();

        Console.Write("Owner Name : ");
        var ownerName = Console.ReadLine();
        Owner owner = new(ownerName);

        Console.Write("Heater Power (Kilowatt) : ");
        heater.powerValue = Convert.ToDouble(Console.ReadLine());

        Console.Write("Heater Type (Gas OR Electric) : ");
        heater.heaterType = Enum.Parse<HeaterType>(Console.ReadLine());

        // To use this data comment the loops below (:
        #region Data For Testing
        // Test data is in class Calculations !
        #endregion

        #region UserInput Hours Per Day
        //Console.WriteLine("Enter Hours Per Day During The Month:");
        //var dayCounter = 1;
        //for (int i = 1; i < 31; i++)
        //{
        //    Console.WriteLine("[ " + dayCounter + " | 30 ] " + "Enter Working Hours:");
        //    double input = Convert.ToDouble(Console.ReadLine());
        //    if (input < 0 || input > 24)
        //    {
        //        Console.WriteLine("Wrong Hour Input (0 < N < 24) hrs");
        //        break;
        //    }
        //    calculations1.workingHours.Add(input);
        //    dayCounter++;
        //    Console.Clear();
        //    if (dayCounter == 30) Console.WriteLine("==Last Day in Month=="); ;
        //    if (dayCounter > 30) break;
        //}
        //Console.Clear();
        #endregion

        #region UserInput HeaterValues
        //Console.WriteLine("Enter Heater Values During The Month:");
        //var heaterCounter = 1;
        //for (int i = 1; i < 31; i++)
        //{
        //    Console.WriteLine("[ " + heaterCounter + " | 30 ] " + "Enter Heater Values:");
        //    double input = Convert.ToDouble(Console.ReadLine());
        //    if (input < 0)
        //    {
        //        Console.WriteLine("Wrong heater Value ... positive only");
        //        break;
        //    }
        //    calculations1.heaterValues.Add(input);
        //    heaterCounter++;
        //    Console.Clear();
        //    if (heaterCounter == 30) Console.WriteLine("==Last Day in Month=="); ;
        //    if (heaterCounter > 30) break;
        //}
        //Console.Clear();
        #endregion
        Console.Clear();

        report.PrintOwnerDetails(owner);
        Console.WriteLine();
        Console.WriteLine("I Know The Task Is not Completed But i will " +
                          "Add Other Features as the clock now is 12:32 AM");
        Console.WriteLine();
        report.PrintHeaterDetails(heater, owner);
    }
}
