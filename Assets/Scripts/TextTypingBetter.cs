using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

[System.Serializable]
public class StringColorInfo
{
    public int stringIndex;
    public Color textColor;
}

[System.Serializable]
public class SoundInfo
{
    public int stringIndex;
    public int stopIndex;
    public string soundName;
    public bool shouldLoop;
}

public class TextTypingBetter : MonoBehaviour
{
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

    [System.Serializable]
    public class ImageChangeInfo
    {
        public int stringIndex;
        public Sprite sprite;
    }
    [Header("Фоны (спрайты)")]
    public List<ImageChangeInfo> imageChangeInfos;

    private Dictionary<int, Sprite> imageChangeDictionary;
    private Dictionary<int, Color> stringColorDictionary;
    private Color originalTextColor;

    private Color brownColor = new Color32(0xA1, 0x40, 0x40, 0xFF); // Коричневый цвет в формате HEX (#a14040)
    private Color ejColor = new Color32(0xC7, 0x91, 0x71, 0xFF);
    private Color belkaColor = new Color32(0xB5, 0x52, 0x19, 0xFF);

    void Start()
    {
        textComponent.text = "";
        isTyping = true;

        // Установка переноса текста
        textComponent.horizontalOverflow = HorizontalWrapMode.Wrap;

        imageChangeDictionary = new Dictionary<int, Sprite>();
        foreach (var info in imageChangeInfos)
        {
            imageChangeDictionary[info.stringIndex] = info.sprite;
        }

        stringColorDictionary = new Dictionary<int, Color>();
        foreach (var info in stringColors)
        {
            stringColorDictionary[info.stringIndex] = info.textColor;
        }

        originalTextColor = textComponent.color;

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
                // Сбросить флаг проигрывания звука для новой строки
                if (soundInfos != null && soundInfos.Exists(info => info.stringIndex == currentStringIndex))
                {
                    SoundInfo soundInfo = soundInfos.Find(info => info.stringIndex == currentStringIndex);
                    if (soundInfo != null)
                    {
                        hasPlayedSoundForString[currentStringIndex] = false;
                    }
                }
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

        // Обновление фона (спрайта)
        if (imageChangeDictionary.ContainsKey(currentStringIndex))
        {
            GameObject.Find("Back").GetComponent<SpriteRenderer>().sprite = imageChangeDictionary[currentStringIndex];
        }

        // Обновление цвета текста
        if (stringColorDictionary.ContainsKey(currentStringIndex))
        {
            Color textColor;
            stringColorDictionary.TryGetValue(currentStringIndex, out textColor);
            textColor.a = originalTextColor.a; // Сохранение оригинального альфа-канала
            textComponent.color = textColor;
        }
        else
        {
            textComponent.color = originalTextColor;
        }

        // Применение коричневого цвета для определенных строк
        string currentString = stringsToType[currentStringIndex];
        if (Regex.IsMatch(currentString, @"\[ФИЛИН\]|\[\?\?\]"))
        {
            textComponent.color = brownColor;
        }
        if (Regex.IsMatch(currentString, @"\[ЁЖ\]|\[\?\?\?\]"))
        {
            textComponent.color = ejColor;
        }
        if (Regex.IsMatch(currentString, @"\[БЕЛКА\]"))
        {
            textComponent.color = belkaColor;
        }
    }
}
