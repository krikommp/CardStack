using GameMain.Scripts.AbilitySystem;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameplayEffect))]
    public class GameplayEffectEditor : UnityEditor.Editor
    {
        private readonly string[] predefinedValues = { "Value1", "Value2", "Value3", "Value4" }; // 预定义的值
        private int selectedOption = 0;

        public override void OnInspectorGUI()
        {
            GameplayEffect gameplayEffect = (GameplayEffect)target;

            selectedOption = EditorGUILayout.Popup("My Dropdown Menu", selectedOption, predefinedValues);

            serializedObject.ApplyModifiedProperties();
        }
    }
}