using Client.GameLogic.Card;
using Client.GameLogic.Manager;
using UnityEngine;

namespace Client.GameLogic.CardPile
{
    [RequireComponent(typeof(CardPackage))]
    public class CardPackageSorter : MonoBehaviour
    {
        private CardPackage m_cardPackage;

        private void Awake()
        {
            m_cardPackage = GetComponent<CardPackage>();
            
            m_cardPackage.OnPileChange += OnPileChange;
        }

        private void OnPileChange(UICard[] cards)
        {
            float moveSpeed = GlobalManager.AssetManager.CardParameterDevelopSettings.MovementSpeed;
            float scaleSpeed = GlobalManager.AssetManager.CardParameterDevelopSettings.ScaleSpeed;
            var recoveryScale = GlobalManager.AssetManager.CardParameterDevelopSettings.CardRecoveryScale;
            
            for (int i = 0; i < cards.Length; ++i)
            {
                cards[i].MoveTo(new Vector3(0, 0, 1.0f), moveSpeed);
                cards[i].ScaleTo(recoveryScale, scaleSpeed);
                cards[i].OnAllMotionFinish = OnAllMotionFinish;
            }
        }

        private void OnAllMotionFinish(UICard card)
        {
            m_cardPackage.Discard(card);
            card.OnAllMotionFinish = null;
            GlobalManager.CardPool.ReturnToPool(card);
            
            m_cardPackage.Test();
        }
    }
}