using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.GameEntry
{
    public class LoadingScreen : MonoBehaviour
    {
        private static LoadingScreen s_Instance;

        public static LoadingScreen Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    // 注意: 这个方法在场景里没有预先放置池的情况下也能创建池
                    // 但在Unity开发中，更推荐将池作为Prefab放置在场景中，并在编辑器中进行配置
                    s_Instance = FindObjectOfType<LoadingScreen>();
                    if (s_Instance == null)
                    {
                        var obj = new GameObject("LoadingScreen");
                        s_Instance = obj.AddComponent<LoadingScreen>();
                    }
                }
                return s_Instance;
            }
        }

        [SerializeField] private TMP_Text m_progressTxt;
        [SerializeField] private Image m_windMill;

        private void Awake()
        {
            // 确保只有一个实例
            if (s_Instance == null)
            {
                s_Instance = this;
                DontDestroyOnLoad(gameObject); // 防止多个场景加载时销毁
            }
            else
            {
                Destroy(gameObject);
            }
            
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                m_windMill.rectTransform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 50);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void UpdateProgress(float progress)
        {
            m_progressTxt.text = progress.ToString("P0");
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}