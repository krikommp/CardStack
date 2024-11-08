using System;
using cfg;
using Client.GameLogic.Manager;
using Client.GameLogic.UICardTransform;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.GameLogic.Card
{
    public sealed class UICard : MonoBehaviour, IGameObjectPoolable
    {
        #region Components

        private Action<UICard> m_onAllMotionFinish = card => { };
        
        private UICardMotionBase m_movement;
        private UICardMotionBase m_scale;
        private UICardMotionBase m_rotation;

        private MonoBehaviour m_monoBehaviour;
        private RectTransform m_rectTransform;
        private Image m_image;
        [SerializeField] private TMP_Text m_cardName;
        [SerializeField] private Image m_cardIcon;
        
        public ECardMaintainingState m_maintainingState = ECardMaintainingState.None;
        
        public UICardMotionBase Movement => m_movement;
        public UICardMotionBase Scale => m_scale;
        public UICardMotionBase Rotation => m_rotation;
        public MonoBehaviour MonoBehaviour => m_monoBehaviour;
        public RectTransform RectTransform => m_rectTransform;
        
        #endregion

        #region Module

        private CardData m_cardData;
        private CardInfo m_cardInfo;
        
        public CardData CardData
        {
            get { return m_cardData; }
        }
        
        public CardInfo CardInfo
        {
            get { return m_cardInfo; }
        }

        public static UICard Create(int cardId)
        {
            var cardData = GlobalManager.AssetManager.TbCardData.GetOrDefault(cardId);
            if (cardData == null)
            {
                return null;
            }
            
            var card = GlobalManager.CardPool.Get();
            card.m_cardData = cardData;
            card.m_cardInfo = new CardInfo();
            
            card.Setup();
            
            return card;
        }

        private void Setup()
        {
            m_cardName.text = GlobalManager.LocalizeManager.GetLocalizeString(m_cardData.Name);
            m_cardIcon.sprite = GlobalManager.IconManager.TryGetCardIcon(m_cardData.Id);
        }

        #endregion
        
        #region Implementation of IGameObjectPoolable

        public void PoolInit(object arg)
        {
            m_movement = new UICardMotionMovement(this);
            m_scale = new UICardMotionScale(this);
            m_rotation = new UICardMotionRotation(this);

            m_monoBehaviour = this;
            m_rectTransform = GetComponent<RectTransform>();
            m_image = GetComponent<Image>();
            
            m_rectTransform.sizeDelta = GlobalManager.AssetManager.CardParameterDevelopSettings.CardSizeDelta;
            m_rectTransform.localScale = GlobalManager.AssetManager.CardParameterDevelopSettings.CardDefaultScale;
            m_rectTransform.pivot = GlobalManager.AssetManager.CardParameterDevelopSettings.CardDefaultPivot;

            m_movement.OnFinishMotion += OnMotionFinish;
            m_scale.OnFinishMotion += OnMotionFinish;
            m_rotation.OnFinishMotion += OnMotionFinish;
        }

        public void PoolRecycle()
        {
            MoveTo(Vector3.zero);
            ScaleTo(Vector3.one);
            RotateTo(Vector3.zero);
            
            m_movement = null;
            m_scale = null;
            m_rotation = null;
            
            m_monoBehaviour = null;
            m_rectTransform = null;
        }

        #endregion

        #region UnityEvents

        private void FixedUpdate()
        {
            m_movement?.Update();
            m_scale?.Update();
            m_rotation?.Update();
        }

        #endregion
        
        #region Movement

        public Action<UICard> OnAllMotionFinish
        {
            get { return m_onAllMotionFinish; }
            set { m_onAllMotionFinish = value; }
        }

        private void OnMotionFinish()
        {
            if (m_movement.IsOperating || m_scale.IsOperating || m_rotation.IsOperating)
            {
                return;
            }
            
            m_onAllMotionFinish?.Invoke(this);
        }

        public void MoveTo(Vector3 postion, float speed, float delay = 0)
        {
            Movement.Execute(postion, speed, delay);
        }

        public void MoveTo(Vector3 postion)
        {
            Movement.Execute(postion);
        }

        public void RotateTo(Vector3 rotation,float speed,float delay = 0)
        {
            Rotation.Execute(rotation, speed, delay);
        }
        
        public void RotateTo(Vector3 rotation)
        {
            Rotation.Execute(rotation);
        }

        public void ScaleTo(Vector3 scale, float speed, float delay = 0)
        {
            Scale.Execute(scale, speed, delay);
        }
        
        public void ScaleTo(Vector3 scale)
        {
            Scale.Execute(scale);
        }

        #endregion

        #region Raycast
        public void EnableRaycast() => m_image.raycastTarget = true;
        public void DisableRaycast() => m_image.raycastTarget = false;
        #endregion
    }
}