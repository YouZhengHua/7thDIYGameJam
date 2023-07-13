using UnityEngine;

namespace Scripts.Game
{
    public interface IAmmoController
    {
        public void HitEmeny();
        public Transform SetPlayerTransform { set; }
        public IEndUIController SetEndUI { set; }
        public bool IsActive { get; }
        public int AmmoGroup { get; set; }
    }
}