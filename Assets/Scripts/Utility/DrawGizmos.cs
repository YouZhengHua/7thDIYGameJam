using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    public bool drawSphere = true;
    public Color drawSphereColor = Color.red;
    public float radius = 2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = drawSphereColor;
        if (drawSphere)
            Gizmos.DrawWireSphere(transform.position, radius);
    }
}
