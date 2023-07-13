namespace Scripts.Game
{
    public interface IMoveController
    {
        /// <summary>
        /// 設定屬性處理器
        /// </summary>
        public IAttributeHandle SetAttributeHandle { set; }
    }
}