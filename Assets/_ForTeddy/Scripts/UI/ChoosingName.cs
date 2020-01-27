using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChoosingName : MonoBehaviour
{
    int currSelectedIndx = 0;

    public Slider[] sliders = new Slider[MAX_NAME_CHAR];
    public Text[] charTexts = new Text[MAX_NAME_CHAR];

    public RankingSystem ranking;

    private char[] charValues = new char[MAX_NAME_CHAR];

    public Text scoreValue_Txt;

    public Button accept_Btn;

    private float nextChangeTime;

    private const int MAX_NAME_CHAR = 3;

    private void Start()
    {
        //Valore Da GameManager
        scoreValue_Txt.text = ArcadeManager.gm.score.ToString();
        InitializeChar();
        sliders[currSelectedIndx].Select();
    }
    void LateUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 && Time.time >= nextChangeTime)
        {
            nextChangeTime = Time.time + 0.2f;
            currSelectedIndx = Mathf.Clamp(++currSelectedIndx, 0, 3);
            if (currSelectedIndx == 3)
            {
                accept_Btn.Select();
            }
            else
            {
                sliders[currSelectedIndx].Select();
            }
        }
        if (Input.GetAxisRaw("Horizontal") == -1 && Time.time >= nextChangeTime)
        {
            nextChangeTime = Time.time + 0.2f;
            currSelectedIndx = Mathf.Clamp(--currSelectedIndx, 0, 3);
                sliders[currSelectedIndx].Select();
        }

        if (Input.GetAxisRaw("Vertical") == -1 && Time.time >= nextChangeTime)
        {
            nextChangeTime = Time.time + 0.2f;
            if (charValues[currSelectedIndx] == 65)
            {
                charValues[currSelectedIndx] = (char)90;
            }
            else
            {
                charValues[currSelectedIndx] = (char)Mathf.Clamp((int)--charValues[currSelectedIndx], 65, 90);
            }
            UpdateCharText();
        }
        if (Input.GetAxisRaw("Vertical") == 1 && Time.time >= nextChangeTime)
        {
            nextChangeTime = Time.time + 0.2f;
            if (charValues[currSelectedIndx] == 90)
            {
                charValues[currSelectedIndx] = (char)65;
            }
            else
            {
                charValues[currSelectedIndx] = (char)Mathf.Clamp((int)++charValues[currSelectedIndx], 65, 90);
            }
            UpdateCharText();
        }
    }


    private void SwitchCharToText(int index, int asciText)
    {
        char c = (char)asciText;
        charTexts[index].text = c.ToString();
    }

    private void UpdateCharText()
    {
        for (int i = 0; i < MAX_NAME_CHAR; i++)
        {
            charTexts[i].text = charValues[i].ToString();
        }
    }

    private void InitializeChar()
    {
        for (int i = 0; i < MAX_NAME_CHAR; i++)
        {
            char defaultChar = (char)65;
            charValues[i] = defaultChar;
            SwitchCharToText(i, defaultChar);
        }
    }

    public void Confirm()
    {
        // Create new score based on the char and the GameManager current score
        Score _score = new Score(charValues, ArcadeManager.gm.score);
        ranking.GetComponent<RankingSystem>().AddPlayerScore(_score);
        gameObject.SetActive(false);
    }
}
