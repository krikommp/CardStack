using System;
using UnityEngine;

namespace Client.GameLogic.Manager
{
    public class GameStateManager : MonoBehaviour
    {
        private static GameStateManager s_Instance;

        public static GameStateManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    // 注意: 这个方法在场景里没有预先放置池的情况下也能创建池
                    // 但在Unity开发中，更推荐将池作为Prefab放置在场景中，并在编辑器中进行配置
                    s_Instance = FindObjectOfType<GameStateManager>();
                    if (s_Instance == null)
                    {
                        var obj = new GameObject("GameStateManager");
                        s_Instance = obj.AddComponent<GameStateManager>();
                    }
                }
                return s_Instance;
            }
        }

        private SceneLogicManager.SceneLogicManager m_sceneLogicManager;
        
        public static SceneLogicManager.SceneLogicManager SceneLogicManager
        {
            get { return Instance.m_sceneLogicManager; }
        }

        private void Awake()
        {
            m_sceneLogicManager = new SceneLogicManager.SceneLogicManager();

            StartCoroutine(m_sceneLogicManager.ManagerInit());
        }
        
        private void Start()
        {
            m_sceneLogicManager.Start();
        }
        
        private void Update()
        {
            m_sceneLogicManager.Update();
        }
        
        private void OnDestroy()
        {
            m_sceneLogicManager = null;
        }
    }
}