using System.Text.RegularExpressions;

namespace GameMain.Scripts.AbilitySystem
{
    public enum EGameplayTagMatchType
    {
        /// <summary>
        /// 完全匹配
        /// </summary>
        Explicit,
        
        /// <summary>
        /// 部分匹配
        /// </summary>
        IncludeParent
    }
    
    [System.Serializable]
    public class GameplayTag
    {
        public string m_tagName;
        public GameplayTag m_childTag;

        public GameplayTag(string tagName)
        {
            // 如果 m_tagName 不包含 . 分割， 那么就赋值给 m_tagName 并返回
            if (!tagName.Contains("."))
            {
                m_tagName = tagName;
                return;
            }
            
            // 如果 m_tagName 包含 . 分割，那么就分割成两部分，只分割第一个 . 前面的赋值给 m_tagName， 后面的赋值给 m_childTag
            // 例如： tagName = "A.B.C"， 那么 m_tagName = "A", m_childTag = new GameplayTag("B.C")
            string[] tagParts = tagName.Split('.');
            m_tagName = tagParts[0];
            m_childTag = new GameplayTag(tagName.Substring(m_tagName.Length + 1));
        }
        
        /// <summary>
        /// 用于判断两个 GameplayTag 是否匹配
        /// 有两种匹配模式
        /// 完全匹配：需要两个 GameplayTag 完全相同，也就是本身包括 childTag 也都是完全一样的, 比如 A.B.C 和 A.B.C
        /// 部分匹配：判断本身是否含有 <paramref name="otherTag"/>， 比如 A.B.C 包含 A.B，但是不包含 A.B.D
        /// </summary>
        /// <param name="otherTag"></param>
        /// <param name="matchType"></param>
        /// <returns></returns>
        public bool IsMatchingTag(GameplayTag otherTag, EGameplayTagMatchType matchType)
        {
            if (matchType == EGameplayTagMatchType.Explicit)
            {
                // 完全匹配：需要两个 GameplayTag 完全相同，包括所有子标签都一样
                if (m_tagName != otherTag.m_tagName)
                {
                    return false;
                }

                // 检查 childTag 的递归匹配
                if (m_childTag == null && otherTag.m_childTag == null)
                {
                    return true;
                }
        
                if (m_childTag != null && otherTag.m_childTag != null)
                {
                    return m_childTag.IsMatchingTag(otherTag.m_childTag, EGameplayTagMatchType.Explicit);
                }

                return false;
            }

            if (matchType == EGameplayTagMatchType.IncludeParent)
            {
                // 部分匹配：otherTag 是 this 的前缀（不必完全相等）
                if (m_tagName == otherTag.m_tagName)
                {
                    if (otherTag.m_childTag == null)
                    {
                        return true; // otherTag 是一个完全匹配的前缀
                    }

                    if (m_childTag != null)
                    {
                        return m_childTag.IsMatchingTag(otherTag.m_childTag, EGameplayTagMatchType.IncludeParent);
                    }
                }

                return false; // m_tagName 不匹配或 otherTag 不是当前标签的前缀
            }

            return false;
        }

        public bool IsMatchingTag(string otherTag, EGameplayTagMatchType matchType)
        {
            return IsMatchingTag(new GameplayTag(otherTag), matchType);
        }

        public override string ToString()
        {
            // 递归拼接 m_childTag，直到 m_childTag 为 null
            // 每取出一个 m_tagName 后都加上 . 作为分割符号
            // 直到 m_childTag 为 null，返回 m_tagName
            return m_childTag == null ? m_tagName : $"{m_tagName}.{m_childTag}";
        }
    }
}