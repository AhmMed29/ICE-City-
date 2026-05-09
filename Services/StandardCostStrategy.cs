namespace IceCity.Services
{
    public class StandardCostStrategy : ICostCalculationStrategy
    {
        public double CalculateTotalHours(List<double> workingHours)
        {
            return workingHours.Sum();
        }

        public double CalculateMedian(List<double> heaterValues)
        {
            if (heaterValues == null || heaterValues.Count == 0) return 0;

            var sortedValues = heaterValues.OrderBy(x => x).ToList();
            int count = sortedValues.Count;
            int mid = count / 2;

            return count.IsOdd() ? sortedValues[mid] : (sortedValues[mid - 1] + sortedValues[mid]) / 2.0;
        }

        public double CalculateCost(List<double> heaterValues, List<double> workingHours)
        {
            double medianHeaterValue = CalculateMedian(heaterValues);
            double totalWorkingHours = CalculateTotalHours(workingHours);

            return medianHeaterValue * (totalWorkingHours / (24.0 * 30.0));
        }
    }
}