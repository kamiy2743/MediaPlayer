using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

namespace MediaPlayer
{
    public class VideoPlaySystem : MonoBehaviour
    {
        [SerializeField] private Button openButton;
        [SerializeField] private VolumeSlider selectedVideoVolumeSlider;
        [SerializeField] private VolumeSlider masterVolumeSlider;

        private RightClickMenu rightClickMenu;
        private VideoDataStore videoDataStore;
        private VideoRendererDataStore videoRendererDataStore;

        private VideoRenderer selectedRenderer;

        private float masterVolume = 1;

        void Awake()
        {
            rightClickMenu = FindObjectOfType<RightClickMenu>();
            videoDataStore = FindObjectOfType<VideoDataStore>();
            videoRendererDataStore = FindObjectOfType<VideoRendererDataStore>();

            rightClickMenu.SetVisible(false);
            openButton.onClick.AddListener(OnOpenButtonClick);
            selectedVideoVolumeSlider.OnValueChanged.AddListener(OnSelectedVideoSliderChanged);
            masterVolumeSlider.OnValueChanged.AddListener(OnMasterVolumeSliderChanged);
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

            renderer.OnClick.AddListener(OnVideoRendererClick);
        }

        private void OnVideoRendererClick(VideoRenderer renderer)
        {
            if (!Input.GetMouseButtonUp(1))
            {
                selectedRenderer?.SetHighlight(false);
                selectedRenderer = null;
                rightClickMenu.SetVisible(false);
                return;
            }

            selectedRenderer = renderer;
            selectedRenderer.SetHighlight(true);
            rightClickMenu.SetVisible(true);
            rightClickMenu.SetToMousePosition();
            selectedVideoVolumeSlider.SetValue(renderer.Source.RelativeVolume);
        }

        private void OnSelectedVideoSliderChanged(float value)
        {
            var video = selectedRenderer.Source;
            video.SetRelativeVolume(value);
            video.ApplyVolume(masterVolume);
        }

        private void OnMasterVolumeSliderChanged(float value)
        {
            masterVolume = value;

            foreach (var video in videoDataStore.Videos)
            {
                video.ApplyVolume(masterVolume);
            }
        }
    }
}
