using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    private GameObject FirstQuest;
    private GameObject SecondQuest;
    private GameObject ThirdQuest;
    private GameObject FourthQuest;


    [Header("Настройки TextTyping")]
    public Text textComponent;
    public List<string> stringsToType;
    public float charactersPerSecond;

    [Header("Цвета текста")]
    public List<StringColorInfo> stringColors;



    [Header("Звуки")]
    public List<SoundInfo> soundInfos;

    private AudioManager audioManager; // Ссылка на AudioManager

    private bool isTyping = false;
    private int currentStringIndex = 0;
    private float currentCharacterIndex = 0f;

    private Dictionary<int, bool> hasPlayedSoundForString; // Словарь для отслеживания проигрывания звука для каждой строки

    public int CurrentStringIndex
    {
        get { return currentStringIndex; }
    }

    [Header("ID следующей сцены")]
    public int nextScene;

    public void CorrectAnswer()
    {
        audioManager.Play("Correct");
        currentCharacterIndex = 0f;
        currentStringIndex++;
        if (currentStringIndex >= stringsToType.Count)
        {
            SceneManager.LoadScene(nextScene);
            return;
        }
        textComponent.text = "";
        isTyping = true;
    }

    public void WrongAnswer()
    {
        audioManager.Play("Wrong");
    }


    void Start()
    {
        FirstQuest = GameObject.Find("FirstQuest");
        SecondQuest = GameObject.Find("SecondQuest");
        ThirdQuest = GameObject.Find("ThirdQuest");
        FourthQuest = GameObject.Find("FourthQuest");
        SecondQuest.SetActive(false);
        ThirdQuest.SetActive(false);
        FourthQuest.SetActive(false);
        textComponent.text = "";
        isTyping = true;

        // Установка переноса текста
        textComponent.horizontalOverflow = HorizontalWrapMode.Wrap;

        // Находим AudioManager в сцене
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager не найден в сцене. Убедитесь, что у вас есть объект с компонентом AudioManager.");
        }

        // Инициализация словаря для отслеживания проигрывания звука для каждой строки
        hasPlayedSoundForString = new Dictionary<int, bool>();
        foreach (var soundInfo in soundInfos)
        {
            hasPlayedSoundForString[soundInfo.stringIndex] = false;
        }
    }

    void Update()
    { 
      
        // Сбросить флаг проигрывания звука для новой строки
        if (soundInfos != null && soundInfos.Exists(info => info.stringIndex == currentStringIndex))
        {
            SoundInfo soundInfo = soundInfos.Find(info => info.stringIndex == currentStringIndex);
            if (soundInfo != null)
            {
                hasPlayedSoundForString[currentStringIndex] = false;
            }
        }
         

        if (isTyping)
        {
            int roundedCharacterIndex = (int)currentCharacterIndex;
            if (roundedCharacterIndex <= stringsToType[currentStringIndex].Length)
            {
                textComponent.text = stringsToType[currentStringIndex].Substring(0, roundedCharacterIndex);
            }
            currentCharacterIndex += Time.deltaTime * charactersPerSecond;

            if (roundedCharacterIndex >= stringsToType[currentStringIndex].Length)
            {
                isTyping = false;
            }
        }
        // Воспроизведение звука, если нужно
        if (soundInfos != null)
        {
            if (soundInfos.Exists(info => info.stringIndex == currentStringIndex))
            {
                SoundInfo soundInfo = soundInfos.Find(info => info.stringIndex == currentStringIndex);
                if (!hasPlayedSoundForString[currentStringIndex] && !audioManager.IsPlaying(soundInfo.soundName))
                {
                    audioManager.Play(soundInfo.soundName);
                    hasPlayedSoundForString[currentStringIndex] = true;
                }
            }

            // Остановка звука, если нужно
            if (soundInfos.Exists(info => info.stopIndex == currentStringIndex))
            {
                SoundInfo soundInfo = soundInfos.Find(info => info.stopIndex == currentStringIndex);
                if (hasPlayedSoundForString.ContainsKey(soundInfo.stringIndex) && hasPlayedSoundForString[soundInfo.stringIndex])
                {
                    audioManager.Stop(soundInfo.soundName);
                    hasPlayedSoundForString[soundInfo.stringIndex] = false;
                }
            }
        }

        switch (currentStringIndex)
        {
            case 1:
                FirstQuest.SetActive(false);
                SecondQuest.SetActive(true);
                break;
            case 2:
                SecondQuest.SetActive(false);
                ThirdQuest.SetActive(true);
                break;
            case 3:
                ThirdQuest.SetActive(false);
                FourthQuest.SetActive(true);
                break;
            case 4:
                FourthQuest.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isTyping)
                    {
                        // Если текст все еще набирается, отобразить его полностью
                        textComponent.text = stringsToType[currentStringIndex];
                        isTyping = false;
                    }
                    else
                    {
                        currentCharacterIndex = 0f;
                        currentStringIndex++;
                        if (currentStringIndex >= stringsToType.Count)
                        {
                            SceneManager.LoadScene(nextScene);
                            return;
                        }
                        textComponent.text = "";
                        isTyping = true;
                    }
                } 
                    break;
            case 5:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isTyping)
                    {
                        // Если текст все еще набирается, отобразить его полностью
                        textComponent.text = stringsToType[currentStringIndex];
                        isTyping = false;
                    }
                    else
                    {
                        currentCharacterIndex = 0f;
                        currentStringIndex++;
                        if (currentStringIndex >= stringsToType.Count)
                        {
                            SceneManager.LoadScene(nextScene);
                            return;
                        }
                        textComponent.text = "";
                        isTyping = true;
                    }
                }
                break;
        }
    }
}
