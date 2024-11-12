using GameMain.Scripts.AbilitySystem;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TestEditor
    {
        [MenuItem("Test/Print")]
        public static void Print()
        {
            GameplayTag tag = new GameplayTag("AAA.BBB.CCC");
            GameplayTag otherTag = new GameplayTag("AAA.CCC");
            
            Debug.LogError(tag.IsMatchingTag(otherTag, EGameplayTagMatchType.IncludeParent));
            Debug.LogError($"tag: {tag}");
            Debug.LogError($"otherTag: {otherTag}");
        }
    }
}