using UnityEngine;

public class RotateItem : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private void Update()
    {
        // ������� ���������� ������ ����� ���
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
