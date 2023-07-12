namespace Scripts.Menu
{
    public interface IMenuManager
    {
        /// <summary>
        /// 載入指定的 Scene
        /// </summary>
        /// <param name="sceneName">場景名稱</param>
        public void LoadScene(string sceneName);
    }
}
