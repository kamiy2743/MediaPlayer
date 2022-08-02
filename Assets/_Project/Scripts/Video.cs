using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;

namespace MediaPlayer
{
    public class Video : MonoBehaviour
    {
        private VideoPlayer player;

        public RenderTexture Texture => _texture;
        private RenderTexture _texture;

        public string Title => _title;
        private string _title;

        public Vector2Int OriginalSize => _originalSize;
        private Vector2Int _originalSize;

        public float OriginalAcpectRatio => (float)_originalSize.x / (float)_originalSize.y;

        public float RelativeVolume => _relativeVolume;
        private float _relativeVolume = 1;

        public static async UniTask<Video> CreateFromURLAsync(string url)
        {
            var videoObject = new GameObject();
            var video = videoObject.AddComponent<Video>();
            video._title = System.IO.Path.GetFileName(url);
            videoObject.name = "video: " + video._title;

            var player = videoObject.AddComponent<VideoPlayer>();
            player.url = url;
            player.source = VideoSource.Url;
            player.playOnAwake = false;
            player.isLooping = true;
            player.renderMode = VideoRenderMode.RenderTexture;
            player.aspectRatio = VideoAspectRatio.NoScaling;

            await UniTask.WaitUntil(() => player.isPrepared);

            video._originalSize = new Vector2Int((int)player.width, (int)player.height);
            video._texture = new RenderTexture((int)player.width, (int)player.height, 16, RenderTextureFormat.ARGB32);

            player.targetTexture = video._texture;
            video.player = player;

            return video;
        }

        public void ApplyVolume(float masterVolume)
        {
            if (!player.canSetDirectAudioVolume) return;
            player.SetDirectAudioVolume(0, _relativeVolume * masterVolume);
        }

        public void SetRelativeVolume(float relativeVolume)
        {
            _relativeVolume = relativeVolume;
        }
    }
}
