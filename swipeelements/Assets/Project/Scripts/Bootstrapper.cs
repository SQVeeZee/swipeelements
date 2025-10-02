using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using UnityEngine;
using Zenject;

namespace Project
{
    public sealed class Bootstrapper : MonoBehaviour, IInitializable, IDisposable
    {
        private DiContainer _container;
        private CancellationTokenSource _cancellationTokenSource;

        private IService[] _services = Array.Empty<IService>();
        private ProjectRunner _projectRunner;

        [Inject]
        private void Construct(
            DiContainer container,
            ProjectRunner projectRunner)
        {
            _projectRunner = projectRunner;
            _container = container;
        }

        void IInitializable.Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            RunAsync(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid RunAsync(CancellationToken cancellationToken)
        {
            await InitializeServices(cancellationToken);
            _projectRunner.Run();
        }

        private async UniTask InitializeServices(CancellationToken cancellationToken)
        {
            var services = _container.ResolveAll<IService>();
            var byType = services.ToDictionary(s => s.GetType());

            var edges = new List<(Type from, Type to)>();
            foreach (var service in services)
            {
                foreach (var req in service.GetDependencies())
                {
                    var match = services.FirstOrDefault(x => req.IsAssignableFrom(x.GetType()));
                    if (match == null)
                    {
                        throw new InvalidOperationException($"{service.GetType().Name} требует {req.Name}, но сервис не зарегистрирован.");
                    }

                    edges.Add((match.GetType(), service.GetType()));
                }
            }

            _services = TopoSort(byType.Keys, edges).Select(tp => byType[tp]).ToArray();

            foreach (var s in _services)
            {
                await s.InitializeServiceAsync(cancellationToken).AttachExternalCancellation(cancellationToken);
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            for (var i = _services.Length - 1; i >= 0; i--)
            {
                try { _services[i]?.Dispose(); }
                catch (Exception e) { Debug.LogException(e); }
            }
        }

        private static List<Type> TopoSort(IEnumerable<Type> nodes, IEnumerable<(Type from, Type to)> edges)
        {
            var nodeSet = new HashSet<Type>(nodes);
            var adj = nodeSet.ToDictionary(n => n, _ => new List<Type>());
            var indeg = nodeSet.ToDictionary(n => n, _ => 0);

            foreach (var (from, to) in edges)
            {
                if (!nodeSet.Contains(from) || !nodeSet.Contains(to))
                {
                    continue;
                }
                adj[from].Add(to);
                indeg[to]++;
            }

            var q = new Queue<Type>(indeg.Where(kv => kv.Value == 0).Select(kv => kv.Key));
            var order = new List<Type>(nodeSet.Count);

            while (q.Count > 0)
            {
                var u = q.Dequeue();
                order.Add(u);
                foreach (var v in adj[u])
                {
                    indeg[v]--;
                    if (indeg[v] == 0)
                    {
                        q.Enqueue(v);
                    }
                }
            }

            if (order.Count != nodeSet.Count)
            {
                var cycle = string.Join(" -> ", indeg.Where(kv => kv.Value > 0).Select(kv => kv.Key.Name));
                throw new InvalidOperationException($"Обнаружен цикл зависимостей: {cycle}");
            }

            return order;
        }
    }
}
