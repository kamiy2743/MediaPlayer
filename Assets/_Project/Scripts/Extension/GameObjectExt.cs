using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaPlayer
{
    public static class GameObjectExt
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent(out T component))
            {
                return component;
            }

            return gameObject.AddComponent<T>();
        }
    }
}
