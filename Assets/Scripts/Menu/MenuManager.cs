using Scripts.Game.Data;
using UnityEngine;

namespace Scripts.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField]
        private Canvas _gameOptionCanvas;
        [SerializeField]
        private Canvas _settingCanvas;
        [SerializeField, Header("槍械資料")]
        private GunData[] _gunDatas;
        [SerializeField, Header("槍械選項預置物")]
        private GameObject _gunOptionPrefab;
        IGameOptionsUIController _gameOptionsUIController;

        private void Awake()
        {
            _gameOptionsUIController = new GameOptionsUIController(_gameOptionCanvas, _gunOptionPrefab, _gunDatas);

            if (StaticPrefs.IsFirstIn)
                FirstOpenGame();
        }

        private void Start()
        {
            ShowGameOptions();
        }

        public void ShowGameOptions()
        {
            _gameOptionsUIController.ShowCanvas();
        }

        private void FirstOpenGame()
        {
            Debug.Log("FirstOpenGame");
            StaticPrefs.IsFirstIn = false;
        }
    }
}