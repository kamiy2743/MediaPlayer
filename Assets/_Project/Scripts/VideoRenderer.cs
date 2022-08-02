using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UniRx;

namespace MediaPlayer
{
    public class VideoRenderer : MonoBehaviour
    {
        public Video Source => _source;
        private Video _source;

        public UnityEvent<VideoRenderer> OnClick => _onClick;
        private UnityEvent<VideoRenderer> _onClick = new UnityEvent<VideoRenderer>();

        private RectTransform rectTransform;

        private static readonly Color HIGHLIGHT_COLOR = new Color(1, 0.92f, 0.016f, 0.4f);
        private GameObject heightlight;

        private ResizableUI resizableUI;

        private System.IDisposable moveUpdateDisposal;

        public static VideoRenderer Create(Video video)
        {
            var rendererObject = new GameObject("renderer: " + video.Title);
            var renderer = rendererObject.AddComponent<VideoRenderer>();
            renderer._source = video;
            renderer.rectTransform = rendererObject.AddComponent<RectTransform>();

            var rawImage = rendererObject.AddComponent<RawImage>();
            rawImage.texture = video.Texture;

            var highlightRect = new GameObject("highlight").AddComponent<RectTransform>();
            highlightRect.SetParent(renderer.transform);
            highlightRect.anchorMin = Vector2.zero;
            highlightRect.anchorMax = Vector2.one;
            highlightRect.sizeDelta = Vector2.zero;
            var image = highlightRect.gameObject.AddComponent<Image>();
            image.color = HIGHLIGHT_COLOR;
            image.raycastTarget = false;
            renderer.heightlight = highlightRect.gameObject;
            renderer.SetHighlight(false);

            var fitter = rendererObject.AddComponent<AspectRatioFitter>();
            fitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
            fitter.aspectRatio = video.OriginalAcpectRatio;

            renderer.resizableUI = rendererObject.AddComponent<ResizableUI>();

            return renderer;
        }

        void Awake()
        {
            var eventTrigger = gameObject.GetOrAddComponent<EventTrigger>();
            eventTrigger.AddListener(EventTriggerType.PointerDown, OnPointerDown);
            eventTrigger.AddListener(EventTriggerType.PointerUp, OnEndMove);
            eventTrigger.AddListener(EventTriggerType.BeginDrag, OnBeginDrag);
        }

        private void OnPointerDown()
        {
            transform.SetAsLastSibling();
            _onClick.Invoke(this);
        }

        private void OnBeginDrag()
        {
            if (resizableUI.InResizableEdge()) return;

            var startRendererPos = rectTransform.anchoredPosition;
            var startPointerPos = Input.mousePosition;
            moveUpdateDisposal = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        OnEndMove();
                        return;
                    }

                    OnMove(startRendererPos, startPointerPos);
                });
        }

        private void OnMove(Vector2 startRendererPos, Vector2 startPointerPos)
        {
            var delta = (Vector2)Input.mousePosition - startPointerPos;
            rectTransform.anchoredPosition = startRendererPos + delta;
        }

        private void OnEndMove()
        {
            moveUpdateDisposal.Dispose();
        }

        public void SetHighlight(bool flag)
        {
            heightlight.SetActive(flag);
        }
    }
}
