using System.Collections.Generic;

namespace GameMain.Scripts.AbilitySystem
{
    public class GameplayTagContainer
    {
        public List<GameplayTag> m_tags = new List<GameplayTag>();
        
        public void AddTag(GameplayTag tag)
        {
            m_tags.Add(tag);
        }
        
        public void RemoveTag(GameplayTag tag)
        {
            m_tags.Remove(tag);
        }
        
        public bool IsMatchingTag(GameplayTag otherTag, EGameplayTagMatchType matchType)
        {
            foreach (var tag in m_tags)
            {
                if (tag.IsMatchingTag(otherTag, matchType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}