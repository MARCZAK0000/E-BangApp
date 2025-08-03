using E_BangDomain.Entities;

namespace E_BangDomain.StaticHelper
{
    public static class CalculateActions
    {
        private static void CalculateUserActions(out bool[] bools, int number, int numberOfActions)
        {
            
            bool[] result = new bool[numberOfActions];
            for (int i = 0; i < numberOfActions; i++)
            {
                result[numberOfActions - i] = (number & (1 << i)) != 0;
            }

            bools = [.. result.Reverse()];
        }

        public static Dictionary<Actions, bool> GetActionKeyValuePairs(int number, IReadOnlyList<Actions> actions)
        {
            CalculateUserActions(out var bools, number, actions.Count - 1);
            var keyVaulePairs = new Dictionary<Actions, bool>();
            for (int i = 0; i < bools.Length; i++)
            {
                keyVaulePairs.Add(actions[i], bools[i]);
            }
            return keyVaulePairs;
        }

        public static int SetActionLevel(Dictionary<Actions, bool> keyValuePairs)
        {
           return keyValuePairs
                .Where(pr=>pr.Value==true)
                .Sum(pr=>pr.Key.ActionLevel);
        }
    }
}
