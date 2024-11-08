using Client.GameLogic.Card;
using UnityEngine;

namespace Client.GameLogic.UICardTransform
{
    public class UICardMotionScale : UICardMotionBase
    {
        public UICardMotionScale(UICard handler) : base(handler)
        {
        }

        #region Overrides of UICardMotionBase

        /// <summary>
        /// 检查终止条件
        /// </summary>
        /// <returns></returns>
        protected override bool CheckFinishState()
        {
            var delta = Target - Handler.RectTransform.localScale;
            return delta.magnitude <= Threshold;
        }

        /// <summary>
        /// 运动函数
        /// </summary>
        protected override void KeepMotion()
        {
            var current = Handler.RectTransform.localScale;
            float amount = Time.deltaTime * Speed;
            Handler.RectTransform.localScale = Vector3.Lerp(current, Target, amount);
        }

        /// <summary>
        /// 运动终止函数
        /// </summary>
        protected override void OnMotionEnds()
        {
            Handler.RectTransform.localScale = Target;
            IsOperating = false;
            base.OnMotionEnds();
        }

        #endregion
    }
}