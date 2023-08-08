using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public Sprite[] sprites; // ������ ����������� Sprite, ������� ����� �����������
    public float timeBetweenSprites = 1.0f; // ����� ����� �������������� � ��������

    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private float timer = 0.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ���������, ��� ������ ����������� �� ������
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("�� ������� ����������� Sprite ��� ������������!");
            enabled = false; // ��������� ������, ����� �� �������� ������
        }

        // ������������� ��������� ����������� Sprite
        spriteRenderer.sprite = sprites[currentSpriteIndex];
    }

    void Update()
    {
        // ���������, ��� ������ ����������� �� ������
        if (sprites == null || sprites.Length == 0)
            return;

        // ��������� ������
        timer += Time.deltaTime;

        // ���������, ������ �� ���������� ������� ��� ������������
        if (timer >= timeBetweenSprites)
        {
            timer = 0.0f;

            // ����������� ����������� �� ��������� � �������
            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }
}
