using System.Collections.Generic;

namespace GameMain.Scripts.AbilitySystem
{
    public class GameplayEffect
    {
        public List<GameplayTag> m_applyTags;
        public List<GameplayTag> m_removeTags;
        public List<GameplayTag> m_blockTags;
        
        public GameplayAbility m_ability;
    }
}