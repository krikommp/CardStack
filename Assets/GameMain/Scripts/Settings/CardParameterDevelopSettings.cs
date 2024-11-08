using UnityEngine;

namespace Client.GameLogic.Settings
{
    [CreateAssetMenu(menuName = "CardDevelop/Card Parameters Settings")]
    public class CardParameterDevelopSettings : ScriptableObject
    {
        [SerializeField] [Tooltip("卡牌旋转的速度")] [Range(0,60)] private float m_rotationSpeed;
        [SerializeField] [Tooltip("卡牌移动速度")] [Range(0,15)] private float m_movementSpeed; 
        [SerializeField] [Tooltip("卡牌缩放速度")] [Range(0,15)] private float m_scaleSpeed;

        [SerializeField] [Tooltip("拖拽卡牌时偏移")] private Vector2 m_cardDragOffset;
        [SerializeField] [Tooltip("堆叠时卡牌的偏移")] private Vector2 m_cardPileOffset;

        [SerializeField] [Tooltip("卡牌原始大小")] private Vector2 m_cardSizeDelta; 
        [SerializeField] [Tooltip("卡牌默认缩放比例")] private Vector3 m_cardDefaultScale;
        [SerializeField] [Tooltip("卡牌默认锚点")] private Vector2 m_cardDefaultPivot;

        [SerializeField] [Tooltip("卡牌回收缩放")] private Vector3 m_cardRecoveryScale;

        [SerializeField] [Tooltip("避免卡牌重叠时需要的偏移")] private Vector2 m_cardOverlayOffset;

        public float RotationSpeed
        {
            get { return m_rotationSpeed; }
        }

        public float MovementSpeed
        {
            get { return m_movementSpeed; }
        }

        public float ScaleSpeed
        {
            get { return m_scaleSpeed; }
        }

        public Vector2 CardDragOffset
        {
            get { return m_cardDragOffset; }
        }

        public Vector2 CardPileOffset
        {
            get { return m_cardPileOffset; }
        }

        public Vector2 CardSizeDelta
        {
            get { return m_cardSizeDelta; }
        }

        public Vector3 CardDefaultScale
        {
            get { return m_cardDefaultScale; }
        }

        public Vector2 CardDefaultPivot
        {
            get { return m_cardDefaultPivot; }
        }

        public Vector3 CardRecoveryScale
        {
            get { return m_cardRecoveryScale; }
        }

        public Vector2 CardOverlayOffset
        {
            get { return m_cardOverlayOffset; }
        }
        
    }
}