using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Map")]
	public class MapData : ScriptableObject
	{
        [Header("地板預置物陣列")]
        public GameObject[] floorPrefabs;
        [Header("地圖總寬度")]
        public int mapWidth = 50;
        [Header("地圖總高度")]
        public int mapHeight = 50;
        [Header("單格地圖寬度")]
        public int floorWidth = 1;
        [Header("單格地圖寬度比例")]
        public float floorWidthScale = 1f;
        [Header("單格地圖高度")]
        public int floorHeight = 1;
        [Header("單格地圖高度比例")]
        public float floorHeightScale = 1f;
    }
}