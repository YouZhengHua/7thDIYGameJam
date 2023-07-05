using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Map")]
	public class MapData : ScriptableObject
	{
        [Header("地板預置物陣列")]
        public GameObject[] floorPrefabs;

        [Header("上方牆壁預置物")]
        public GameObject wallTop;

        [Header("左上牆壁預置物")]
        public GameObject wallTopLeft;

        [Header("右上牆壁預置物")]
        public GameObject wallTopRight;

        [Header("左方牆壁預置物")]
        public GameObject wallLeft;

        [Header("右方牆壁預置物")]
        public GameObject wallRight;

        [Header("下方牆壁預置物")]
        public GameObject wallBot;

        [Header("左下牆壁預置物")]
        public GameObject wallBotLeft;

        [Header("右下牆壁預置物")]
        public GameObject wallBotRight;

        [Header("地圖總寬度")]
        public int mapWidth = 50;
        [Header("地圖總高度")]
        public int mapHeight = 50;
        [Header("單格地圖寬度")]
        public int floorWidth = 1;
        [Header("單格地圖寬度比例")]
        public float floorWidthScale = 1.1f;
        [Header("單格地圖高度")]
        public int floorHeight = 1;
        [Header("單格地圖高度比例")]
        public float floorHeightScale = 1.1f;
        [Header("是否擁有上方邊界")]
        public bool HaveTopBorder = true;
        [Header("是否擁有下方邊界")]
        public bool HaveBotBorder = true;
        [Header("是否擁有左方邊界")]
        public bool HaveLeftBorder = true;
        [Header("是否擁有右方邊界")]
        public bool HaveRightBorder = true;
    }
}