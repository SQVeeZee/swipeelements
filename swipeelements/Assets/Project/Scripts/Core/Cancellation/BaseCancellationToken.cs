using System.Threading;

namespace Project.Core
{
    public abstract class BaseCancellationToken : ICancellationToken
    {
        protected CancellationTokenSource Current { get; set; }

        public CancellationToken Token => Current.Token;
    }
}