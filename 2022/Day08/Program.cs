using Common;

var allLines = File.ReadAllLines(Constants.FILE_NAME);
var forest = new Forest(allLines);
Console.WriteLine($"Part 1 solution: {forest.GetNumVisibleTrees()}");
Console.WriteLine($"Part 2 solution: {forest.GetMaxScenicScore()}");

class Forest
{
    public Forest(string[] input)
    {
        _trees = new Tree[input.Length][];

        for (int i = 0; i < input.Length; i++)
        {
            _trees[i] = new Tree[input[i].Length];
            for (int j = 0; j < input[i].Length; j++)
            {
                _trees[i][j] = new Tree(i, j, input[i][j] - '0');
            }
        }

        CalculateTreeVisibility();
        CalculateScenicScore();
    }

    private readonly Tree[][] _trees;

    private void CalculateTreeVisibility()
    {
        _trees.SelectMany(x => x).ToList().ForEach(x =>
        {
            x.IsVisible = IsTreeVisibleFromTheOutside(x);
        });
    }

    private bool IsTreeVisibleFromTheOutside(Tree tree)
    {
        foreach (EDirection direction in Enum.GetValues(typeof(EDirection)))
        {
            if (GetTreesInDirection(tree, direction).All(x => tree.Height > x.Height))
            {
                return true;
            }
        }

        return false;
    }

    private void CalculateScenicScore()
    {
        _trees.SelectMany(x => x).ToList().ForEach(x =>
        {
            x.ScenicScore = GetScenicScore(x);
        });
    }

    private int GetScenicScore(Tree tree)
    {
        int score = 1;
        
        foreach (EDirection direction in Enum.GetValues(typeof(EDirection)))
        {
            var treesInDirection = GetTreesInDirection(tree, direction);

            score *= IsTallestInDirection(tree, direction) ?
                treesInDirection.Count() :
                (treesInDirection.TakeWhile(x => x.Height < tree.Height).Count() + 1);
        }

        return score;
    }

    private bool IsTallestInDirection(Tree tree, EDirection direction)
    {
        return GetTreesInDirection(tree, direction).All(x => x.Height < tree.Height);
    }

    private IEnumerable<Tree> GetTreesInDirection(Tree tree, EDirection direction)
    {
        switch (direction)
        {
            case EDirection.Top:
                for (int k = tree.I - 1; k >= 0; k--)
                {
                    yield return _trees[k][tree.J];
                }
                break;
            case EDirection.Right:
                for (int k = tree.J + 1; k < _trees[tree.I].Length; k++)
                {
                    yield return _trees[tree.I][k];
                }
                break;
            case EDirection.Bottom:
                for (int k = tree.I + 1; k < _trees.Length; k++)
                {
                    yield return _trees[k][tree.J];
                }
                break;
            case EDirection.Left:
                for (int k = tree.J - 1; k >= 0; k--)
                {
                    yield return _trees[tree.I][k];
                }
                break;
            default:
                throw new Exception($"Direction {direction} is not recognized");
        }
    }

    public int GetNumVisibleTrees()
    {
        return _trees.SelectMany(x => x).Count(x => x.IsVisible);
    }

    public int GetMaxScenicScore()
    {
        return _trees.SelectMany(x => x).Max(x => x.ScenicScore);
    }
}

class Tree
{
    public Tree(int i, int j, int height)
    {
        I = i;
        J = j;
        Height = height;
    }

    public int I { get; set; }
    public int J { get; set; }
    public int Height { get; set; }
    public bool IsVisible { get; set; }
    public int ScenicScore { get; set; }

    public override string ToString()
    {
        return $"({I},{J}) Height = {Height}, Visible = {IsVisible}, ScenicScore = {ScenicScore}";
    }
}

public enum EDirection
{
    Top,
    Right,
    Bottom,
    Left
}