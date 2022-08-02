using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class ResizableUI : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Camera mainCamera;
        private const int RESIZABLE_EDGE_WIDTH = 50;

        private System.IDisposable mouseOverUpdate;
        private System.IDisposable resizeUpdate;

        void Awake()
        {
            rectTransform = gameObject.GetOrAddComponent<RectTransform>();
            mainCamera = Camera.main;

            var eventTrigger = gameObject.GetOrAddComponent<EventTrigger>();
            eventTrigger.AddListener(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTrigger.AddListener(EventTriggerType.PointerExit, OnPointerExit);
            eventTrigger.AddListener(EventTriggerType.BeginDrag, OnBeginDrag);
        }

        private void OnPointerEnter()
        {
            mouseOverUpdate = Observable.EveryUpdate()
                .Subscribe(_ => OnMouseOver());
        }

        private void OnMouseOver()
        {
            if (!InResizableEdge()) return;
        }

        private void OnPointerExit()
        {
            mouseOverUpdate.Dispose();
        }

        private void OnBeginDrag()
        {
            if (!Input.GetMouseButton(0)) return;
            if (!InResizableEdge()) return;

            var localCursorPos = GetLocalCursorPosition();
            var pivoitX = localCursorPos.x > 0 ? 0 : 1;
            var pivoitY = localCursorPos.y > 0 ? 0 : 1;
            rectTransform.SetPivot(new Vector2(pivoitX, pivoitY));

            var startSize = rectTransform.sizeDelta;
            var startPointerPos = Input.mousePosition;
            resizeUpdate = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        OnEndResize();
                        return;
                    }

                    OnResize(startSize, startPointerPos);
                });
        }

        private void OnResize(Vector2 startSize, Vector2 startPointerPos)
        {
            var delta = (Vector2)Input.mousePosition - startPointerPos;
            var signX = (rectTransform.pivot.x < 0.5f) ? 1 : -1;
            var signY = (rectTransform.pivot.y < 0.5f) ? 1 : -1;
            rectTransform.sizeDelta = startSize + new Vector2(delta.x * signX, delta.y * signY);
        }

        private void OnEndResize()
        {
            resizeUpdate.Dispose();
            rectTransform.SetPivot(Vector2.one * 0.5f);
        }

        private Vector2 GetLocalCursorPosition()
        {
            var lastPivot = rectTransform.pivot;
            rectTransform.SetPivot(Vector2.zero);

            var screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, rectTransform.position) - rectTransform.CalcDelta();
            var localCursorPos = (Vector2)Input.mousePosition - screenPos;

            rectTransform.SetPivot(lastPivot);
            return localCursorPos;
        }

        public bool InResizableEdge()
        {
            var halfSize = rectTransform.sizeDelta * 0.5f;
            var localCursorPos = GetLocalCursorPosition();

            var top = halfSize.y - localCursorPos.y < RESIZABLE_EDGE_WIDTH;
            var bottom = halfSize.y + localCursorPos.y < RESIZABLE_EDGE_WIDTH;
            var left = halfSize.x + localCursorPos.x < RESIZABLE_EDGE_WIDTH;
            var right = halfSize.x - localCursorPos.x < RESIZABLE_EDGE_WIDTH;
            return (top || bottom || left || right);
        }
    }
}
