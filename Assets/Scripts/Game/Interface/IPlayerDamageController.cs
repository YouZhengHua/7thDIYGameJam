namespace Scripts.Game
{
    public interface IPlayerDamageController
    {
        /// <summary>
        /// 設定結算UI控制器
        /// </summary>
        public IEndUIController SetEndUI { set; }
    }
}