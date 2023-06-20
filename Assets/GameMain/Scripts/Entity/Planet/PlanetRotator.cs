using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public float rotationSpeed = 10f; // 自转速度

    private void Update()
    {
        // 在每一帧更新中，绕 Y 轴旋转
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
