
using System;
using System.Collections;

public static class Utility
{
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random random = new System.Random(seed);

        for(int i = 0; i < array.Length-1; i++)
        {
            int randomIndex = random.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }

    internal static int ShuffleArray(MapGenerator.Coordinate[] coordinate)
    {
        throw new NotImplementedException();
    }
}
