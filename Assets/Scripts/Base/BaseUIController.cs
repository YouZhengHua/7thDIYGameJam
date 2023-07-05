using UnityEngine;

namespace Scripts.Base
{
    public class BaseUIController : IBaseUIController
    {
        protected Canvas _canvas;

        public BaseUIController(Canvas canvas)
        {
            _canvas = canvas;
        }

        public virtual void ShowCanvas()
        {
            _canvas.gameObject.SetActive(true);
        }

        public virtual void HideCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}