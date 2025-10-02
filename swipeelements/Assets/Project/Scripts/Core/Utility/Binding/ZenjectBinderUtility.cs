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

        public static void BindSelfRunCancellationToken<TCancellationToken>(this DiContainer container, string id)
            where TCancellationToken : class, ICancellationToken, ICancellationTokenControl =>
            container.Bind(typeof(ICancellationToken), typeof(ICancellationTokenControl))
                .WithId(id)
                .To<TCancellationToken>()
                .AsSingle();

        public static void BindCancellationToken<TCancellationToken>(this DiContainer container, TCancellationToken cancellationToken, string id)
            where TCancellationToken : class, ICancellationToken =>
            container.Bind(typeof(ICancellationToken))
                .WithId(id)
                .FromInstance(cancellationToken)
                .AsSingle();

        public static bool BindPanel<TScreen>(this DiContainer container, TScreen panel)
            where TScreen : BasePanelScreen
        {
            container.BindPanel(new ScreenLoader<TScreen>(panel));
            return true;
        }

        private static void BindPanel<TScreen>(this DiContainer container, IScreenLoader<TScreen> loader, string id)
            where TScreen : IScreen
        {
            container.BindPanel(loader);
            foreach (var type in loader.GetType().GetInterfaces())
            {
                container.Bind(type).WithId(id).FromInstance(loader);
            }
        }

        private static IScreenLoader<TScreen> BindPanel<TScreen>(this DiContainer container, IScreenLoader<TScreen> loader)
            where TScreen : IScreen
        {
            container.QueueForInject(loader);
            container.BindInterfacesTo(loader.GetType()).FromInstance(loader);
            return loader;
        }
    }
}