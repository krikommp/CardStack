using UnityEngine;
using GameStateHandler = System.Int32;

namespace Client.GameLogic.Manager.SceneLogicManager.GameStateChangeCondition
{
    public class GSC_EntryToMainMenu : GameStateChangeCondition
    {
        #region Overrides of GameStateChangeCondition

        public override bool CanChange()
        {
            Debug.Log("GSC_EntryToMainMenu CanChange");
            return true;
        }

        #endregion

        public GSC_EntryToMainMenu(SceneLogicManager sceneLogicManager, GameStateHandler fromState, GameStateHandler toState) : base(sceneLogicManager, fromState, toState)
        {
        }
    }
}