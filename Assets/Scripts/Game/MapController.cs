using System.Collections;
using System.Collections.Generic;
using Scripts.Game.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.Game
{
    /// <summary>
    /// 地圖控制器
    /// 初始化時會依照傳入的地圖資料生成地圖
    /// </summary>
    public class MapController : MonoBehaviour
    {
        /// <summary>
        /// 地圖資料
        /// </summary>
        [SerializeField, Header("地圖資料")]
        private MapData[] _datas;
        [SerializeField, Header("進度管理")]
        private StageManager _stageManager;
        private Transform _playerContainer;
        private MapData _data;
        private Transform _mapConainer;
        private IList<Transform> _maps = new List<Transform>();
        private Vector3 _postition = Vector3.zero;

        private void Awake()
        {
            _data = _datas[(int)_stageManager.GetCurrentStage() - 1];
            _mapConainer = GameObject.Find("MapContainer").GetComponent<Transform>();
            _playerContainer = GameObject.Find("PlayerContainer").GetComponent<Transform>();
            MapInstantiate();
        }

        private void Update()
        {
            foreach(Transform map in _maps)
            {
                _postition = map.position;
                if (map.position.x - _playerContainer.position.x > _data.mapWidth / 2f)
                {
                    _postition.x -= _data.mapWidth;
                }
                else if(map.position.x - _playerContainer.position.x < _data.mapWidth / 2f * -1)
                {
                    _postition.x += _data.mapWidth;
                }
                if(map.position.y - _playerContainer.position.y > _data.mapHeight / 2f)
                {
                    _postition.y -= _data.mapWidth;
                }
                else if(map.position.y - _playerContainer.position.y < _data.mapHeight / 2f * -1)
                {
                    _postition.y += _data.mapWidth;
                }
                map.position = _postition;
            }
        }

        private void MapInstantiate()
        {
            for (int x = 0; x < _data.mapWidth; x += _data.floorWidth)
            {
                for (int y = 0; y < _data.mapHeight; y += _data.floorHeight)
                {
                    GameObject map;
                    int floorIndex = Random.Range(0, _data.floorPrefabs.Length);
                    map = GameObject.Instantiate(_data.floorPrefabs[floorIndex], _mapConainer);
                    map.transform.localScale = new Vector3(_data.floorWidthScale, _data.floorHeightScale, 1);
                    map.transform.localPosition = new Vector3(_data.floorWidth * x - _data.mapWidth / 2, _data.floorHeight * y - _data.mapHeight / 2, 0);
                    _maps.Add(map.transform);
                }
            }
        }
    }
}