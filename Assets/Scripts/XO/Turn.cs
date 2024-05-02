namespace XO
{
    internal abstract class Turn
    {
        internal sealed class Available : Turn
        {
            internal override string Name => string.Empty;
        }

        internal sealed class Cross : Turn
        {
            internal override string Name => "X";
        }

        internal sealed class Circle : Turn
        {
            internal override string Name => "O";
        }

        internal abstract string Name { get; }
    }
}
