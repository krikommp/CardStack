using UnityEngine;

namespace Client.GameLogic.Manager
{
    public interface IGameObjectPoolable
    {
        void PoolInit(object arg);
        void PoolRecycle(); 
    }

    public static class GameObjectPoolableUtil
    {
        public static IGameObjectPoolable GetPoolable(this GameObject gameObject)
        {
            return gameObject.GetComponent<IGameObjectPoolable>();
        }
    }
}