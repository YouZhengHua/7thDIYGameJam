using Scripts.Game.Data;
using UnityEngine;

namespace Scripts.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        private void Awake()
        {
            if (StaticPrefs.IsFirstIn)
                FirstOpenGame();
        }

        private void Start()
        {
        }

        private void FirstOpenGame()
        {
            Debug.Log("FirstOpenGame");
            StaticPrefs.IsFirstIn = false;
        }

        /// <summary>
        /// 載入指定的 Scene
        /// </summary>
        /// <param name="sceneName">場景名稱</param>
        public void LoadScene(string sceneName)
        {
            LoadingScreen.instance.LoadScene(sceneName, false);
        }
    }
}