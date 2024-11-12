using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Scripts.AbilitySystem
{
    [CreateAssetMenu(fileName = "GameplayEffect", menuName = "GAS/GameplayEffect")]
    public class GameplayEffect : ScriptableObject
    {
        public List<string> test = new List<string>() { "Value1" };
        
        public List<GameplayTag> m_applyTags;
        public List<GameplayTag> m_removeTags;
        public List<GameplayTag> m_blockTags;
        
        public GameplayAbility m_ability;
    }
}