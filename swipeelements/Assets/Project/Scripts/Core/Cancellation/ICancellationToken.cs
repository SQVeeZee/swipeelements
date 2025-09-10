using System.Threading;

namespace Project.Core
{
    public interface ICancellationToken
    {
        CancellationToken Token { get; }
    }
}