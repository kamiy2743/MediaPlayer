using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaPlayer
{
    public class RightClickMenu : MonoBehaviour
    {
        private RectTransform rectTransform => transform as RectTransform;

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        public void SetToMousePosition()
        {
            rectTransform.anchoredPosition = Input.mousePosition;
        }
    }
}
