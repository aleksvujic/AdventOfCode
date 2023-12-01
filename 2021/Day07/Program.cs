using Common;

namespace Day07
{
    internal class Program
    {
        static void Main()
        {
            int[] positions = File.ReadAllLines(Constants.FILE_NAME)
                .FirstOrDefault()?
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            Console.WriteLine($"Part 1 result: {GetCheapestFuelConsumption(positions, x => x)}");
            Console.WriteLine($"Part 2 result: {GetCheapestFuelConsumption(positions, x => Enumerable.Range(1, x).Sum())}");
        }

        static int GetCheapestFuelConsumption(int[] positions, Func<int, int> funcFuelConsumptionIncrease)
        {
            int max = positions.Max();
            int[] fuelConsumptions = new int[max];

            // try all starting positions
            for (int currentPos = 0; currentPos < fuelConsumptions.Length; currentPos++)
            {
                // calculate fuel consumption if all crabs gathered at 'currentPos'
                int fuelConsumption = 0;
                for (int j = 0; j < positions.Length; j++)
                {
                    int temp = Math.Abs(positions[j] - currentPos);

                    // increase fuel consumption by function passed as a parameter
                    fuelConsumption += funcFuelConsumptionIncrease(temp);
                }
                fuelConsumptions[currentPos] = fuelConsumption;
            }

            // return best starting position
            return fuelConsumptions.Min();
        }
    }
}
