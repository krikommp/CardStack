using System.Collections;
using System.Collections.Generic;
using Client.GameLogic.Manager.SceneLogicManager.GameState;
using Client.GameLogic.Manager.SceneLogicManager.GameStateChangeCondition;
using JetBrains.Annotations;
using UnityEngine;
using GameStateHandler = System.Int32;

namespace Client.GameLogic.Manager.SceneLogicManager
{
    public class SceneLogicManager : IManagerInitializable, IManagerStart, IManagerUpdate
    {
        #region Implementation of IManagerInitializable

        public IEnumerator ManagerInit()
        {
            yield return null;
            RegisterGameState();
            
            Debug.Log("[SceneLogicManager] 场景逻辑管理器初始化完成");
        }

        public int Priority { get; } = (int)EManagerPriority.Low;

        #endregion

        public void Start()
        {
            m_currentGameStateHandler = 0;
            CurrentGameState?.OnEnter();
        }

        public void Update()
        {
            CheckGameState();
            CurrentGameState?.OnUpdate();
        }

        private void RegisterGameState()
        {
            CreateNode();
            CreateEdge();
        }

        private void CheckGameState()
        {
            if (CurrentGameState == null)
            {
                return;
            }

            CurrentGameState.TryChangeState(out GameStateHandler newGameStateHandler);
            
            if (newGameStateHandler == kInvalidGameStateHandler)
            {
                return;
            }

            CurrentGameState.OnLeave();
            GetGameState(newGameStateHandler)?.OnEnter();

            m_currentGameStateHandler = newGameStateHandler;
        }

        private List<GameBaseState> m_gameStates = new();
        private GameStateHandler m_currentGameStateHandler = kInvalidGameStateHandler;
        public const GameStateHandler kInvalidGameStateHandler = -1;
        
        public GameStateHandler RequireGameState<T>() where T : GameBaseState, new()
        {
            var newGameState = new T();
            newGameState.OnCreate(this);
            m_gameStates.Add(newGameState);
            return (GameStateHandler)(m_gameStates.Count - 1);
        }
        
        private void CreateNode()
        {
            RequireGameState<GameEntryState>(); // 0
            RequireGameState<GameMainMenuState>();  // 1
            RequireGameState<GameBattleState>();    // 2
        }

        private void CreateEdge()
        {
            // 0 -> 1
            var condition1 = new GSC_EntryToMainMenu(this, 0, 1);
            m_gameStates[0].m_conditions = new List<GameStateChangeCondition.GameStateChangeCondition> {condition1};

            // 1 -> 2
            var condition2 = new GSC_MainMenuToBattle(this, 1, 2);
            m_gameStates[1].m_conditions = new List<GameStateChangeCondition.GameStateChangeCondition> {condition2};
        }
        
        public GameBaseState CurrentGameState
        {
            get
            {
                if (m_currentGameStateHandler >= 0 && m_currentGameStateHandler < m_gameStates.Count)
                {
                    return m_gameStates[m_currentGameStateHandler];
                }

                return null;
            }
        }
        
        public T GetCurrentGameState<T>() where T : GameBaseState
        {
            return CurrentGameState as T;
        }
        
        [CanBeNull]
        public GameBaseState GetGameState(GameStateHandler handler)
        {
            if (handler >= 0 && handler < m_gameStates.Count)
            {
                return m_gameStates[handler];
            }

            return null;
        }

        private GameStateBlackBoard m_stateBlackBoard;
        public GameStateBlackBoard GetOrCreateBlackBoard()
        {
            if (m_stateBlackBoard == null)
            {
                m_stateBlackBoard = new GameStateBlackBoard();
            }

            return m_stateBlackBoard;
        }
    }
}