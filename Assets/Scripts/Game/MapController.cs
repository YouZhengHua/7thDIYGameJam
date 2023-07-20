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
    public class MapController : IMapController
    {
        /// <summary>
        /// 地圖資料
        /// </summary>
        private MapData _data;
        private Transform _mapConainer;

        public MapController(MapData data)
        {
            _data = data;
            _mapConainer = GameObject.Find("MapContainer").GetComponent<Transform>();
            MapInstantiate();
        }

        private void MapInstantiate()
        {
            for (int x = 0; x < _data.mapWidth; x += _data.floorWidth)
            {
                for (int y = 0; y < _data.mapHeight; y += _data.floorHeight)
                {
                    bool isLeft = x == 0;
                    bool isBot = y == 0;
                    bool isTop = x > 0 && y + _data.floorWidth >= _data.mapWidth;
                    bool isRight = y > 0 && x + _data.floorHeight >= _data.mapHeight;
                    GameObject map;
                    if (isTop && isLeft && (_data.HaveTopBorder || _data.HaveLeftBorder))
                        map = GameObject.Instantiate(_data.wallTopLeft, _mapConainer);
                    else if (isTop && isRight && (_data.HaveTopBorder || _data.HaveRightBorder))
                        map = GameObject.Instantiate(_data.wallTopRight, _mapConainer);
                    else if (isBot && isLeft && (_data.HaveBotBorder || _data.HaveLeftBorder))
                        map = GameObject.Instantiate(_data.wallBotLeft, _mapConainer);
                    else if (isBot && isRight && (_data.HaveBotBorder || _data.HaveRightBorder))
                        map = GameObject.Instantiate(_data.wallBotRight, _mapConainer);
                    else if (isTop && _data.HaveTopBorder)
                        map = GameObject.Instantiate(_data.wallTop, _mapConainer);
                    else if (isBot && _data.HaveBotBorder)
                        map = GameObject.Instantiate(_data.wallBot, _mapConainer);
                    else if (isLeft && _data.HaveLeftBorder)
                        map = GameObject.Instantiate(_data.wallLeft, _mapConainer);
                    else if (isRight && _data.HaveRightBorder)
                        map = GameObject.Instantiate(_data.wallRight, _mapConainer);
                    else
                    {
                        int floorIndex = Random.Range(0, _data.floorPrefabs.Length);
                        map = GameObject.Instantiate(_data.floorPrefabs[floorIndex], _mapConainer);
                    }
                    map.transform.localScale = new Vector3(_data.floorWidthScale, _data.floorHeightScale, 1);
                    map.transform.localPosition = new Vector3(_data.floorWidth * x - _data.mapWidth / 2, _data.floorHeight * y - _data.mapHeight / 2, 0);
                }
            }
        }
    }
}