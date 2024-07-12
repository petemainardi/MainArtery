using System;
using System.Collections.Generic;
using System.Linq;

namespace MainArtery.Utilities
{
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /**
     *  Extension methods for the C# System.Random class.
     */
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public static class RandomExtensions
    {
        /// <summary>
        /// Get a random date between the specified dates, inclusive.
        /// </summary>
        /// <param name="random">The randomizing agent</param>
        /// <param name="start">The earliest date to consider</param>
        /// <param name="end">The latest date to consider</param>
        /// <returns>Randomly selected date between specified dates</returns>
        public static DateTime GetDateRandom(this Random random, DateTime start, DateTime end)
        {
            int days = random.Next(0, (end - start).Days + 1);  // +1 because Next() upper bound is exclusive
            return end.AddDays(-days);
        }

        /// <summary>
        /// Randomly select a number of items from the list.
        /// </summary>
        /// <typeparam name="T">The type of objects contained in the array</typeparam>
        /// <param name="random">The randomizing agent</param>
        /// <param name="items">The items from which to choose</param>
        /// <param name="numRandom">The number of items to choose</param>
        /// <returns>The randomly selected items</returns>
        public static IEnumerable<T> GetNRandom<T>(this Random random, IEnumerable<T> items, int numRandom)
        {
            List<T> selectedItems = new List<T>(numRandom);
            numRandom = Math.Min(numRandom, items.Count());

            for (int i = 0; i < items.Count() && numRandom > 0; i++)
            {
                if (random.Next(0, items.Count() - i) < numRandom)
                {
                    selectedItems.Add(items.ElementAt(i));
                    numRandom--;
                }
            }

            return selectedItems;
        }

        /// <summary>
        /// Randomly distribute a set number of items among different categories.<br/>
        /// Results are not guaranteed to be exact if the number of supplied items is less than the sum total of category allocations.
        /// </summary>
        /// <typeparam name="T">The type of object that is being distributed</typeparam>
        /// <typeparam name="TCategory">The type of category amongst which objects are being distributed</typeparam>
        /// <param name="random">The randomizing agent</param>
        /// <param name="items">The objects to be distributed</param>
        /// <param name="categoryCounts">
        /// Dictionary whose keys are the categories amongst which objects are distributed,
        /// and whose values are how many objects to distribute to that category
        /// </param>
        /// <returns>
        /// A dictionary of pairs of items and categories.
        /// </returns>
        public static Dictionary<T, TCategory> DistributeNRandom<T, TCategory>(
            this Random random, IEnumerable<T> items, Dictionary<TCategory, int> categoryCounts)
        {
            Dictionary<T, TCategory> distributedItems = new Dictionary<T, TCategory>();
            Dictionary<TCategory, int> counts = new Dictionary<TCategory, int>(categoryCounts);

            int total = counts.Values.Sum();
            IEnumerable<T> itemsAdjusted = items.Count() > total ? random.GetNRandom(items, total) : items;

            for (int i = 0; i < itemsAdjusted.Count() && counts.Count > 0; i++)
            {
                TCategory category = counts.Keys.ElementAt(random.Next(0, counts.Count));
                distributedItems.Add(itemsAdjusted.ElementAt(i), category);

                if (--counts[category] <= 0)
                {
                    counts.Remove(category);
                }
            }

            return distributedItems;
        }

        /// <summary>
        /// Randomly distribute a collection of items evenly among a collection of categories.<br/>
        /// </summary>
        /// <typeparam name="T">The type of object that is being distributed</typeparam>
        /// <typeparam name="TCategory">The type of category amongst which objects are being distributed</typeparam>
        /// <param name="random">The randomizing agent</param>
        /// <param name="items">The objects to be distributed</param>
        /// <param name="categories">Categories amongst which objects are distributed</param>
        /// <returns>Dictionary whose keys are items and whose values are the corresponding category selected for each item</returns>
        public static Dictionary<T, TCategory> DistributeEvenlyRandom<T, TCategory>(
            this Random random, IEnumerable<T> items, IEnumerable<TCategory> categories)
        {
            int quotient = items.Count() / categories.Count();
            int remainder = items.Count() % categories.Count();

            Dictionary<TCategory, int> counts = new Dictionary<TCategory, int>();
            foreach (TCategory category in categories)
                counts.Add(category, quotient);
            foreach (TCategory category in random.GetNRandom(categories, remainder))
                counts[category]++;

            return random.DistributeNRandom(items, counts);
        }

    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
}