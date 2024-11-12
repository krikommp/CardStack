using System;
using System.Collections.Generic;
using GameMain.Scripts.AbilitySystem;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameplayTagManager))]
    public class GameplayTagManagerEditor : UnityEditor.Editor
    {
        private GameplayTagManager m_manager;

        private void OnEnable()
        {
            m_manager = target as GameplayTagManager;
        }

        private string name = "";
        private string comment = "";
        private string source = "Not selected";
        private bool showFields = false; // 控制是否显示输入框的标志

        public override void OnInspectorGUI()
        {
            // 绘制搜索框和 + 按钮
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search Gameplay Tags", GUILayout.Width(150));
            GUILayout.TextField(""); // 可以根据需要在这里增加功能

            // + 按钮
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                showFields = !showFields; // 点击按钮切换显示状态
            }

            EditorGUILayout.EndHorizontal();

            // 如果 showFields 为 true，显示其他输入框
            if (showFields)
            {
                // 绘制名称输入框
                name = EditorGUILayout.TextField("Name", name);

                // 绘制备注输入框
                comment = EditorGUILayout.TextField("Comment", comment);

                // 绘制添加新标签按钮
                if (GUILayout.Button("Add New Tag"))
                {
                    m_manager.AddTag(name);
                }
            }

            foreach (var tagNode in m_manager.m_tagNodes)
            {
                DisplayGameplayTagNode(tagNode);
            }
        }

        private void DisplayGameplayTagNode(GameplayTagNode node, int indentLevel = 0)
        {
            float indentWidth = indentLevel * 10f;
            Rect rect = EditorGUILayout.GetControlRect();
            rect.x += indentWidth;
            
            GUIStyle style = new GUIStyle(GUI.skin.box);  
            style.alignment = TextAnchor.MiddleLeft;
            style.padding.left = 10;
            
            // 如果没有子项，则直接显示标签
            if (node.m_children == null || node.m_children.Count == 0)
            {
                if (GUI.Button(rect, node.m_tag.m_tagName, style))
                {
                    ShowRightClickMenu(node);
                }
                return;
            }
            
            node.m_isExtend = EditorGUI.Foldout(rect, node.m_isExtend, string.Empty);
            // 创建一个新的Rect来绘制按钮，按钮在Foldout后面
            // 创建按钮
            if (GUI.Button(rect, node.m_tag.m_tagName, style))
            {
                ShowRightClickMenu(node);
            }

            // 如果该项展开，显示它的子项
            if (node.m_isExtend)
            {
                foreach (var childNode in node.m_children)
                {
                    // 递归调用时传递更高的缩进层级
                    DisplayGameplayTagNode(childNode, indentLevel + 1);
                }
            }
        }
        
        private void ShowRightClickMenu(GameplayTagNode node)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent($"Delete Node"), false, () => OnDeleteNodeClicked(node));
            menu.ShowAsContext();
        }
        
        private void OnDeleteNodeClicked(GameplayTagNode node)
        {
            m_manager.DeleteTag(node);
        }
    }
}