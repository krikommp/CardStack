using System.Collections;
using GameMain.Scripts.GameEntry;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Client.GameLogic.Manager.SceneLogicManager.GameState
{
    public class GameMainMenuState : GameBaseState
    {
        private bool m_isEnterGameBattle;
        
        public bool IsEnterGameBattle
        {
            get { return m_isEnterGameBattle; }
        }
        
        #region Overrides of GameBaseState

        public override void OnEnter()
        {
            m_isEnterGameBattle = false;
        }

        public override void OnUpdate()
        {
        }

        public override void OnLeave()
        {
        }

        #endregion

        public void EntreGameBattle()
        {
            LoadingScreen.Instance.Show();
            GameStateManager.Instance.StartCoroutine(StartToLoadAsset());
        }

        private IEnumerator StartToLoadAsset()
        {
            GlobalManager.Instance.InitializeSequence();

            while (GlobalManager.ManagerState != EManagerState.Loaded)
            {
                LoadingScreen.Instance.UpdateProgress(GlobalManager.Instance.progress * 0.5f);
                yield return null; 
            }
            
            var asyncOperation = Addressables.LoadSceneAsync("GameBattle");
            
            while (!asyncOperation.IsDone)
            {
                LoadingScreen.Instance.UpdateProgress(0.5f + asyncOperation.PercentComplete * 0.5f);
                yield return null;
            }
            
            m_isEnterGameBattle = true;
        }
    }
}