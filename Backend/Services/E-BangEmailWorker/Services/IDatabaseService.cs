﻿using E_BangEmailWorker.Model;

namespace E_BangEmailWorker.Services
{
    public interface IDatabaseService
    {
        Task<IList<SendMailDto>> CurrentEmailsInQueueAsync(CancellationToken token);

        Task SetIsSendAsync(IList<int> ids, CancellationToken token);

        Task ClearEmailQueueAsync();
    }
}
