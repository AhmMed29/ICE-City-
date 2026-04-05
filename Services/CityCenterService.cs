namespace IceCity.Services
{
    public class CityCenterService
    {
        private House house { get; set; }
        public void RequestReplacement(House house, int heaterId)
        {
            Console.WriteLine($"==Heater [{heaterId}] Will be Replaced==");

            if (house.Heaters == null)
            {
                Console.WriteLine("No heaters found in this house.");
                return;
            }

            var index = house.Heaters.FindIndex(h => h.HeaterId == heaterId);
            if (index < 0)
            {
                Console.WriteLine($"Heater [{heaterId}] not found.");
                return;
            }

            house.Heaters.RemoveAt(index);
            Console.WriteLine($"Heater [{heaterId}] has been successfully removed.");
        }
    }
}
