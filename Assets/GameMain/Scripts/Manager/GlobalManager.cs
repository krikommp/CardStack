using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Client.GameLogic.Card;
using Client.GameLogic.CardPile;
using Client.GameLogic.Manager.AssetManager;
using Client.GameLogic.Manager.GameManager;
using Client.GameLogic.State;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Client.GameLogic.Manager
{
    public enum EManagerState
    {
        NotStarted,
        Loading,
        Loaded,
        PostLoad
    }

    public class GlobalManager : MonoBehaviour
    {
        private AssetManager.AssetManager m_assetManager;
        private GuideManager.GuideManager m_guideManager;
        private AssetManager.IconManager m_iconManager;
        private LocalizeManager.LocalizeManager m_localizeManager;

        private GameManager.CardManager m_cardManager;
        private GameManager.TimeManager m_timeManager;
        
        private GameObjectPool<UICard> m_cardPool;
        private GameObjectPool<CardStack> m_cardStackPool;

        private List<IManagerInitializable> m_managers = new List<IManagerInitializable>();
        private List<IManagerStart> m_needStartManagers = new List<IManagerStart>();
        private List<IManagerUpdate> m_needUpdateManagers = new List<IManagerUpdate>();
        
        private EManagerState m_managerState = EManagerState.NotStarted;
        
        private static GlobalManager s_Instance;

        public static GlobalManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    // 注意: 这个方法在场景里没有预先放置池的情况下也能创建池
                    // 但在Unity开发中，更推荐将池作为Prefab放置在场景中，并在编辑器中进行配置
                    s_Instance = FindObjectOfType<GlobalManager>();
                    if (s_Instance == null)
                    {
                        var obj = new GameObject("GlobalManager");
                        s_Instance = obj.AddComponent<GlobalManager>();
                    }
                }
                return s_Instance;
            }
        }

        public static AssetManager.AssetManager AssetManager
        {
            get { return s_Instance.m_assetManager; }
        }

        public static GameObjectPool<UICard> CardPool
        {
            get { return s_Instance.m_cardPool; }
        }
        
        public static GameObjectPool<CardStack> CardStackPool
        {
            get { return s_Instance.m_cardStackPool; }
        }

        public static GuideManager.GuideManager GuideManager
        {
            get { return s_Instance.m_guideManager; }
        }
        
        public static CardManager CardManager
        {
            get { return s_Instance.m_cardManager; }
        }

        public static TimeManager TimeManager
        {
            get { return s_Instance.m_timeManager; }
        }

        public static IconManager IconManager
        {
            get { return s_Instance.m_iconManager; }
        }
        
        public static LocalizeManager.LocalizeManager LocalizeManager
        {
            get { return s_Instance.m_localizeManager; }
        }
        
        public static EManagerState ManagerState
        {
            get { return s_Instance.m_managerState; }
        }

        public void Awake()
        {
            // 确保只有一个实例
            if (s_Instance == null)
            {
                s_Instance = this;
                DontDestroyOnLoad(gameObject); // 防止多个场景加载时销毁
            }
            else
            {
                Destroy(gameObject);
            }
            
            m_cardPool = new GameObjectPool<UICard>(transform, new AssetReference("Card"));
            m_managers.Add(m_cardPool);
            m_cardStackPool = new GameObjectPool<CardStack>(transform, new AssetReference("CardStack"));
            m_managers.Add(m_cardStackPool);
            m_assetManager = new AssetManager.AssetManager();
            m_managers.Add(m_assetManager);
            m_guideManager = new GuideManager.GuideManager();
            m_managers.Add(m_guideManager);
            m_iconManager = new IconManager();
            m_managers.Add(m_iconManager);
            m_localizeManager = new LocalizeManager.LocalizeManager();
            m_managers.Add(m_localizeManager);

            m_cardManager = new CardManager();
            m_managers.Add(m_cardManager);
            m_timeManager = new TimeManager();
            m_managers.Add(m_timeManager);
                
            m_managers = m_managers.OrderBy(x => x.Priority).ToList();

            CollectSpecialManagers();
        }

        private void Update()
        {
            switch (m_managerState)
            {
                case EManagerState.NotStarted:
                    break;
                case EManagerState.Loading:
                    break;
                case EManagerState.Loaded:
                    OnManagerLoaded();
                    break;
                case EManagerState.PostLoad:
                    OnManagerPosLoaded();
                    break;
                default:
                    Debug.LogError("[GlobalManager] 未知的管理器状态");
                    break;
            }
        }

        private void CollectSpecialManagers()
        {
            foreach (var manager in m_managers)
            {
                if (manager.GetType().GetInterface(nameof(IManagerStart)) != null)
                {
                    m_needStartManagers.Add(manager as IManagerStart);
                }

                if (manager.GetType().GetInterface(nameof(IManagerUpdate)) != null)
                {
                    m_needUpdateManagers.Add(manager as IManagerUpdate);
                }
            }
        }

        public float progress = 0.0f;
        
        public void InitializeSequence()
        {
            Debug.Log("[GlobalManager] 开始初始化管理器");
            m_managerState = EManagerState.Loading;
            progress = 0.0f;

            InitManagers(0);
        }

        private void InitManagers(int idx)
        {
            if (idx >= m_managers.Count)
            {
                Debug.Log("[GlobalManager] 管理器初始化完成");
                m_managerState = EManagerState.Loaded;
                progress = 1.0f;
                
                return;
            }

            var initializable = m_managers[idx];
            progress = (float)(idx) / m_managers.Count;
            StartCoroutine(InitManager(idx, initializable));
        }

        private IEnumerator InitManager(int idx, IManagerInitializable initializable)
        {
            yield return initializable.ManagerInit();
            
            InitManagers(idx + 1);
        }

        private void OnManagerLoaded()
        {
            foreach (var manager in m_needStartManagers)
            {
                manager.Start();
            }

            m_managerState = EManagerState.PostLoad;
        }

        private void OnManagerPosLoaded()
        {
            foreach (var manager in m_needUpdateManagers)  
            {
                manager.Update();
            }
        }
    }
}