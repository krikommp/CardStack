using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace Client.GameLogic.Manager.AssetManager
{
    public class IconManager : IManagerInitializable
    {
        private enum EIconLoadState 
        {
            Waiting,
            Loading,
            Complete,
        }
        
        #region Implementation of IManagerInitializable

        public IEnumerator ManagerInit()
        {
            StartToLoadIconAsset();
            while (m_loadState != EIconLoadState.Complete)
            {
                yield return null;
            }
            
            Debug.Log("[IconManager] 图标管理器初始化完成");
        }

        public int Priority { get; } = (int)EManagerPriority.Normal;

        #endregion

        private EIconLoadState m_loadState = EIconLoadState.Waiting;
        private int m_idx = 0;
        private HashSet<string> m_spriteAtlasSet;
        private Dictionary<string, SpriteAtlas> m_spriteAtlasDic = new Dictionary<string, SpriteAtlas>();

        private void StartToLoadIconAsset()
        {
            var tbIconDataTable = GlobalManager.AssetManager.TbIcon.DataList;
            m_loadState = EIconLoadState.Loading;
            m_idx = 0;
            m_spriteAtlasSet = new HashSet<string>();
            
            foreach (var iconData in tbIconDataTable)
            {
                m_spriteAtlasSet.Add(iconData.Path);
            }

            LoadSpriteAtlas(m_idx);
        }

        void LoadSpriteAtlas(int idx)
        {
            if (idx >= m_spriteAtlasSet.Count)
            {
                m_loadState = EIconLoadState.Complete;
                return;
            }
            string spriteAtlasName = m_spriteAtlasSet.ElementAt(idx);
            Addressables.LoadAssetAsync<SpriteAtlas>(spriteAtlasName).Completed += OnIconLoadComplete;
        }

        private void OnIconLoadComplete(AsyncOperationHandle<SpriteAtlas> obj)
        {
            ++m_idx;
            
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                m_spriteAtlasDic[obj.Result.name] = obj.Result;
            }
            else
            {
                Debug.LogError($"[IconManager] 加载图标失败: {obj.OperationException}");
            }

            LoadSpriteAtlas(m_idx);
        }

        public Sprite TryGetCardIcon(int cardId)
        {
            string cardIconName = $"card_icon_{cardId}";

            var cardIconData = GlobalManager.AssetManager.TbIcon.GetOrDefault(cardIconName);
            if (cardIconData == null)
            {
                Debug.LogError($"[IconManager] 未找到卡牌图标: {cardIconName}");
                return null;
            }

            if (m_spriteAtlasDic.TryGetValue(cardIconData.Path, out var spriteAtlas))
            {
                return spriteAtlas.GetSprite(cardIconName);
            }
            
            Debug.LogError($"[IconManager] 未找到图标集: {cardIconData.Path}");

            return null;
        }
    }
}