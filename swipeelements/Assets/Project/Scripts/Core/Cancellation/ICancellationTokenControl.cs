using System.Threading;

namespace Project.Core
{
    public interface ICancellationTokenControl
    {
        CancellationToken CreateToken();
        void Cancel();
    }
}