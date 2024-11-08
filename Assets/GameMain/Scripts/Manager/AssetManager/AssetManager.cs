using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cfg.card;
using Client.GameLogic.Settings;
using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Client.GameLogic.Manager.AssetManager
{ 
    public class AssetManager : IManagerInitializable
    {
        public CardAssetDevelopSettings CardAssetDevelopSettings { get; private set; }
        public CardParameterDevelopSettings CardParameterDevelopSettings { get; private set; }
        public cfg.card.TbCardData TbCardData {get; private set; }
        public cfg.localize.TbLocalizeTable TbLocalizeTable {get; private set; }
        public cfg.card.TbFormula TbFormula {get; private set; }
        public cfg.card.TbIcon TbIcon {get; private set; }

        private Stack<AssetLoadRequest> m_loadAssetRequests;
        
        private void LoadCardAsset()
        {
            m_loadAssetRequests = new();
            
            m_loadAssetRequests.Push(new AssetLoadRequest()
            {
                m_assetName = "CardAssetDevelopSettings",
                m_loadAction = (obj) =>
                {
                    CardAssetDevelopSettings = obj as CardAssetDevelopSettings;
                    if (CardAssetDevelopSettings != null)
                    {
                        Debug.Log($"[AssetManager] {nameof(CardAssetDevelopSettings)} 加载完成");
                    }
                }
            });
            
            m_loadAssetRequests.Push(new AssetLoadRequest()
            {
                m_assetName = "CardParameterDevelopSettings",
                m_loadAction = (obj) =>
                {
                    CardParameterDevelopSettings = obj as CardParameterDevelopSettings;
                    if (CardParameterDevelopSettings != null)
                    {
                        Debug.Log($"[AssetManager] {nameof(CardParameterDevelopSettings)} 加载完成");
                    }
                }
            });
            
            m_loadAssetRequests.Push(new AssetLoadRequest()
            {
                m_assetName = "CardData",
                m_loadAction = (obj) =>
                {
                    var text = obj as TextAsset;

                    if (text != null)
                    {
                        var json = JSON.Parse(text.text);
                        TbCardData = new TbCardData(json);
                        Debug.Log($"[AssetManager] CardData 加载完成");
                    }
                }
            });
            
            m_loadAssetRequests.Push(new AssetLoadRequest()
            {
                m_assetName = "CardFormula",
                m_loadAction = (obj) =>
                {
                    var text = obj as TextAsset;

                    if (text != null)
                    {
                        var json = JSON.Parse(text.text);
                        TbFormula = new TbFormula(json);
                        Debug.Log($"[AssetManager] CardFormula 加载完成");
                    }
                }
            });
            
            m_loadAssetRequests.Push(new AssetLoadRequest()
            {
                m_assetName = "LocalizeTable",
                m_loadAction = (obj) =>
                {
                    var text = obj as TextAsset;

                    if (text != null)
                    {
                        var json = JSON.Parse(text.text);
                        TbLocalizeTable = new cfg.localize.TbLocalizeTable(json);
                        Debug.Log($"[AssetManager] LocalizeTable 加载完成");
                    }
                }
            });
            
            m_loadAssetRequests.Push(new AssetLoadRequest()
            {
                m_assetName = "IconTable",
                m_loadAction = (obj) =>
                {
                    var text = obj as TextAsset;

                    if (text != null)
                    {
                        var json = JSON.Parse(text.text);
                        TbIcon = new cfg.card.TbIcon(json);
                        Debug.Log($"[AssetManager] IconTable 加载完成");
                    }
                }
            });

            SendLoadAssetRequest();
        }

        private void SendLoadAssetRequest()
        {
            if (m_loadAssetRequests.Count == 0)
            {
                return;
            }

            var topRequest = m_loadAssetRequests.Peek();
            Addressables.LoadAssetAsync<Object>(topRequest.m_assetName).Completed += OnAssetLoadFinish;
        }

        private void OnAssetLoadFinish(AsyncOperationHandle<Object> obj)
        {
            var topRequest = m_loadAssetRequests.Pop();
            topRequest.m_loadAction?.Invoke(obj.Result);
            topRequest.m_bFinish = true;
            
            SendLoadAssetRequest();
        }

        #region Implementation of IManagerInitializable

        public IEnumerator ManagerInit()
        {
            LoadCardAsset();
            
            while (m_loadAssetRequests.Count != 0)
            {
                yield return null;
            }
         
            Debug.Log("[AssetManager] 资源管理器初始化完成");
        }

        public int Priority { get; } = (int)EManagerPriority.High;

        #endregion
    }
}