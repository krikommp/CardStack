﻿using System.Collections.Generic;
using UnityEngine;

namespace GameMain.Scripts.AbilitySystem
{
    public class GameplayAbilitySystem : MonoBehaviour
    {
        private GameplayTagContainer m_tagContainer = new GameplayTagContainer();
        
        public void ApplyGameplayEffectToSelf(GameplayEffect effect)
        {
            ApplyGameplayEffectToTarget(effect, this);
        }
        
        public void ApplyGameplayEffectToTarget(GameplayEffect effect, GameplayAbilitySystem target)
        {
            if (!CheckCanApplyEffectToTarget(effect, target))
            {
                return;
            }
            
            // 需要根据延时，周期，触发条件等来处理
            ApplyEffectToTarget(effect, target);
        }
        
        private bool CheckCanApplyEffectToTarget(GameplayEffect effect, GameplayAbilitySystem target)
        {
            foreach (var blockTag in effect.m_blockTags)
            {
                if (m_tagContainer.IsMatchingTag(blockTag, EGameplayTagMatchType.Explicit))
                {
                    return false;
                }
            }

            return true;
        }
        
        private void ApplyEffectToTarget(GameplayEffect effect, GameplayAbilitySystem target)
        {
            foreach (var applyTag in effect.m_applyTags)
            {
                target.m_tagContainer.AddTag(applyTag);
            }
            
            foreach (var removeTag in effect.m_removeTags)
            {
                target.m_tagContainer.RemoveTag(removeTag);
            }
            
            effect.m_ability.ApplyGameplayAbility(target);
        }
    }
}