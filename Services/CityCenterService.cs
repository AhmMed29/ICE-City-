namespace IceCity.Services
{
    public class CityCenterService
    {
        House house { get; set; }
        List<Heater> Heaters { get; set; } = new();
        public void RequestReplacement(int HouseId, int HeaterId)
        {

        }
    }
}
