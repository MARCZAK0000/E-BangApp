using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.Cache
{
    public interface ICacheStrategy
    {
        Task<TResponse> Handle<TRequest, TResponse> (TRequest request, Func<Task<TResponse>> nextAction, CancellationToken token);
    }
}
                                               