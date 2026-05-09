namespace IceCity.Services
{
    public interface ICostStrategyFactory
    {
        ICostCalculationStrategy GetStrategy(string type);
    }
}