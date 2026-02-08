class Program
{
    public static void Main()
    {
        List<double> HeaterValues = new();
        List<double> WorkingHoursPerDay = new();

        Console.WriteLine("Enter Owner Name:");
        var ownerName = Console.ReadLine();
        Console.Clear();

        Console.WriteLine("Enter Hours Per Day During The Month:");
        #region Enter Working Hours During The Month
        var dayCounter = 1;
        for (int i = 1; i < 31; i++)
        {
            Console.WriteLine("[ " + dayCounter + " | 30 ] " + "Enter Working Hours:");
            double input = Convert.ToDouble(Console.ReadLine());
            if (input < 0 || input > 24)
            {
                Console.WriteLine("Wrong Hour Input (0 < N < 24) hrs");
                break;
            }
            WorkingHoursPerDay.Add(input);
            dayCounter++;
            Console.Clear();
            if (dayCounter == 30) Console.WriteLine("==Last Day in Month=="); ;
            if (dayCounter > 30) break;
        }
        Console.Clear();
        #endregion

        Console.WriteLine("Enter Heater Values During The Month:");
        #region Enter Heater Values During The Month
        var heaterCounter = 1;
        for (int i = 1; i < 31; i++)
        {
            Console.WriteLine("[ " + heaterCounter + " | 30 ] " + "Enter Heater Values:");
            double input = Convert.ToDouble(Console.ReadLine());
            if (input < 0)
            {
                Console.WriteLine("Wrong heater Value ... positive only");
                break;
            }
            HeaterValues.Add(input);
            heaterCounter++;
            Console.Clear();
            if (heaterCounter == 30) Console.WriteLine("==Last Day in Month=="); ;
            if (heaterCounter > 30) break;
        }
        Console.Clear();
        #endregion

        #region Algorithm To Sort Heater Values
        // new List To Store Sorted Heater Values Using Bubble Sort Algorithm
        double temp = 0;
        for (int i = 0; i < HeaterValues.Count - 1; i++)
        {
            for (int j = 0; j < HeaterValues.Count - 1; j++)
            {
                if (HeaterValues[j] > HeaterValues[j + 1])
                {
                    temp = HeaterValues[j];
                    HeaterValues[j] = HeaterValues[j + 1];
                    HeaterValues[j + 1] = temp;
                }
            }
        }

        Console.WriteLine("**************************");
        Console.WriteLine("The Numbers Bubble Sorted:");
        foreach (var num in HeaterValues)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
        Console.WriteLine("The middle element: " + HeaterValues[HeaterValues.Count / 2]);
        Console.WriteLine("**************************");
        Console.WriteLine();
        #endregion

        #region Total Cost Calculation

        // Total Cost Calculation over the Month
        double medianValue = HeaterValues.Sum() / HeaterValues.Count;
        var workingHours = WorkingHours(WorkingHoursPerDay);
        double totalCostResult = totalCost(medianValue, workingHours);

        Console.WriteLine("========================");
        Console.WriteLine("The bill of " + ownerName);
        Console.WriteLine("The Average Cost: " + Math.Round(totalCostResult, 2));
        Console.WriteLine("========================");

        #endregion

    }
    public static double WorkingHours(List<double> workingHours)
    {
        return workingHours.Sum();
    }
    public static double totalCost(double medianValue, double totalWorkingHours)
    {
        return medianValue * (totalWorkingHours / (24 * 30));
    }
}
