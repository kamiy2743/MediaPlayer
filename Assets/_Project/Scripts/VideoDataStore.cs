using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaPlayer
{
    public class VideoDataStore : MonoBehaviour
    {
        public IReadOnlyList<Video> Videos => _videos;
        private List<Video> _videos = new List<Video>();

        public void Add(Video video)
        {
            _videos.Add(video);
        }

        public void SetChild(Video video)
        {
            video.transform.SetParent(this.transform);
        }
    }
}
