using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;

namespace MediaPlayer
{
    public class Video : MonoBehaviour
    {
        public VideoPlayer Player => _player;
        private VideoPlayer _player;

        public RenderTexture Texture => _texture;
        private RenderTexture _texture;

        public string Title => _title;
        private string _title;

        public Vector2Int OriginalSize => _originalSize;
        private Vector2Int _originalSize;

        public float OriginalAcpectRatio => _originalSize.x / _originalSize.y;

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
            video._player = player;

            return video;
        }
    }
}
