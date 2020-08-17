using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace MPLib.Random
{
    public class Shuffling
    {
        /// <summary>
        ///     Simple Method to shuffle list.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> Shuffle<T>(IEnumerable<T> source)
        {
            var list = new List<T>();
            list.AddRange(source);

            for (var i = list.Count - 1; i > 0; i--)
            {
                var rand = SafeRandomGenerator.GetInt(0, i);

                var temp = list[i];
                list[i] = list[rand];
                list[rand] = temp;
            }

            return list;
        }
    }
}