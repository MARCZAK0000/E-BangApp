using E_BangDomain.Entities;

namespace E_BangDomain.StaticHelper
{
    public static class CalculateActions
    {
        private static void CalculateUserActions(out bool[] bools, int number, int numberOfAcitons)
        {
            
            bool[] result = new bool[numberOfAcitons];
            for (int i = 0; i < numberOfAcitons; i++)
            {
                result[numberOfAcitons - i] = (number & (1 << i)) != 0;
            }

            bools = [.. result.Reverse()];
        }

        public static Dictionary<Actions, bool> GetActionKeyValuePairs(int number, IReadOnlyList<Actions> actions)
        {
            CalculateUserActions(out var bools, number, actions.Count);
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
