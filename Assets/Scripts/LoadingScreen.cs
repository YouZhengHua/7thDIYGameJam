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
        public static LoadingScreen instance; // 當前載入畫面實例
        public Image loadingImage; // 載入圖片
        public TextMeshProUGUI loadText;
        public TextMeshProUGUI readyText;
        public TextMeshProUGUI tipText;
        public AudioMixer audioMixer;
        public Canvas canvas;
        private bool _needDestory = false;
        private AsyncOperation _operation;
        private LoadState _state = LoadState.Idle;

        private void Awake()
        {
            DontDestroyOnLoad(canvas.gameObject);
            DontDestroyOnLoad(gameObject); // 保證載入畫面在場景切換時不被銷毀
            if (instance == null)
            {
                instance = this;
            }
            SetVolume(0f);

            canvas.enabled = false;
        }

        public void LoadScene(string sceneName, bool needDestory)
        {
            LoadSceneAsync(sceneName);
            _needDestory = needDestory;
        }

        private void LoadSceneAsync(string sceneName)
        {
            _state = LoadState.Load;
            canvas.enabled = true;
            loadText.enabled = true;
            readyText.enabled = false;
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
                    loadText.enabled = false;
                    readyText.enabled = true;

                    if (Input.anyKeyDown)
                    {
                        if(_needDestory)
                            instance = null;
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
                    canvas.enabled = false;
                    _state = LoadState.Idle;
                    SetVolume(0f);
                    // 銷毀載入畫面
                    if (_needDestory)
                    {
                        Destroy(canvas.gameObject);
                        Destroy(gameObject);
                    }
                }
            }
        }

        private void SetVolume(float value)
        {
            audioMixer.SetFloat("Master", value);
        }

        private enum LoadState
        {
            Idle,
            Load,
            Loaded,
        }
    }
}