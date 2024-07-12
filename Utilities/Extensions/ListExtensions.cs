using System;
using System.Collections.Generic;

namespace MainArtery.Utilities
{
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /**
     *  Extension methods for the C# List class using the System.Random class.
     */
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public static class ListExtenstions
    {
        /// <summary>
        /// Remove the specified element from the list and return it.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the list</typeparam>
        /// <param name="list">The list being acted upon</param>
        /// <param name="element">The element to remove</param>
        /// <returns>The removed element</returns>
        public static T RemoveGrab<T>(this List<T> list, T element)
        {
            if (list.Remove(element))
                return element;

            return default;
        }

        /// <summary>
        /// Remove the element at the given index and return it.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the list</typeparam>
        /// <param name="list">The list being acted upon</param>
        /// <param name="index">The index of the element to remove</param>
        /// <returns>The removed element</returns>
        public static T RemoveGrabAt<T>(this List<T> list, int index)
        {
            if (index < 0 || index >= list.Count)
                return default;

            T t = list[index];
            list.RemoveAt(index);
            return t;
        }

        /// <summary>
        /// Remove a random element from the list and return it.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the list</typeparam>
        /// <param name="list">The list being acted upon</param>
        /// <param name="rand">Randomizing agent</param>
        /// <returns>The randomly selected item that was removed</returns>
        public static T RandomRemove<T>(this List<T> list, Random rand)
        {
            return list.RemoveGrabAt(rand.Next(0, list.Count));
        }

        /// <summary>
        /// Reorder elements by the Fisher-Yates Shuffle.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the list</typeparam>
        /// <param name="list">The list being acted upon</param>
        /// <param name="rand">Randomizing agent</param>
        // Based on code from https://bost.ocks.org/mike/shuffle/
        public static void Shuffle<T>(this List<T> list, Random rand)
        {
            int idx, count = list.Count;
            T temp;

            while (count > 0)
            {
                idx = (int)Math.Floor(rand.NextDouble() * count--);
                temp = list[count];
                list[count] = list[idx];
                list[idx] = temp;
            }
        }
    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
}