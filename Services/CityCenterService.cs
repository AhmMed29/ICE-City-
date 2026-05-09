namespace IceCity.Services
{
    public class CityCenterService
    {
        public async Task<Heater?> RequestReplacementAsync(House house, int? heaterId)
        {
            await Task.Delay(500); // Simulate network delay
            
            if (house.Heaters == null || !heaterId.HasValue) return null;

            var index = house.Heaters.FindIndex(h => h != null && h.HeaterId == heaterId.Value);
            if (index < 0) return null;

            // Simulate replacing with a new heater
            var replacement = new Heater(new DailyUsage())
            {
                HeaterId = heaterId.Value + 100, // Just a simulated new ID
                houseID = house.HouseID,
                powerValue = house.Heaters[index]?.powerValue ?? 1500,
                heaterType = house.Heaters[index]?.heaterType ?? EnumHeaterType.Electric
            };

            house.Heaters[index] = replacement;
            return replacement;
        }

        public bool RequestReplacement(House house, int heaterId)
        {
            if (house.Heaters == null) return false;

            var index = house.Heaters.FindIndex(h => h != null && h.HeaterId == heaterId);
            if (index < 0) return false;

            house.Heaters.RemoveAt(index);
            return true;
        }
    }
}
