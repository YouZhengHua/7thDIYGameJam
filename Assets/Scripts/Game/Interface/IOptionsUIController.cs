using Scripts.Game.Data;

namespace Scripts.Game
{
    public interface IOptionsUIController
    {
        /// <summary>
        /// 關閉升級選項視窗
        /// </summary>
        public void HideCanvas();
        /// <summary>
        /// 顯示升級選項視窗
        /// </summary>
        public void ShowCanvas();
        /// <summary>
        /// 點擊升級選項
        /// </summary>
        /// <param name="data"></param>
        public void OptionOnClick(OptionData data);
        /// <summary>
        /// 顯示武器三選一
        /// </summary>
        public void ShowWeaponOptions();
    }
}
