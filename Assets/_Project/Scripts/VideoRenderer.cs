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

        public static VideoRenderer Create(Video video)
        {
            var rendererObject = new GameObject("renderer: " + video.Title);
            var renderer = rendererObject.AddComponent<VideoRenderer>();
            renderer._source = video;
            renderer.rectTransform = rendererObject.AddComponent<RectTransform>();

            var rawImage = rendererObject.AddComponent<RawImage>();
            rawImage.texture = video.Texture;

            return renderer;
        }

        void Awake()
        {
            var eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.AddListener(EventTriggerType.PointerClick, () => { _onClick.Invoke(this); });
            eventTrigger.AddListener(EventTriggerType.BeginDrag, (e) => OnBeginDrag(e));
            eventTrigger.AddListener(EventTriggerType.Drag, (e) => OnDrag(e));
        }

        private void OnBeginDrag(PointerEventData e)
        {
            onDragRelativePosition = rectTransform.anchoredPosition - e.position;
        }

        private void OnDrag(PointerEventData e)
        {
            rectTransform.anchoredPosition = e.position + onDragRelativePosition;
        }
    }
}
