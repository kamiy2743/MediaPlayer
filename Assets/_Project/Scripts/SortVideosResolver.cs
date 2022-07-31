using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class SortVideosResolver : MonoBehaviour
    {
        [SerializeField] private Button sortButton;

        private VideoRendererDataStore videoRendererDataStore;

        void Awake()
        {
            sortButton.onClick.AddListener(Sort);

            videoRendererDataStore = FindObjectOfType<VideoRendererDataStore>();
        }

        private void Sort()
        {

        }
    }
}
