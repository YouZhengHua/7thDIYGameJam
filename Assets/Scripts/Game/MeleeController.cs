using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class MeleeController : MonoBehaviour, IMeleeController
    {
        private IAttributeHandle _attributeHandle;
        private IGameFiniteStateMachine _gameFiniteStateMachine;
        private IAudioContoller _audio;
        private Animator _animator;
        private string meleeAttackTriggerName = "MeleeAttack";
        private string meleeAttackAnimationName = "MeleeAttack";
        private int attackHash;
        private SpriteRenderer _sprite;
        private BoxCollider2D _boxCollider2D;
        private SpriteRenderer _gunImage;
        private Transform _meleeContainerTransform;

        private void Awake()
        {
            _animator = GameObject.Find("MeleeContainer").GetComponent<Animator>();
            _meleeContainerTransform = GameObject.Find("MeleeContainer").GetComponent<Transform>();
            _sprite = gameObject.GetComponent<SpriteRenderer>();
            _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            attackHash = Animator.StringToHash(meleeAttackAnimationName);
            _sprite.enabled = false;
            _boxCollider2D.enabled = false;

        }

        private void Update()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == attackHash && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && _gameFiniteStateMachine.PlayerState == PlayerState.MeleeAttack)
            {
                _gameFiniteStateMachine.SetPlayerState(PlayerState.Idle);
                _sprite.enabled = false;
                _boxCollider2D.enabled = false;
                _gunImage.enabled = true;
            }
        }

        public void MeleeAttack()
        {
            _gameFiniteStateMachine.SetPlayerState(PlayerState.MeleeAttack);
            _sprite.enabled = true;
            _boxCollider2D.enabled = true;
            _gunImage.enabled = false;
            _meleeContainerTransform.localScale = _attributeHandle.MeleeScale;
            _animator.speed = _attributeHandle.MeleeAttackSpeed;
            _animator.SetTrigger(meleeAttackTriggerName);
            _audio.PlayEffect(_attributeHandle.MeleeAttackAudio);
        }

        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set => _gameFiniteStateMachine = value; }
        public SpriteRenderer SetGunImage { set => _gunImage = value; }
        public IAudioContoller SetAudio { set => _audio = value; }
    }
}