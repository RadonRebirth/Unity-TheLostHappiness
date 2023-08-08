using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject conePrefab;
    public GameObject applePrefab;
    public GameObject pepperPrefab;
    public GameObject berriesPrefab;
    public GameObject cinnamonPrefab;
    public GameObject plPrefab;
    public Text text;

    private GameObject player;
    private List<GameObject> objDrops;
    private int dropsGathered;

    private int speed = 15;
    private int speedI = 2;
    private float speedD = 1;


    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("jungle");

        // Определение тега для съедобных ингредиентов (конусов)
        conePrefab.tag = "Cone";

        player = Instantiate(plPrefab, new Vector3(0, -2.4f, 0), Quaternion.identity);
        objDrops = new List<GameObject>();

        // Вызываем метод SpawnRaindrop каждые 2 секунды, начиная сразу после старта
        InvokeRepeating("SpawnRaindrop", 0f, speedD);
    }


    private void SpawnRaindrop()
    {
        GameObject objdropPrefab = null;
        int type = Random.Range(0, 5);

        switch (type)
        {
            case 0:
                objdropPrefab = conePrefab;
                break;
            case 1:
                objdropPrefab = applePrefab;
                break;
            case 2:
                objdropPrefab = pepperPrefab;
                break;
            case 3:
                objdropPrefab = berriesPrefab;
                break;
            case 4:
                objdropPrefab = cinnamonPrefab;
                break;
        }

        Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 5f, 0);
        GameObject objdrop = Instantiate(objdropPrefab, spawnPosition, Quaternion.identity);
        objDrops.Add(objdrop);
    }

    private void Update()
    {
        HandleInput();

        text.text = $"Шишек собрано: {dropsGathered}";

        for (int i = 0; i < objDrops.Count; i++)
        {
            GameObject objdrop = objDrops[i];
            objdrop.transform.position -= new Vector3(0, speedI * Time.deltaTime, 0);

            if (objdrop.transform.position.y < -4.5f)
            {
                // Игрок упустил ингредиент - проигрыш
                Destroy(objdrop);
                objDrops.RemoveAt(i);
                i--;
            }


            if (objdrop.GetComponent<BoxCollider2D>().bounds.Intersects(player.GetComponent<BoxCollider2D>().bounds))
            {
                if (objdrop.CompareTag("Cone"))
                {
                    // Съедобный ингредиент - добавляем очки
                    FindObjectOfType<AudioManager>().Play("Correct");
                    dropsGathered++;

                    if (dropsGathered % 3 == 0)
                    {
                        speedI += 1;
                        speedD -= 0.2f;

                    }

                    Destroy(objdrop);
                    objDrops.RemoveAt(i);
                    i--;
                    Debug.Log($"поймано:{dropsGathered}");
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("Wrong");
                    // Несъедобный ингредиент - проигрыш
                    Destroy(objdrop);
                    objDrops.RemoveAt(i);
                    i--;
                    Debug.Log("проигрыш");
                }
            }
        }

        // Проверяем условие победы
        if (dropsGathered >= 20)
        {
            // Игрок победил
            Debug.Log("Победа!");
            SceneManager.LoadScene("NovellaTLHFinal");
        }
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 plPosition = player.transform.position;
        plPosition.x += horizontalInput * speed * Time.deltaTime;
        plPosition.x = Mathf.Clamp(plPosition.x, -6f, 6f);
        player.transform.position = plPosition;
    }
}
