using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace MediaPlayer
{
    public class VideoRenderer : MonoBehaviour
    {
        public Video Source => _source;
        private Video _source;

        public UnityEvent<VideoRenderer> OnClick => _onClick;
        private UnityEvent<VideoRenderer> _onClick = new UnityEvent<VideoRenderer>();

        private RectTransform rectTransform;
        private Vector2 onDragRelativePosition;

        private static readonly Color HIGHLIGHT_COLOR = new Color(1, 0.92f, 0.016f, 0.4f);
        private GameObject heightlight;

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

            return renderer;
        }

        void Awake()
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.AddListener(EventTriggerType.PointerDown, OnPointerDown);
            eventTrigger.AddListener(EventTriggerType.PointerClick, () => { _onClick.Invoke(this); });
            eventTrigger.AddListener(EventTriggerType.BeginDrag, (e) => OnBeginDrag(e));
            eventTrigger.AddListener(EventTriggerType.Drag, (e) => OnDrag(e));
        }

        private void OnPointerDown()
        {
            transform.SetAsLastSibling();
        }

        private void OnBeginDrag(PointerEventData e)
        {
            onDragRelativePosition = rectTransform.anchoredPosition - e.position;
        }

        private void OnDrag(PointerEventData e)
        {
            rectTransform.anchoredPosition = e.position + onDragRelativePosition;
        }

        public void SetHighlight(bool flag)
        {
            heightlight.SetActive(flag);
        }
    }
}
