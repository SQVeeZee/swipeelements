using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Project.Core.Utility
{
    public static class SceneExtensions
    {
        public static T FindFirstComponentOfType<T>(this Scene scene, bool includeInactive = true)
        {
            return scene.FindComponentsOfType<T>(includeInactive).FirstOrDefault();
        }

        public static T FindFirstComponentOfType<T>(this Scene scene, Func<T, bool> predicate, bool includeInactive = true)
        {
            return scene.FindComponentsOfType<T>(includeInactive).FirstOrDefault(predicate);
        }

        public static IEnumerable<T> FindComponentsOfType<T>(this Scene scene, bool includeInactive = true)
        {
            if (!scene.IsValid())
            {
                yield break;
            }

            var rootGameObjects = scene.GetRootGameObjects();
            if (rootGameObjects == null)
            {
                yield break;
            }

            var comparer = EqualityComparer<T>.Default;
            foreach (var gameObject in rootGameObjects)
            {
                foreach (var component in gameObject.GetComponentsInChildren<T>(includeInactive))
                {
                    if (!comparer.Equals(component, default))
                    {
                        yield return component;
                    }
                }
            }
        }
    }
}