using UnityEngine;

public class RotateItem : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private void Update()
    {
        // Вращаем ингредиент вокруг своей оси
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
