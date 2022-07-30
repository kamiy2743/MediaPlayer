using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class VideoRenderer : MonoBehaviour
    {
        public RawImage RawImage => _rawImage;
        private RawImage _rawImage;

        public Video Source => _source;
        private Video _source;

        public static VideoRenderer Create(Video video)
        {
            var rendererObject = new GameObject("renderer: " + video.Title);
            var renderer = rendererObject.AddComponent<VideoRenderer>();
            renderer._source = video;

            var rawImage = rendererObject.AddComponent<RawImage>();
            rawImage.texture = video.Texture;
            rawImage.raycastTarget = false;
            renderer._rawImage = rawImage;

            return renderer;
        }
    }
}
