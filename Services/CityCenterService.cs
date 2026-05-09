namespace IceCity.Services
{
    public class CityCenterService
    {
        public bool RequestReplacement(House house, int heaterId)
        {
            if (house.Heaters == null) return false;

            var index = house.Heaters.FindIndex(h => h.HeaterId == heaterId);
            if (index < 0) return false;

            house.Heaters.RemoveAt(index);
            return true;
        }
    }
}
