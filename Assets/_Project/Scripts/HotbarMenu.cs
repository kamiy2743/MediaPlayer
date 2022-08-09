using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaPlayer
{
    public class HotbarMenu : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform => transform as RectTransform;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            canvasGroup.alpha = rectTransform.IsUnderPointer() ? 1 : 0;
        }
    }
}
