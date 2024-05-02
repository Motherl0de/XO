using System.Collections.Generic;

internal static class GamePatterns
{
    public static int GetPattern<TCheck, TCell>(TCell[,] field) where TCheck : TCell
    {
        var pattern = 0;
        var i = 0;
        foreach (var cell in field)
        {
            if (cell is TCheck)
            {
                pattern |= 1 << i;
            }

            i++;
        }

        return pattern;
    }

    private static int GetPattern<TCell>(TCell[,] field, TCell filled) where TCell : struct
    {
        var pattern = 0;
        var i = 0;
        foreach (var cell in field)
        {
            if (cell.Equals(filled))
            {
                pattern |= 1 << i;
            }

            i++;
        }

        return pattern;
    }

    public static IEnumerable<int> WinPatterns
    {
        get
        {
            yield return GetPattern(new char[3, 3]
            {
                { ' ', 'x', ' ' },
                { ' ', 'x', ' ' },
                { ' ', 'x', ' ' }
            }, 'x');
            yield return GetPattern(new char[3, 3]
            {
                { 'x', ' ', ' ' },
                { ' ', 'x', ' ' },
                { ' ', ' ', 'x' }
            }, 'x');
            yield return GetPattern(new char[3, 3]
            {
                { 'x', ' ', ' ' },
                { 'x', ' ', ' ' },
                { 'x', ' ', ' ' }
            }, 'x');
            yield return GetPattern(new char[3, 3]
            {
                { ' ', ' ', 'x' },
                { ' ', 'x', ' ' },
                { 'x', ' ', ' ' }
            }, 'x');
            yield return GetPattern(new char[3, 3]
            {
                { ' ', ' ', 'x' },
                { ' ', ' ', 'x' },
                { ' ', ' ', 'x' }
            }, 'x');
            yield return GetPattern(new char[3, 3]
            {
                { 'x', 'x', 'x' },
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' }
            }, 'x');
            yield return GetPattern(new char[3, 3]
            {
                { ' ', ' ', ' ' },
                { 'x', 'x', 'x' },
                { ' ', ' ', ' ' }
            }, 'x');
            yield return GetPattern(new char[3, 3]
            {
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' },
                { 'x', 'x', 'x' }
            }, 'x');
        }
    }
}
