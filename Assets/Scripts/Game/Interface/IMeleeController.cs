using UnityEngine;

namespace Scripts.Game
{
    public interface IMeleeController
    {
        public void MeleeAttack();

        public IAttributeHandle SetAttributeHandle { set; }

        public IGameFiniteStateMachine SetGameFiniteStateMachine { set; }

        public SpriteRenderer SetGunImage { set; }

        public IAudioContoller SetAudio { set; }
    }
}