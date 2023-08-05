using UnityEngine;

namespace Scripts.Game
{
    public interface IEndUIController
    {
        public void ShowCanvas(bool isWin, Sprite background);
        public void HideCanvas();
    }
}