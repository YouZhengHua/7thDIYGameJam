using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Skill")]
	public class SkillData : BaseItemData
	{
        /// <summary>
        /// 技能類別
        /// </summary>
        public SkillType SkillType;

        /// <summary>
        /// 技能敘述
        /// </summary>
        public string Description;

        public int Level = 0;
        public int MaxLevel = 5;

        /// <summary>
        /// 數值
        /// </summary>
        public float Value;
    }
}