using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Scripts.AbilitySystem
{
    [Serializable]
    public class GameplayTagNode
    {
        public GameplayTag m_tag;
        public List<GameplayTagNode> m_children = new List<GameplayTagNode>();
        public bool m_isExtend = false;
    }
    
    [CreateAssetMenu(fileName = "GameplayTagManager", menuName = "GAS/GameplayTagManager")]
    public class GameplayTagManager : ScriptableObject
    {
        public List<GameplayTagNode> m_tagNodes = new List<GameplayTagNode>();

        public void AddTag(string tagName)
        {
            // 构建树形结构, tagName = "A.B.C.D"
            // 1. 遍历 m_tagNodes，找到 A
            // 2. 遍历 A 的 m_children，找到 B
            // 3. 遍历 B 的 m_children，找到 C
            // 4. 遍历 C 的 m_children，找到 D
            
            string[] tagParts = tagName.Split('.');
            GameplayTagNode parentNode = null;
            for (int i = 0; i < tagParts.Length; i++)
            {
                string tagPart = tagParts[i];
                GameplayTagNode node = null;
                if (parentNode == null)
                {
                    node = m_tagNodes.Find(n => n.m_tag.m_tagName == tagPart);
                }
                else
                {
                    node = parentNode.m_children.Find(n => n.m_tag.m_tagName == tagPart);
                }

                if (node == null)
                {
                    node = new GameplayTagNode()
                    {
                        m_tag = new GameplayTag(tagPart)
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
    }
}