using E_BangDomain.Entities;
using E_BangDomain.Enums;
using E_BangDomain.StaticData;
using System.Globalization;

namespace E_BangDomain.Repository
{
    public interface IActionRepository
    {
        Task<int> GetUserShopActionLevelAsync(string accountId, string shoId, CancellationToken token);
        Dictionary<Actions, bool> GetUserActions(int number);
        bool HasPermission (Dictionary<Actions, bool> actions, EAction action);
        int SetUserShopActionLevel(Dictionary<Actions, bool> keyValuePairs);
        List<Actions> GetActions();
        Dictionary<Actions, bool> CreateUserActions(bool canCreate, bool canEdit, bool canDelete);
    }
}
