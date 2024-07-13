using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    [Header("Ranking Info")]
    [SerializeField] private Text firstText;
    [SerializeField] private Text secondText;
    [SerializeField] private Text thirdText;
    [SerializeField] private InputField nameInput;

    [SerializeField] private Text scoreText;
    [SerializeField] private Button commitBtn;

    [Header("Other")]
    [SerializeField]  private GameObject ranking;

    private void Awake()
    {
        nameInput.characterLimit = 5;
    }

    private void Update()
    {
        firstText.text = "1등: " + GameManager.Instance.rankName1 + " - " + GameManager.Instance.rankScore1 + "점";
        secondText.text = "2등: " + GameManager.Instance.rankName2 + " - " + GameManager.Instance.rankScore2 + "점";
        thirdText.text = "3등: " + GameManager.Instance.rankName3 + " - " + GameManager.Instance.rankScore3 + "점";

        scoreText.text = "점수: " + (int)GameManager.Instance.score + "점";
    }

    public void Submit()
    {
        SoundManager.Instance.PlaySound("Button");
        string temp = nameInput.text;
        if (temp.Length >= 5) { temp = temp.Substring(0, 5); }

        Insert(temp);

        commitBtn.interactable = false;
        Commit();
    }

    private void Insert(string temp)
    {
        if (GameManager.Instance.rankScore1 <= GameManager.Instance.score)
        {
            GameManager.Instance.rankScore3 = GameManager.Instance.rankScore2;
            GameManager.Instance.rankName3 = GameManager.Instance.rankName2;

            GameManager.Instance.rankScore2 = GameManager.Instance.rankScore1;
            GameManager.Instance.rankName2 = GameManager.Instance.rankName1;

            GameManager.Instance.rankScore1 = (int)GameManager.Instance.score;
            GameManager.Instance.rankName1 = temp;

        }

        else if (GameManager.Instance.rankScore2 <= GameManager.Instance.score)
        {
            GameManager.Instance.rankScore3 = GameManager.Instance.rankScore2;
            GameManager.Instance.rankName3 = GameManager.Instance.rankName2;

            GameManager.Instance.rankScore2 = (int)GameManager.Instance.score;
            GameManager.Instance.rankName2 = temp;
        }

        else if (GameManager.Instance.rankScore3 <= GameManager.Instance.score)
        {
            GameManager.Instance.rankScore3 = (int)GameManager.Instance.score;
            GameManager.Instance.rankName3 = temp;
        }
    }

    private void Commit()
    {
        firstText.text = "1등 : " + GameManager.Instance.rankName1 + " - " + GameManager.Instance.rankScore1;
        secondText.text = "2등 : " + GameManager.Instance.rankName2 + " - " + GameManager.Instance.rankScore2;
        thirdText.text = "3등 : " + GameManager.Instance.rankName3 + " - " + GameManager.Instance.rankScore3;

        DataManager.Instance.JsonSave();
    }

    public void Retry()
    {
        SceneManager.LoadScene("GameScene");
    }
}