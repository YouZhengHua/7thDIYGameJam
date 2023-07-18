using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitVFX;
    // [SerializeField] AudioData[] hitSFX;
    [SerializeField] float damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected Vector3 targetPos;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectlyCoroutine());
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.TryGetComponent<Character>(out Character character))
        // {
        //     character.TakeDamage(damage);
        //     PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
        //     AudioManager.Instance.PlayRandomSFX(hitSFX);
        //     gameObject.SetActive(false);
        // }
    }

    IEnumerator MoveDirectlyCoroutine()
    {
        while (gameObject.activeSelf)
        {
            Move();

            yield return null;
        }
    }

    public void SetTarget(Vector3 targetPos) => this.targetPos = targetPos;

    public void SetMoveDirection(Vector2 moveDirection) => this.moveDirection = moveDirection;

    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
}