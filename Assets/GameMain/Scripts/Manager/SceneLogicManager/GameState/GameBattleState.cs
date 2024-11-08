
using System;
using System.Collections;
using GameMain.Scripts.GameEntry;
using UnityEngine;

namespace Client.GameLogic.Manager.SceneLogicManager.GameState
{
    public class GameBattleState : GameBaseState
    {
        #region Overrides of GameBaseState

        public override void OnEnter()
        { 
            GlobalManager.CardManager.FindGameSceneObjects();
            
            GlobalManager.Instance.StartCoroutine(DelayTest());
        }

        public override void OnUpdate()
        {
        }

        public override void OnLeave()
        {
            GlobalManager.CardManager.ClearGameSceneObjects();
        }

        IEnumerator DelayTest()
        {
            yield return new WaitForSeconds(1.0f);
            GlobalManager.CardManager.GiveACard(211007, Vector2.zero);
            GlobalManager.CardManager.GiveACard(211007, Vector2.one * 100);
            GlobalManager.CardManager.GiveACard(310002, Vector2.one * 200);
            GlobalManager.CardManager.GiveACard(310003, Vector2.one * 300);
            
            LoadingScreen.Instance.Hide();
        }

        #endregion
    }
}