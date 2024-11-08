using Client.GameLogic.Card;
using UnityEngine;

namespace Client.GameLogic.UICardTransform
{
    public class UICardMotionRotation : UICardMotionBase
    {
        public UICardMotionRotation(UICard handler) : base(handler)
        {
        }

        #region Overrides of UICardMotionBase

        /// <summary>
        /// 运动终止函数
        /// </summary>
        protected override void OnMotionEnds()
        {
            Handler.RectTransform.eulerAngles = Target;
            IsOperating = false;
            base.OnMotionEnds();
        }

        /// <summary>
        /// 检查终止条件
        /// </summary>
        /// <returns></returns>
        protected override bool CheckFinishState()
        {
            var distance = Target - Handler.RectTransform.eulerAngles;
            bool smallerThanLimit = distance.magnitude <= Threshold;
            bool equals360 = (int) distance.magnitude == 360;
            bool isFinal = smallerThanLimit || equals360;
            return isFinal;
        }

        /// <summary>
        /// 运动函数
        /// </summary>
        protected override void KeepMotion()
        {
            var current = Handler.RectTransform.rotation;
            float amount = Speed * Time.deltaTime;
            var rotation = Quaternion.Euler(Target);
            var newRotation = Quaternion.RotateTowards(current, rotation, amount);
            Handler.RectTransform.rotation = newRotation;
        }

        #endregion
    }
}