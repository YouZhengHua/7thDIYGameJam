namespace Scripts.Game
{
    public interface IEndUIController
    {
        public void ShowCanvas(bool isSuccess);
        public void HideCanvas();
        public void AddShootCount();
        public void AddGetHitTimes();
        public void AddHitEnemyCount(int group);
        public void AddKillEnemyCountByGun();
        public void AddKillEnemyCountByMelee();
        public void AddKillBossCount();
        public void AddDamage(float value);
    }
}