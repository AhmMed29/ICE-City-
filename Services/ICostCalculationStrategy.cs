namespace IceCity.Services
{
    public interface ICostCalculationStrategy
    {
        double CalculateCost(List<double> heaterValues, List<double> workingHours);
        double CalculateMedian(List<double> heaterValues);
        double CalculateTotalHours(List<double> workingHours);
    }
}
