using System;

namespace MainArtery.Utilities
{
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /**
     *  Extension methods for the C# Array class using the System.Random class.
     */
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public static class ArrayExtensions
    {
        /// <summary>
        /// Reorder elements by the Fisher-Yates Shuffle.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the array</typeparam>
        /// <param name="array">The array being acted upon</param>
        /// <param name="rand">Randomizing agent</param>
        // Based on code from https://bost.ocks.org/mike/shuffle/
        public static void Shuffle<T>(this T[] array, Random rand)
        {
            int idx, count = array.Length;
            T temp;

            while (count > 0)
            {
                idx = rand.Next(0, count--);
                temp = array[count];
                array[count] = array[idx];
                array[idx] = temp;
            }
        }

    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
}