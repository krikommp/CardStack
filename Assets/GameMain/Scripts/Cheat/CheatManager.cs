using TMPro;
using UnityEngine;

namespace GameMain.Scripts.Cheat
{
    public class CheatManager : MonoBehaviour
    {
        #region 可视变量
        [SerializeField] [Tooltip("控制台输入框对象。")] private TMP_InputField inputField = null;
        [SerializeField] [Tooltip("控制台输出框对象。")] private TMP_Text ouputText = null;
        [SerializeField] [Tooltip("控制台对象")] private GameObject m_consolePanel = null;
        #endregion

        private static CheatManager s_Instance;

        public static CheatManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    // 注意: 这个方法在场景里没有预先放置池的情况下也能创建池
                    // 但在Unity开发中，更推荐将池作为Prefab放置在场景中，并在编辑器中进行配置
                    s_Instance = FindObjectOfType<CheatManager>();
                    if (s_Instance == null)
                    {
                        var obj = new GameObject("CheatManager");
                        s_Instance = obj.AddComponent<CheatManager>();
                    }
                }
                return s_Instance;
            }
        }

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
            
            m_consolePanel.gameObject.SetActive(false);
        }
        
        #region 功能方法
        /// <summary>
        /// 第一帧调用之前触发。
        /// </summary>
        private void Start()
        {
            inputField.ActivateInputField();
        }

        /// <summary>
        /// 在帧刷新时触发。
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                m_consolePanel.gameObject.SetActive(!m_consolePanel.gameObject.activeSelf);
            }
            
            if (!m_consolePanel.gameObject.activeSelf)
                return;

            string input = inputField.text;     // 获取输入文本
            // 按下回车输入指令
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!input.Equals(""))
                {
                    ouputText.text += ">>" + input + "\n";
                    string output = Console.Input(input);
                    if (output != null)
                    {
                        // 回调信息为cls时清空控制台面板内容
                        if (output.Equals("cls"))
                            ouputText.text = "";
                        else
                            ouputText.text += output + "\n";
                    }
                    inputField.text = "";
                }
            }
            // 按下上跳转到上一条指令
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                inputField.text = Console.Last();
            // 按下下跳转到下一条指令
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                inputField.text = Console.Next();
            inputField.ActivateInputField();
        }
        #endregion
    }
}