using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public Sprite[] sprites; // Массив изображений Sprite, которые будем переключать
    public float timeBetweenSprites = 1.0f; // Время между переключениями в секундах

    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private float timer = 0.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Проверяем, что массив изображений не пустой
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("Не указаны изображения Sprite для переключения!");
            enabled = false; // Отключаем скрипт, чтобы не возникли ошибки
        }

        // Устанавливаем начальное изображение Sprite
        spriteRenderer.sprite = sprites[currentSpriteIndex];
    }

    void Update()
    {
        // Проверяем, что массив изображений не пустой
        if (sprites == null || sprites.Length == 0)
            return;

        // Обновляем таймер
        timer += Time.deltaTime;

        // Проверяем, прошло ли достаточно времени для переключения
        if (timer >= timeBetweenSprites)
        {
            timer = 0.0f;

            // Переключаем изображение на следующее в массиве
            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }
}
