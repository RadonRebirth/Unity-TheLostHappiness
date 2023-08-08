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
    [Header("��������� TextTyping")]
    public Text textComponent;
    public List<string> stringsToType;
    public float charactersPerSecond;

    [Header("����� ������")]
    public List<StringColorInfo> stringColors;

    [Header("�����")]
    public List<SoundInfo> soundInfos;

    private AudioManager audioManager; // ������ �� AudioManager

    private bool isTyping = false;
    private int currentStringIndex = 0;
    private float currentCharacterIndex = 0f;

    private Dictionary<int, bool> hasPlayedSoundForString; // ������� ��� ������������ ������������ ����� ��� ������ ������

    public int CurrentStringIndex
    {
        get { return currentStringIndex; }
    }

    [Header("ID ��������� �����")]
    public int nextScene;

    [System.Serializable]
    public class ImageChangeInfo
    {
        public int stringIndex;
        public Sprite sprite;
    }
    [Header("���� (�������)")]
    public List<ImageChangeInfo> imageChangeInfos;

    private Dictionary<int, Sprite> imageChangeDictionary;
    private Dictionary<int, Color> stringColorDictionary;
    private Color originalTextColor;

    private Color brownColor = new Color32(0xA1, 0x40, 0x40, 0xFF); // ���������� ���� � ������� HEX (#a14040)
    private Color ejColor = new Color32(0xC7, 0x91, 0x71, 0xFF);
    private Color belkaColor = new Color32(0xB5, 0x52, 0x19, 0xFF);

    void Start()
    {
        textComponent.text = "";
        isTyping = true;

        // ��������� �������� ������
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

        // ������� AudioManager � �����
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager �� ������ � �����. ���������, ��� � ��� ���� ������ � ����������� AudioManager.");
        }

        // ������������� ������� ��� ������������ ������������ ����� ��� ������ ������
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
                // ���� ����� ��� ��� ����������, ���������� ��� ���������
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
                // �������� ���� ������������ ����� ��� ����� ������
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

        // ��������������� �����, ���� �����
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


            // ��������� �����, ���� �����
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

        // ���������� ���� (�������)
        if (imageChangeDictionary.ContainsKey(currentStringIndex))
        {
            GameObject.Find("Back").GetComponent<SpriteRenderer>().sprite = imageChangeDictionary[currentStringIndex];
        }

        // ���������� ����� ������
        if (stringColorDictionary.ContainsKey(currentStringIndex))
        {
            Color textColor;
            stringColorDictionary.TryGetValue(currentStringIndex, out textColor);
            textColor.a = originalTextColor.a; // ���������� ������������� �����-������
            textComponent.color = textColor;
        }
        else
        {
            textComponent.color = originalTextColor;
        }

        // ���������� ����������� ����� ��� ������������ �����
        string currentString = stringsToType[currentStringIndex];
        if (Regex.IsMatch(currentString, @"\[�����\]|\[\?\?\]"))
        {
            textComponent.color = brownColor;
        }
        if (Regex.IsMatch(currentString, @"\[��\]|\[\?\?\?\]"))
        {
            textComponent.color = ejColor;
        }
        if (Regex.IsMatch(currentString, @"\[�����\]"))
        {
            textComponent.color = belkaColor;
        }
    }
}
