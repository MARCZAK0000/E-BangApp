using E_BangDomain.Entities;
using E_BangDomain.StaticData;
using System.Globalization;

namespace E_BangDomain.Repository
{
    public interface IActionRepository
    {
        Task<int> GetUserShopActionLevelAsync(string accountId, string shoId);
        Dictionary<Actions, bool> GetUserActionsAsync(int number);
        bool CanUserDoActionAsync(Dictionary<Actions, bool> actions, Actions actionsToPerform);
    }
}
