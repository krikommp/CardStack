using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Scripts.AbilitySystem
{
    [Serializable]
    public class GameplayTagNode
    {
        public string m_tag;
        public GameplayTagNode m_parent;
        public List<GameplayTagNode> m_children = new List<GameplayTagNode>();
        public bool m_isExtend = false;
    }
    
    [CreateAssetMenu(fileName = "GameplayTagManager", menuName = "GAS/GameplayTagManager")]
    public class GameplayTagManager : ScriptableObject
    {
        [SerializeField] public List<GameplayTagNode> m_tagNodes = new List<GameplayTagNode>();

        public GameplayTag GetGameplayTag(GameplayTagNode node)
        {
            // 调用递归方法，构建路径
            string path = GetNodePath(node, string.Empty);
            
            // 返回新的 GameplayTag，带有路径
            return new GameplayTag(path);
        }

        public void AddTag(string tagName)
        {
            string[] tagParts = tagName.Split('.');
            GameplayTagNode parentNode = null;
            for (int i = 0; i < tagParts.Length; i++)
            {
                string tagPart = tagParts[i];
                GameplayTagNode node = null;
                if (parentNode == null)
                {
                    node = m_tagNodes.Find(n => n.m_tag == tagPart);
                }
                else
                {
                    node = parentNode.m_children.Find(n => n.m_tag == tagPart);
                }

                if (node == null)
                {
                    node = new GameplayTagNode()
                    {
                        m_tag = tagPart,
                        m_parent = parentNode
                    };
                    if (parentNode == null)
                    {
                        m_tagNodes.Add(node);
                    }
                    else
                    {
                        parentNode.m_children.Add(node);
                    }
                }

                parentNode = node;
            }
        }
        
        public void DeleteTag(GameplayTagNode node)
        {
            // 递归遍历 m_tagNodes 找到 node 并删除
            foreach (var tagNode in m_tagNodes)
            {
                if (tagNode == node)
                {
                    m_tagNodes.Remove(tagNode);
                    return;
                }

                DeleteTagNode(tagNode, node);
            }
        }
        
        private void DeleteTagNode(GameplayTagNode parentNode, GameplayTagNode node)
        {
            foreach (var tagNode in parentNode.m_children)
            {
                if (tagNode == node)
                {
                    parentNode.m_children.Remove(tagNode);
                    return;
                }

                DeleteTagNode(tagNode, node);
            }
        }
        
        // 递归查找路径
        private string GetNodePath(GameplayTagNode node, string currentPath)
        {
            if (currentPath == string.Empty)
            {
                currentPath = node.m_tag;
            }
            else
            {
                currentPath = $"{node.m_tag}.{currentPath}";
            }
            
            if (node.m_parent != null)
            {
                return GetNodePath(node.m_parent, currentPath);
            }
            return currentPath;
        }
    }
}