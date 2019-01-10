namespace CommandStack.Utilities
{
    internal interface IRandomizer
    {
        int Generate(int max);
        int Generate(int min, int max);
    }
}
