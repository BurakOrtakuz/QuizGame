using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] List<QuestionSO> questions= new List<QuestionSO>();
    [SerializeField] QuestionSO currentQuestions;
    [SerializeField] TextMeshProUGUI questionText;
    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    public bool hasAnsweredEarly = true;
    [Header("Button Spriters")]
    [SerializeField] Sprite correctAnswerSprite;
    [SerializeField] Sprite defaultAnswerSprite;
    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;
    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;
    [Header("Slider")]
    [SerializeField] Slider slider;
    public bool isCompleate = false;
    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        slider.minValue = 0;
        slider.maxValue = questions.Count;
        slider.value = 0;
    }
    void Update() {
        timerImage.fillAmount = timer.fillFraction;
        
        if(timer.loadNextQuestion)
        {
            if(slider.value == slider.maxValue)
            {
                isCompleate = true;
                return;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonStatement(false);
        }
        
    }
    void GetNextQuestion()
    {  
        if(questions.Count >= 0)
        {
            SetButtonStatement(true);
            SetDefaultButtonsSprites();
            GetRandomQuestion();
            DisplayQuestion();
            scoreKeeper.IncrementQuestionsSeen();
            slider.value++;
        }   
    }
    
    void GetRandomQuestion()
    {
        int random = Random.Range(0,questions.Count);
        currentQuestions = questions[random];
        if(questions.Contains(currentQuestions))
        {
            questions.Remove(currentQuestions);
        }
    }
    public void onAnswerSelected(int index)
    {
        if(index >= 0)
        {
            hasAnsweredEarly = true;
            DisplayAnswer(index);
            SetButtonStatement(false);
            timer.CancelTimer();
            scoreText.text = "Score: " +scoreKeeper.calculateScore() + "%";
        }
    }
    void DisplayAnswer(int index)
    {
        Image buttonImage;
        if(index == currentQuestions.getCorrectAnswerIndex())
        {
            questionText.text = "Answer is correct";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = currentQuestions.getCorrectAnswerIndex();
            string correctAnswer = currentQuestions.getAnswer(correctAnswerIndex);
            questionText.text  = "Sorry correct answer was\n" + correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }
    void DisplayQuestion()
    {
        questionText.text = currentQuestions.getQuestion();
        for(int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestions.getAnswer(i);            
        }
    }
    void SetButtonStatement(bool state)
    {
        for(int i = 0; i<answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
    void SetDefaultButtonsSprites()
    {
        for(int i = 0; i<answerButtons.Length;i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
