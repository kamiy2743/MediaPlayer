using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaPlayer
{
    public class VideoRendererDataStore : MonoBehaviour
    {
        public IReadOnlyList<VideoRenderer> Renderers => _renderers;
        private List<VideoRenderer> _renderers = new List<VideoRenderer>();

        public void Add(VideoRenderer renderer)
        {
            _renderers.Add(renderer);
            SetChild(renderer);
        }

        private void SetChild(VideoRenderer renderer)
        {
            var rt = renderer.transform as RectTransform;
            rt.SetParent(this.transform);
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            rt.sizeDelta = renderer.Source.OriginalSize;
        }
    }
}
