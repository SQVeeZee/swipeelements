using Zenject;

namespace Project.Core.Utility
{
    public static class ZenjectBinderUtility
    {
        public static void BindCanvas(this DiContainer container, ICanvasItem canvasItem, string id) =>
            container.Bind<ICanvasItem>()
                .WithId(id)
                .FromInstance(canvasItem)
                .AsSingle();

        public static void BindProfile<TSection>(this DiContainer container)
            where TSection : IProfileSection => container.BindInterfacesAndSelfTo<TSection>().AsSingle();

        public static void BindService<TService>(this DiContainer container)
            where TService : IService => container.BindInterfacesAndSelfTo<TService>().AsSingle();

        public static void BindCancellationToken<TCancellationToken>(this DiContainer container, string id)
            where TCancellationToken : class, ICancellationToken, ICancellationTokenControl =>
            container.Bind(typeof(ICancellationToken), typeof(ICancellationTokenControl))
                .WithId(id)
                .To<TCancellationToken>()
                .AsSingle();
    }
}