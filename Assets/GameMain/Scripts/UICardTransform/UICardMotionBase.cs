using System;
using System.Collections;
using Client.GameLogic.Card;
using UnityEngine;

namespace Client.GameLogic.UICardTransform
{
    /// <summary>
    /// UICard 移动基类
    /// 所有移动方法都需要继承这个类
    /// 例如：MoveTo, ScaleTo, RotateTo
    /// </summary>
    public abstract class UICardMotionBase
    {
        /// <summary>
        /// 运动完成的回调
        /// </summary>
        public Action OnFinishMotion = () => { };

        protected UICardMotionBase(UICard handler) => Handler = handler;
        
        protected UICard Handler { get; }
        
        /// <summary>
        /// 是否在运动中
        /// </summary>
        public bool IsOperating { get; protected set; }
        
        /// <summary>
        /// 运动目标
        /// </summary>
        protected Vector3 Target { get; set; }

        /// <summary>
        /// 最远运动距离限制
        /// </summary>
        protected virtual float Threshold => 0.01f;

        /// <summary>
        /// 控制运动速度
        /// </summary>
        protected float Speed { get; set; }

        /// <summary>
        /// 检查终止条件
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckFinishState();

        /// <summary>
        /// 运动函数
        /// </summary>
        protected abstract void KeepMotion();
        
        /// <summary>
        /// 运动终止函数
        /// </summary>
        protected virtual void OnMotionEnds() => OnFinishMotion?.Invoke();
        
        /// <summary>
        /// 强制终止运动
        /// </summary>
        public virtual void StopMotion() => IsOperating = false;
        
        /// <summary>
        /// 执行运动
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="speed"></param>
        /// <param name="delay"></param>
        public virtual void Execute(Vector3 vector, float speed, float delay) {
            Speed = speed;
            Target = vector;
            if (delay == 0) {
                IsOperating = true;
            }
            else {
                Handler.MonoBehaviour.StartCoroutine(AllowMotion(delay));
            }
        }

        public virtual void Execute(Vector3 vector)
        {
            Target = vector;
            IsOperating = true;
            OnMotionEnds();
        }

        IEnumerator AllowMotion(float delay) {
            yield return new WaitForSeconds(delay);
            IsOperating = true;
        }

        public void Update() {
            if (!IsOperating) {
                return;
            }

            if (CheckFinishState()) {
                OnMotionEnds();
            }
            else {
                KeepMotion();
            }
        }
    }
}