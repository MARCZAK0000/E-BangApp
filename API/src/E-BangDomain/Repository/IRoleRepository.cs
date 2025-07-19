using E_BangDomain.Entities;

namespace E_BangDomain.Repository
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of roles associated with the specified account ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch roles for the given account ID.
        /// Ensure that the <paramref name="token"/> is properly managed to handle cancellation scenarios.</remarks>
        /// <param name="accountId">The unique identifier of the account for which roles are being retrieved. Must not be <see langword="null"/>
        /// or empty.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of role names associated
        /// with the specified account ID. If no roles are found, the list will be empty.</returns>
        Task<List<string>> GetRoleByAccountIdAsync(string accountId, CancellationToken token);

        /// <summary>
        /// Asynchronously retrieves a list of roles.
        /// </summary>
        /// <remarks>The returned list may be empty if no roles are available. Ensure the cancellation
        /// token is properly handled to avoid unnecessary resource usage.</remarks>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of role names.</returns>
        Task<List<Roles>> GetRolesAsync(CancellationToken token);

        /// <summary>
        /// Assigns the specified role to the account identified by the given account ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to assign a role to an account. Ensure
        /// that the account ID and role name are valid and that the cancellation token is properly managed to avoid
        /// unintended cancellations.</remarks>
        /// <param name="accountId">The unique identifier of the account to which the role will be assigned. Cannot be null or empty.</param>
        /// <param name="roleName">The name of the role to assign to the account. Cannot be null or empty.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns><see langword="true"/> if the role was successfully assigned to the account; otherwise, <see
        /// langword="false"/>.</returns>
        Task<bool> AddToRoleAsync(string accountId, string roleName, CancellationToken token);


    }
}
