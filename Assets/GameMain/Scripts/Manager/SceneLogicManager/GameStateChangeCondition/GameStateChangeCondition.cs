using Client.GameLogic.Manager.SceneLogicManager.GameState;
using GameStateHandler = System.Int32;

namespace Client.GameLogic.Manager.SceneLogicManager.GameStateChangeCondition
{
    public abstract class GameStateChangeCondition
    {
        public SceneLogicManager m_sceneLogicManager;
        public GameStateHandler m_fromState;
        public GameStateHandler m_toState;
        
        public GameStateChangeCondition(SceneLogicManager sceneLogicManager, GameStateHandler fromState, GameStateHandler toState)
        {
            m_sceneLogicManager = sceneLogicManager;
            m_fromState = fromState;
            m_toState = toState;
        }
        
        public abstract bool CanChange();
        
        protected T GetFromState<T>() where T : GameBaseState
        {
            return m_sceneLogicManager.GetGameState(m_fromState) as T;
        }
    }
}