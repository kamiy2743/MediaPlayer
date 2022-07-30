using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace MediaPlayer
{
    public class VideoRenderer : MonoBehaviour, IPointerClickHandler
    {
        public RawImage RawImage => _rawImage;
        private RawImage _rawImage;

        public Video Source => _source;
        private Video _source;

        public UnityEvent<VideoRenderer> OnClick => _onClick;
        private UnityEvent<VideoRenderer> _onClick = new UnityEvent<VideoRenderer>();

        public static VideoRenderer Create(Video video)
        {
            var rendererObject = new GameObject("renderer: " + video.Title);
            var renderer = rendererObject.AddComponent<VideoRenderer>();
            renderer._source = video;

            var rawImage = rendererObject.AddComponent<RawImage>();
            rawImage.texture = video.Texture;
            renderer._rawImage = rawImage;

            return renderer;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick.Invoke(this);
        }
    }
}
