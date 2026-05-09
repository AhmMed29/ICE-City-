namespace IceCity.Services
{
    public class CostStrategyFactory : ICostStrategyFactory
    {
        public ICostCalculationStrategy GetStrategy(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return new StandardCostStrategy();
            }

            return type.ToLower() switch
            {
                "eco" => new EcoCostStrategy(),
                "standard" => new StandardCostStrategy(),
                _ => new StandardCostStrategy(),
            };
        }
    }
}