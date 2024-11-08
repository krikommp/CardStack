using Client.GameLogic.Card;
using UnityEngine;

namespace Client.GameLogic.UICardTransform
{
    public class UICardMotionMovement : UICardMotionBase
    {
        public UICardMotionMovement(UICard handler) : base(handler)
        {
        }

        #region Overrides of UICardMotionBase

        /// <summary>
        /// 运动终止函数
        /// </summary>
        protected override void OnMotionEnds()
        {
            IsOperating = false;

            var target = Target;
            var rectTransform = Handler.RectTransform;
            target.z = 0.0f;
            
            rectTransform.anchoredPosition = target;
            
            base.OnMotionEnds();
        }

        /// <summary>
        /// 检查终止条件
        /// </summary>
        /// <returns></returns>
        protected override bool CheckFinishState()
        {
            var anchoredPosition = Handler.RectTransform.anchoredPosition;
            var anchoredPosition3 = new Vector3(anchoredPosition.x, anchoredPosition.y, Target.z);
            
            var distance = Target - anchoredPosition3;
            
            return distance.magnitude <= Threshold;
        }

        /// <summary>
        /// 运动函数
        /// </summary>
        protected override void KeepMotion()
        {
            var anchoredPosition = Handler.RectTransform.anchoredPosition;
            var anchoredPosition3 = new Vector3(anchoredPosition.x, anchoredPosition.y, Target.z);

            float speed = Speed * Time.deltaTime;
            var delta = Vector3.Lerp(anchoredPosition3, Target, speed);
            
            Handler.RectTransform.anchoredPosition = delta;
        }

        #endregion
    }
}