using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using Cysharp.Threading.Tasks;

namespace MediaPlayer
{
    public class VideoPlaySystem : MonoBehaviour
    {
        [SerializeField] private Button openButton;

        private VideoDataStore videoDataStore;
        private VideoRendererDataStore videoRendererDataStore;

        void Awake()
        {
            videoDataStore = FindObjectOfType<VideoDataStore>();
            videoRendererDataStore = FindObjectOfType<VideoRendererDataStore>();

            openButton.onClick.AddListener(OnOpenButtonClick);
        }

        private void OnOpenButtonClick()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("動画を選択", "", "", true);
            foreach (var path in paths)
            {
                AddVideoAsync(path).Forget();
            }
        }

        private async UniTask AddVideoAsync(string url)
        {
            var video = await Video.CreateFromURLAsync(url);
            videoDataStore.Add(video);
            videoDataStore.SetChild(video);

            var renderer = VideoRenderer.Create(video);
            videoRendererDataStore.Add(renderer);
        }
    }
}
