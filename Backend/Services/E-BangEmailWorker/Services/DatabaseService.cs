using E_BangEmailWorker.Database;
using E_BangEmailWorker.Model;
using E_BangEmailWorker.OptionsPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MimeKit;

namespace E_BangEmailWorker.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ServiceDbContext _serviceDbContext;
        private readonly EmailConnectionOptions _emailConnectionOptions;
        public DatabaseService(ServiceDbContext serviceDbContext, EmailConnectionOptions emailConnectionOptions)
        {
            _serviceDbContext = serviceDbContext;
            _emailConnectionOptions = emailConnectionOptions;
        }

        public async Task<IList<SendMailDto>> CurrentEmailsInQueueAsync(CancellationToken token)
        {
            using IDbContextTransaction transaction = await _serviceDbContext.Database.BeginTransactionAsync(token);
            try
            {
                var emails = await _serviceDbContext.Emails.ToListAsync(token);
                if (emails.Count == 0)
                {
                    return Array.Empty<SendMailDto>();
                }

                var listOfMails = emails.Select(pr =>
                    new SendMailDto(
                        pr.EmailID,
                        new MailboxAddress(nameof(_emailConnectionOptions.EmailName), _emailConnectionOptions.EmailName),
                        new MailboxAddress("client", pr.EmailAddress),
                        pr.EmailBodyJson
                    )).ToList();
                await transaction.CommitAsync(token);
                return listOfMails;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(token);
                throw;
            }
        }
        public async Task SetIsSendAsync(IList<int> ids, CancellationToken token)
        {
            using IDbContextTransaction dbContextTransaction = await _serviceDbContext.Database.BeginTransactionAsync(token);
            try
            {
                var emails = await _serviceDbContext.Emails.Where(pr=>ids.Contains(pr.EmailID)).ToListAsync(token);
                foreach (var item in emails)
                {
                    item.IsSend = true;
                }
                await dbContextTransaction.CommitAsync(token);
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(token);
                throw;  
            }
        }
        public async Task ClearEmailQueueAsync()
        {
            await _serviceDbContext.Database.ExecuteSqlRawAsync("[HANDLING].[usp_EmailRemover]");
        }
    }
}
