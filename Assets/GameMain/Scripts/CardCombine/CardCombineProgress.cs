using System;
using Client.GameLogic.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.CardCombine
{
    public class CardCombineProgress : MonoBehaviour
    {
        private Slider m_slider;
        private DateTime m_startTime;
        private DateTime m_endTime;
        private bool m_isCombine = false;
        private RectTransform m_rectTransform;

        public RectTransform RectTransform
        {
            get { return m_rectTransform; }
        }
        
        private void Awake()
        {
            m_slider = GetComponent<Slider>();
            m_rectTransform = GetComponent<RectTransform>();
        }

        public void StartCombine(DateTime endTime)
        {
            m_startTime = GlobalManager.TimeManager.GetGameDateTime();
            m_endTime = endTime;
            m_isCombine = true;
            gameObject.SetActive(true);
        }
        
        public void StopCombine()
        {
            m_isCombine = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (m_isCombine)
            {
                double totalSeconds = (m_endTime.ToUniversalTime() - m_startTime.ToUniversalTime()).TotalSeconds;
                double currentSeconds = (GlobalManager.TimeManager.GetGameDateTime() - m_startTime.ToUniversalTime()).TotalSeconds;

                double progress = totalSeconds == 0 ? 0 : currentSeconds / totalSeconds;
                progress = Mathf.Clamp01((float)progress);

                m_slider.value = (float)progress;
                
                if (currentSeconds >= totalSeconds)
                {
                    m_slider.value = 1.0f;
                    m_isCombine = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}