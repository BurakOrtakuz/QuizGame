using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0;
    int questionsSeen = 0;
    public int getCorrectAnswers()
    {
        return correctAnswers;
    }
    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }
    public int getQuestionsSeen()
    {
        return questionsSeen;
    }
    public void IncrementQuestionsSeen()
    {
        Debug.Log("AAA");
        questionsSeen++;
    }
    public int calculateScore()
    {
        return Mathf.RoundToInt((float)correctAnswers / (float)questionsSeen * 100);
    }
}
