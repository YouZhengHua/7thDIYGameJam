using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Base
{
    public class SliderObject
    {
        private GameObject _gameObject;
        private Slider _slider;
        public SliderObject(GameObject gameObject)
        {
            _gameObject = gameObject;
            _slider = gameObject.GetComponentInChildren<Slider>();
        }

        public void SetListiner(Action action)
        {
            _slider.onValueChanged.AddListener(delegate { action(); });
        }

        public float Value { get => _slider.value; set => _slider.value = value; }
    }
}