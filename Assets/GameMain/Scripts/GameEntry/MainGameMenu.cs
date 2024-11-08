using System;
using Client.GameLogic.Manager;
using Client.GameLogic.Manager.SceneLogicManager.GameState;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.GameEntry
{
    [Serializable]
    public enum EGameButtonType
    {
        START,
        RELOAD,
        SETTINGS,
    }

    public class MainGameMenu : MonoBehaviour
    {
        [SerializeField] private EGameButtonType m_buttonType;
        private Button m_button;
        private TMP_Text m_text;
        
        private void Awake()
        {
            m_button = GetComponent<Button>();
            m_text = m_button.GetComponentInChildren<TMP_Text>();
        }

        public void OnButtonClick()
        {
            switch (m_buttonType)
            {
                case EGameButtonType.START:
                    var gameState = GameStateManager.SceneLogicManager.GetCurrentGameState<GameMainMenuState>();
                    if (gameState == null)
                    {
                        return;
                    }

                    gameState.EntreGameBattle();
                    break;
                default:
                    break;
            }
        }
    }
}