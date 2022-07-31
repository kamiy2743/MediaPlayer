using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MediaPlayer
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public Slider.SliderEvent OnValueChanged => slider.onValueChanged;

        public void SetValue(float value)
        {
            slider.value = value;
        }
    }
}
