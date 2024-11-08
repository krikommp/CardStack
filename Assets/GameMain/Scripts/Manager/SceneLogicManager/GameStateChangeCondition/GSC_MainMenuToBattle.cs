using Client.GameLogic.Manager.SceneLogicManager.GameState;
using UnityEngine;
using GameStateHandler = System.Int32;

namespace Client.GameLogic.Manager.SceneLogicManager.GameStateChangeCondition
{
    public class GSC_MainMenuToBattle : GameStateChangeCondition
    {
        #region Overrides of GameStateChangeCondition

        public override bool CanChange()
        {
            var fromState = GetFromState<GameMainMenuState>();
            if (fromState != null)
            {
                return fromState.IsEnterGameBattle;
            }
            return false;
        }

        #endregion

        public GSC_MainMenuToBattle(SceneLogicManager sceneLogicManager, GameStateHandler fromState, GameStateHandler toState) : base(sceneLogicManager, fromState, toState)
        {
        }
    }
}