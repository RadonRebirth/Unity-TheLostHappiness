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

        // ��������� �������� ������
        textComponent.horizontalOverflow = HorizontalWrapMode.Wrap;

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
      
        // �������� ���� ������������ ����� ��� ����� ������
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
                    }
                } 
                    break;
            case 5:
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
                    }
                }
                break;
        }
    }
}
