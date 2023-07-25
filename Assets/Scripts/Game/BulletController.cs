using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Game
{
    public class BulletController : MonoBehaviour, IBulletEvent
    {
        private float _targetX = 0f;
        private float _targetY = 0f;
        private float _flySpeed = 0f;
        private bool _havePenetrationLimit = false;
        private int _penetrationCount = 1;
        private Rigidbody2D _rigidbody2D;
        [SerializeField, Header("飛行距離上限")]
        private float _maxRange = 15f;
        [SerializeField, Header("目標碰撞圖層")]
        private LayerMask _targetLayer;
        private float _damage;
        private UnityEvent<Collider2D> _onHitEvenet = new UnityEvent<Collider2D>();
        private Vector3 _startPostion;
        private bool _needAutoDestory = true;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (GameStateMachine.Instance.CurrectState == GameState.InGame)
            {
                _rigidbody2D.velocity = Vector2.ClampMagnitude(new Vector2(_targetX, _targetY), 1f) * _flySpeed;
            }
            if (_needAutoDestory && (transform.position - _startPostion).magnitude > _maxRange)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer | _targetLayer) == _targetLayer)
            {
                OnHitEvent?.Invoke(collision);
                if (_penetrationCount > 0 && _havePenetrationLimit)
                {
                    _penetrationCount--;
                    if (_penetrationCount == 0)
                        Destroy(gameObject);
                }
            }
        }

        public void Init(Vector3 from, Vector3 target, float speed, float damage, int penetrationCount = -1)
        {
            _targetX = target.x - from.x;
            _targetY = target.y - from.y;
            _flySpeed = speed;
            _havePenetrationLimit = penetrationCount > 0;
            _penetrationCount = penetrationCount;
            _damage = damage;
            _startPostion = transform.position;
        }

        public bool NeedAutoDestory { set => _needAutoDestory = value; }

        public float Damage { get => _damage; }
        public UnityEvent<Collider2D> OnHitEvent { get => _onHitEvenet; }
    }
}