using System;
using UnityEngine;

namespace Client.GameLogic.Util
{
    public static class UITool
    {
        public static Canvas GetRootCanvas(this RectTransform rectTransform)
        {
            var c = rectTransform.GetComponentsInParent<Canvas>();
            var topMostCanvas = c[^1];
            return topMostCanvas;
        }
    }
}