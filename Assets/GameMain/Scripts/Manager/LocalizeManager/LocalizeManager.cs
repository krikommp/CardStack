using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.GameLogic.Manager.LocalizeManager
{
    public enum ELocalizeLanguage
    {
        zhCN,
        zhTW,
        enUS
    }
    
    public class LocalizeManager : IManagerInitializable
    {
        #region Implementation of IManagerInitializable

        public IEnumerator ManagerInit()
        {
            LoadLocalizeData();
            yield return null;
            
            Debug.Log("[LocalizeManager] 本地化管理器初始化完成");
        }

        public int Priority { get; } = (int)EManagerPriority.Normal;

        #endregion
        
        private Dictionary<string, string> m_zhCNLocalizeDic = new();
        private Dictionary<string, string> m_zhTWLocalizeDic = new();
        private Dictionary<string, string> m_enUSLocalizeDic = new();
        
        private ELocalizeLanguage m_currentLanguage = ELocalizeLanguage.enUS;

        private void LoadLocalizeData()
        {
            GlobalManager.AssetManager.TbLocalizeTable.DataList.ForEach(localizeData =>
            {
                m_zhCNLocalizeDic.Add(localizeData.LocalizeKey, localizeData.ZhCn);
                m_zhTWLocalizeDic.Add(localizeData.LocalizeKey, localizeData.ZhTw);
                m_enUSLocalizeDic.Add(localizeData.LocalizeKey, localizeData.EnUs);
            });
        }
        
        public string GetLocalizeString(string key)
        {
            switch (m_currentLanguage)
            {
                case ELocalizeLanguage.zhCN:
                    return m_zhCNLocalizeDic[key];
                case ELocalizeLanguage.zhTW:
                    return m_zhTWLocalizeDic[key];
                case ELocalizeLanguage.enUS:
                    return m_enUSLocalizeDic[key];
                default:
                    return string.Empty;
            }
        }
    }
}