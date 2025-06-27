using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UI_Account : MonoBehaviour
{
    public TMP_InputField NickNameInputField;

    private readonly List<string> _positiveAdverbs = new List<string>
    {
        "행복한", "즐거운", "기쁜", "신나는", "유쾌한", "활기찬", "밝은", "웃는", "따뜻한", "설레는"
    };

    private readonly List<string> _animals = new List<string>
    {
        "고양이", "강아지", "여우", "호랑이", "펭귄", "토끼", "사자", "곰", "하마", "다람쥐"
    };

    public string GenerateNickname()
    {
        string adverb = _positiveAdverbs[Random.Range(0, _positiveAdverbs.Count)];
        string animal = _animals[Random.Range(0, _animals.Count)];
        int number = Random.Range(100, 1000);

        return $"{adverb}{animal}{number}";
    }

    public void Login()
    {
        // 명세
        string nickname = NickNameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            nickname = GenerateNickname();
        }

        Account account = new Account(nickname);
        AccountManager.Instance.TryLogin(account);
    }
}