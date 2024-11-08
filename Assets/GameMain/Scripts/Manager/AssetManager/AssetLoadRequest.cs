using System;

namespace Client.GameLogic.Manager.AssetManager
{
    internal class AssetLoadRequest
    {
        public string m_assetName = null;
        public Action<Object> m_loadAction = null;
        public bool m_bFinish = false;
    }
}