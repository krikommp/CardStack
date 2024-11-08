using System.Collections.Generic;
using GameStateHandler = System.Int32;

namespace Client.GameLogic.Manager.SceneLogicManager.GameState
{
    public abstract class GameBaseState
    {
        protected SceneLogicManager m_sceneLogicManager;

        public virtual void OnCreate(SceneLogicManager sceneLogicManager)
        {
            m_sceneLogicManager = sceneLogicManager;
        }

        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnLeave();
        
        public List<GameStateChangeCondition.GameStateChangeCondition> m_conditions;

        public bool TryChangeState(out GameStateHandler toGameStateHandler)
        {
            toGameStateHandler = SceneLogicManager.kInvalidGameStateHandler;

            if (m_conditions != null)
            {
                foreach (var condition in m_conditions)
                {
                    if (condition.CanChange())
                    {
                        toGameStateHandler = condition.m_toState;
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}