using Scripts.Game.Data;
using UnityEngine;

namespace Scripts.Game
{
    public interface IEnemyController
    {
        public void AddVelocityTime(float delayTime);
        public IExpPool SetExpPool { set; }
        public EnemyData SetEnemyData { set; }
        public IEndUIController SetEndUI { set; }
        public IAttributeHandle SetAttributeHandle { set; }
        public IDamagePool SetDamagePool { set; }
        public IDropHealthPool SetDropHealthPool { set; }
        public float EnemyDamage { get; }
        public void PlayAttackAnimation();
        public bool IsDead { get; }
        public Transform SetPlayerTransform { set; }
    }
}