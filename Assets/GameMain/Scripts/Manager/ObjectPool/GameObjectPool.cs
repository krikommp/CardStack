using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Client.GameLogic.Manager
{
    public class GameObjectPool<T> : IManagerInitializable where T : MonoBehaviour, IGameObjectPoolable
    { 
        private AssetReference m_prefabRef;
        private int m_poolSize = kDefaultPoolSize;
        private Transform m_parentTransform;

        private GameObject m_prefab;
        private Queue<T> m_pool = new();
        private static readonly int kDefaultPoolSize = 20;
        private bool m_isInit = false;
        
        public GameObjectPool(Transform transform, AssetReference prefabRef)
        {
            m_parentTransform = transform;
            m_prefabRef = prefabRef;
        }

        public T Get(object arg = null)
        {
            if (m_pool.Count == 0)
            {
                Debug.LogWarning("[GameObjPool] 池中对象不足，临时创建出一个，请尽快回收！！");
                AddObjects(m_prefab, 1);
            }
            
            var obj = m_pool.Dequeue();

            obj.gameObject.SetActive(true);
            obj.PoolInit(arg);
            
            Debug.Log($"[GameObjPool] {obj.name} 已取出");
            
            return obj;
        }
        
        public void ReturnToPool(T obj)
        {
            if (obj == null)
            {
                Debug.LogError("[GameObjPool] 回收的对象为空");
            }
            
            obj.gameObject.transform.SetParent(m_parentTransform);
            obj.PoolRecycle();
            obj.gameObject.SetActive(false);
            
            m_pool.Enqueue(obj);
            
            Debug.Log($"[GameObjPool] {obj.name} 已回收");
        }
        
        private void InitPool()
        {
            foreach (var item in m_pool)
            {
                item.PoolRecycle();
            }

            foreach (var item in m_pool)
            {
                GameObject.Destroy(item.gameObject);
            }
            
            m_pool.Clear();
            m_isInit = false;
            
            if (m_prefabRef == null)
            {
                Debug.LogError("[GameObjPool] 无法加载池对象 prefab, 可能是没有设置 prefabRef");
                return;
            }

            m_prefabRef.LoadAssetAsync<GameObject>().Completed += OnPrefabLoaded;
        }

        private void OnPrefabLoaded(AsyncOperationHandle<GameObject> obj)
        {
            m_prefab = obj.Result;
            AddObjects(m_prefab, m_poolSize);
            m_isInit = true;
        }

        private void AddObjects(GameObject prefab, int count, bool needRecycle = false)
        {
            if (prefab == null)
            {
                Debug.LogError("[GameObjPool] 无法创建空对象");
                return;
            }
        
            for (int i = 0; i < count; i++)
            {
                var newObject = GameObject.Instantiate(prefab, this.m_parentTransform, true);
                newObject.gameObject.SetActive(false);

                var newObjectPoolableComp = newObject.GetComponent<T>();
        
                if (newObjectPoolableComp == null)
                {
                    Debug.LogError($"[GameObjPool] {nameof(newObject)} 不含有 {nameof(IGameObjectPoolable)} 组件, 无法加入池中, 请检查是否实现了 {nameof(IGameObjectPoolable)}");
                    return;
                }
        
                if (needRecycle)
                {
                    newObjectPoolableComp.PoolRecycle();
                }
        
                Debug.Log($"[GameObjPool] {m_prefab.name} 创建成功, 正在加入池中...({i + 1}/{count})");
        
                m_pool.Enqueue(newObjectPoolableComp);
            }
        }

        #region Implementation of IManagerInitializable

        public IEnumerator ManagerInit()
        {
            InitPool();

            while (!m_isInit)
            {
                yield return null;
            }
            
            Debug.Log("[GameObjPool] 初始化完成");
        }

        public int Priority { get; } = (int)EManagerPriority.Normal;

        #endregion
    }
}