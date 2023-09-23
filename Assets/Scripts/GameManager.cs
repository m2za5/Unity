using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<Card> allCards;

    private Card flippedCard;

    private bool isFlipping = false;  

    [SerializeField]
    private Slider TimeoutSlider;

    [SerializeField]
    private TextMeshProUGUI TimeoutText;

    [SerializeField]
    private TextMeshProUGUI GameOverText;

    [SerializeField]
    private GameObject GameOverPanel;

    private bool isGameOver = false;

    [SerializeField]
    private float timeLimit = 60f;
    private float currentTime;

    private int totalMatchChiikawa = 10; 

    private int matchChiikawa = 0;


    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Board board = FindObjectOfType<Board>();
        allCards = board.GetCards();

        currentTime = timeLimit;

        SetCurrentTimeText();

        StartCoroutine("FlipAllCardsRoutine");
    }

    void SetCurrentTimeText() {
        int timeSec = Mathf.CeilToInt(currentTime);
        TimeoutText.SetText(timeSec.ToString());
    }

    IEnumerator FlipAllCardsRoutine() {
        isFlipping = true;
        yield return new WaitForSeconds(0.5f);
        FlipAllCards();
        yield return new WaitForSeconds(3f);
        FlipAllCards();
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;

        yield return StartCoroutine("CountDownTimerRoutine");
    }

    IEnumerator CountDownTimerRoutine() {
        while (currentTime > 0) {
            currentTime -= Time.deltaTime;
            TimeoutSlider.value = currentTime / timeLimit;
            SetCurrentTimeText();
            yield return null; //바로 다음 프레임의 작업을 이어서 함.
        }

        GameOver(false);
    }

    void FlipAllCards() {
        foreach (Card card in allCards) {
            card.FlipCard();
        }
    }

    public void CardClicked(Card card) {
        if (isFlipping || isGameOver) {
            return;
        }

        card.FlipCard();

        if (flippedCard == null) {
            flippedCard = card;
        } else {
            StartCoroutine(CheckMatchRoutine(flippedCard, card));
        }
    }

    IEnumerator CheckMatchRoutine(Card card1, Card card2) {
        isFlipping = true;

        if (card1.cardID == card2.cardID) {
            card1.SetMatched();
            card2.SetMatched();
            matchChiikawa++;

            if (matchChiikawa == totalMatchChiikawa) {
                GameOver(true);
            }
        } else {
            Debug.Log("Different Card");
            yield return new WaitForSeconds(1f);

            card1.FlipCard();
            card2.FlipCard();

            yield return new WaitForSeconds(0.4f);
        }

        isFlipping = false;
        flippedCard = null;
    }

    void GameOver(bool success) {
        
        if (!isGameOver) 
        {
            isGameOver = true;

            StopCoroutine("CountDownTimerRoutine");
        
            if (success) {
                GameOverText.SetText("You Match \nAll Chiikawa!");
            } else {
                GameOverText.SetText("GAME OVER!");
            }

            Invoke("ShowGameOevrPanel", 2f);

        }
        
    }

    void ShowGameOevrPanel() {
        GameOverPanel.SetActive(true);
    }

    public void Restart () {
        SceneManager.LoadScene("SampleScene");
    }
}
