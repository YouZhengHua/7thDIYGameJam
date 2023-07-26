using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

namespace Scripts
{
    public class LoadingScreen : MonoBehaviour
    {
        private static readonly object padlock = new object();
        private static LoadingScreen _instance;
        public static LoadingScreen instance 
        {
            get 
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameObject("LoadingScreen").AddComponent<LoadingScreen>();
                    }
                    return _instance;
                }
            }
        } // 當前載入畫面實例

        private GameObject _canvasPrefab;
        private Canvas _canvas;

        private Image _loadingImage; // 載入圖片
        private Image _tipBackground; // 載入圖片
        private TextMeshProUGUI _loadText;
        private TextMeshProUGUI _tipText;
        private TextMeshProUGUI _readyText;
        private bool _autoContinue = false;
        private AsyncOperation _operation;
        private LoadState _state = LoadState.Idle;

        private void Awake()
        {
            if (_canvasPrefab == null)
            {
                _canvasPrefab = Resources.Load<GameObject>("Prefab/LoadingCanvas");
            }
            if (_canvas == null)
            {
                _canvas = GameObject.Instantiate(_canvasPrefab).GetComponent<Canvas>();
            }
            DontDestroyOnLoad(_canvas);
            DontDestroyOnLoad(this); // 保證載入畫面在場景切換時不被銷毀
            SetVolume(0f);

            foreach(Image image in _canvas.GetComponentsInChildren<Image>())
            {
                if (image.name == "LoadingImage")
                    _loadingImage = image;
                else if (image.name == "TipBackground")
                    _tipBackground = image;
            }
            foreach (TextMeshProUGUI text in _canvas.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (text.name == "LoadText")
                    _loadText = text;
                else if (text.name == "ReadyText")
                    _readyText = text;
                else if (text.name == "TipText")
                    _tipText = text;
            }

            _canvas.enabled = false;
        }

        public void LoadScene(string sceneName, Sprite loadingImage, string tipText, bool needAutoContinue = false)
        {
            _autoContinue = needAutoContinue;
            _loadingImage.sprite = loadingImage;
            _tipText.text = tipText.Replace("\\n", "\n");
            LoadSceneAsync(sceneName);
        }

        private void LoadSceneAsync(string sceneName)
        {
            _state = LoadState.Load;
            _canvas.enabled = true;
            _loadText.enabled = true;
            _readyText.enabled = false;
            SetVolume(-80f);
            // 載入資源
            _operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            _operation.allowSceneActivation = false;
        }

        private void Update()
        {
            if (_state == LoadState.Load)
            {
                if (_operation.progress >= 0.9f)
                {
                    _loadText.enabled = false;
                    _readyText.enabled = true;

                    if (Input.anyKeyDown || _autoContinue)
                    {
                        _operation.allowSceneActivation = true;
                        _state = LoadState.Loaded;
                    }
                }
                else
                {
                    //Debug.Log(_operation.progress / 0.9f);
                }
            }

            if(_state == LoadState.Loaded)
            {
                if (_operation.isDone)
                {
                    _canvas.enabled = false;
                    _state = LoadState.Idle;
                    SetVolume(0f);
                }
            }
        }

        private void SetVolume(float value)
        {
        }

        private enum LoadState
        {
            Idle,
            Load,
            Loaded,
        }
    }
}