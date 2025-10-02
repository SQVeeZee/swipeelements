using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core
{
    public interface IService
    {
        UniTask InitializeServiceAsync(CancellationToken cancellationToken);
        IEnumerable<Type> GetDependencies();
        void Dispose();
    }
}