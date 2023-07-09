using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatRange = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        // 计算漂浮的位置偏移量
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;

        // 应用偏移量到鸟的Y轴位置
        transform.localPosition = new Vector3(transform.localPosition.x, startPosition.y + yOffset, transform.localPosition.z);
    }
}
