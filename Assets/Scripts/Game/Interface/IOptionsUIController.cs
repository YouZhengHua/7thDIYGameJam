using Scripts.Game.Data;

namespace Scripts.Game
{
    public interface IOptionsUIController
    {
        public void HideCanvas();
        public void ShowCanvas();
        public void OptionOnClick(OptionData data);
    }
}
