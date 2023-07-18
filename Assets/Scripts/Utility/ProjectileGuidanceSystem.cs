using System.Collections;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] float minBallisticAngle = 50f;
    [SerializeField] float maxBallisticAngle = 75f;

    float ballisticAngle;

    Vector3 targetDirection;

    public IEnumerator HomingCoroutine(Vector3 targetPos)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);

        while (gameObject.activeSelf)
        {
            targetDirection = targetPos - transform.position;
            projectile.SetMoveDirection(targetDirection.normalized);
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
            transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
            projectile.Move();

            yield return null;
        }
    }
}