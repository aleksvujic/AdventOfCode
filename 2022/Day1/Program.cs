using Common;

var elvesCalories = File.ReadAllLines(Constants.FILE_NAME)
    .ToList();

var elvesCalorieSums = new List<int>();
var currentElfCalorieSum = 0;
foreach (var calories in elvesCalories)
{
    if (string.IsNullOrEmpty(calories))
    {
        elvesCalorieSums.Add(currentElfCalorieSum);
        currentElfCalorieSum = 0;
        continue;
    }

    currentElfCalorieSum += int.Parse(calories);
}

Console.WriteLine($"The most calories that the elf is carrying: {elvesCalorieSums.Max()}");
Console.WriteLine($"Calories carried by top 3 elves: {elvesCalorieSums.OrderByDescending(x => x).Take(3).Sum()}");