using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInfoController : MonoBehaviour
{
    private Canvas _canvas;
    private TextMeshProUGUI _version;
    private TextMeshProUGUI _fps;
    private bool _isShow = false;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        _canvas = this.GetComponent<Canvas>();
        _canvas.enabled = _isShow;

        foreach(TextMeshProUGUI text in this.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.name == "Version")
                _version = text;
            else if (text.name == "Fps")
                _fps = text;
        }
        _version.text = "Version: " + Application.version;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            _isShow = !_isShow;
            _canvas.enabled = _isShow;
        }
        if (_canvas.enabled)
            Reflash();
    }

    private void Reflash()
    {
        int fps = Mathf.RoundToInt(1f / Time.deltaTime);
        _fps.text = "FPS: " + (Time.timeScale == 0 ? "Stop" : fps);
    }
}
