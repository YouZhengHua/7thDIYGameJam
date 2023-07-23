using System.Collections;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] protected Projectile projectile;
    [SerializeField] protected float minBallisticAngle = 50f;
    [SerializeField] protected float maxBallisticAngle = 75f;

    protected float ballisticAngle;

    protected Vector3 targetDirection;

    public IEnumerator HomingCoroutine(Vector3 targetPos)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);

        while (gameObject.activeSelf)
        {
            MoveAction(targetPos);
            yield return null;
        }
    }

    public virtual void MoveAction(Vector3 targetPos)
    {
        //隨機取得一個角度
        targetDirection = targetPos - transform.position;
        projectile.SetMoveDirection(targetDirection.normalized);
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
        transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
        projectile.Move();
    }
}

