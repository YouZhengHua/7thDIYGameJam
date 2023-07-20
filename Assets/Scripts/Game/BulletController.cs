using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Game
{
    public class BulletController : MonoBehaviour
    {
        private float _targetX = 0f;
        private float _targetY = 0f;
        private float _flySpeed = 0f;
        private int _penetrationCount = 1;
        private Rigidbody2D _rigidbody2D;
        [SerializeField, Header("飛行距離上限")]
        private float _maxRange = 15f;
        [SerializeField, Header("目標碰撞圖層")]
        private LayerMask _targetLayer;
        private UnityEvent _hitEvent;
        private Transform _playerContainer;
        private float _damage;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerContainer = GameObject.Find("PlayerContainer").GetComponent<Transform>();
        }

        private void Update()
        {
            if (GameStateMachine.Instance.CurrectState == GameState.InGame)
            {
                _rigidbody2D.velocity = Vector2.ClampMagnitude(new Vector2(_targetX, _targetY), 1f) * _flySpeed;
            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
            if((transform.position - _playerContainer.position).magnitude > _maxRange)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("子彈控制器與目標圖層碰撞", collision.gameObject);
            if((1 << collision.gameObject.layer | _targetLayer) == _targetLayer)
            {
                Debug.Log("擊中目標", gameObject);
                //_hitEvent?.Invoke();
                if(_penetrationCount > 0)
                {
                    _penetrationCount--;
                    if (_penetrationCount == 0)
                        Destroy(gameObject);
                }
            }
        }

        public void Init(Vector3 from, Vector3 target, float speed, int penetrationCount, float damage, UnityEvent hitEvent = null)
        {
            _targetX = target.x - from.x;
            _targetY = target.y - from.y;
            _flySpeed = speed;
            _penetrationCount = penetrationCount;
            //_hitEvent = hitEvent;
            _damage = damage;
        }

        public float Damage { get => _damage; }
    }
}