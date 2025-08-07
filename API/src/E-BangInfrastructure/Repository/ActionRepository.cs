using E_BangDomain.Entities;
using E_BangDomain.Enums;
using E_BangDomain.Repository;
using E_BangDomain.StaticData;
using E_BangDomain.StaticHelper;
using E_BangInfrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace E_BangInfrastructure.Repository
{
    public class ActionRepository : IActionRepository
    {
        private readonly ProjectDbContext _projectDbContext;

        private readonly ActionStaticData _actionStaticData;

        public ActionRepository(ProjectDbContext projectDbContext, ActionStaticData actionStaticData)
        {
            _projectDbContext = projectDbContext;
            _actionStaticData = actionStaticData;
        }

        public bool HasPermission(Dictionary<Actions, bool> actions,EAction action)
        {
            Actions actionToPerform = _actionStaticData.Actions
                .FirstOrDefault(pr => pr.ActionName == Enum.GetName(action))!;
            return actions.Any(pr => pr.Key == actionToPerform && pr.Value);
        }

        public Dictionary<Actions, bool> GetUserActions(int number)
        {
            return CalculateActions.GetActionKeyValuePairs(number, _actionStaticData.Actions);
        }

        public async Task<int> GetUserShopActionLevelAsync(string accountId, string shopId, CancellationToken token)
        {
            return await _projectDbContext
                .Staff
                .Where(pr=>pr.AccountId == accountId && pr.ShopId == shopId)
                .Select(pr=>pr.ActionLevel)
                .FirstOrDefaultAsync(token);
        }

        public int SetUserShopActionLevel(Dictionary<Actions, bool> keyValuePairs)
        {
            return CalculateActions.SetActionLevel(keyValuePairs);
        }
        public List<Actions> GetActions()
        {
            return _actionStaticData.Actions.ToList();
        }
        public Dictionary<Actions, bool> CreateUserActions(bool canCreate, bool canEdit, bool canDelete)
        {
            return new()
            {
                [_actionStaticData.Actions.Where(pr => pr.ActionName.Equals("Update", StringComparison.CurrentCultureIgnoreCase)).First()] = canEdit,
                [_actionStaticData.Actions.Where(pr => pr.ActionName.Equals("Create", StringComparison.CurrentCultureIgnoreCase)).First()] = canCreate,
                [_actionStaticData.Actions.Where(pr => pr.ActionName.Equals("Delete", StringComparison.CurrentCultureIgnoreCase)).First()] = canDelete
            };
        }
    }
}
