using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace MediaPlayer
{
    public class SortVideosResolver : MonoBehaviour
    {
        [SerializeField] private Button sortButton;
        [SerializeField] private Transform renderersParent;

        void Awake()
        {
            sortButton.onClick.AddListener(() => Sort().Forget());
        }

        private async UniTask Sort()
        {
            var rendererCount = renderersParent.childCount;
            var square = Mathf.Ceil(Mathf.Sqrt(rendererCount));
            var gridLayout = renderersParent.gameObject.GetOrAddComponent<GridLayoutGroup>();
            gridLayout.enabled = true;
            gridLayout.cellSize = new Vector2(Screen.width, Screen.height) / square;

            await UniTask.WaitForEndOfFrame();
            gridLayout.enabled = false;
        }
    }
}
