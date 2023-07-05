using Scripts.Game.Data;

namespace Scripts.Game
{
    public interface IEnemyController
    {
        public IGameFiniteStateMachine SetGameFinitStateMachine { set; }
        public void AddVelocityTime(float delayTime);
        public IExpPool SetExpPool { set; }
        public IDropAmmoPool SetDropAmmoPool { set; }
        public EnemyData SetEnemyData { set; }
        public IPlayerController SetPlayer { set; }
        public IEndUIController SetEndUI { set; }
        public IAttributeHandle SetAttributeHandle { set; }
        public IDamagePool SetDamagePool { set; }
        public IDropHealthPool SetDropHealthPool { set; }
        public float EnemyDamage { get; }
        public void PlayAttackAnimation();
        public bool IsDead { get; }
    }
}