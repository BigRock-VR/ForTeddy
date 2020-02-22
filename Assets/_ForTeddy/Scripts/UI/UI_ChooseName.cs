using System;
using UnityEngine.UI;
using UnityEngine;

public class UI_ChooseName : MonoBehaviour
{
    public Text[] charTexts = new Text[MAX_NAME_CHAR];
    public PlayerManager player;
    public RankingSystem ranking;
    public Text t_Score;
    public GameObject scoreMenu, gameOverMenu;
    public AnimationCurve animationCurve;

    public bool isOpen, isOpenChooseMenu;
    private char[] charValues = new char[MAX_NAME_CHAR];
    private float nextChangeTime;
    private int playerScore;
    private const int MAX_NAME_CHAR = 3;
    private int currSelectedIndx = 0;
    private string scoreString = "Score: {0}";
    private float timer = 0.0f;
    private float maxTimer;
    private Color clr_Text;
    private const int TIMER_GAME_OVER = 3;
    private const int TIMER_CHOOSE_NAME = 7;
    private float gameOverTimer = 0.0f;
    private void Start()
    {
        playerScore = player.score;
        t_Score.text = String.Format(scoreString, playerScore.ToString());
        maxTimer = animationCurve.keys[animationCurve.keys.Length - 1].time;
        timer = maxTimer;
        clr_Text = charTexts[0].color;
        player.onPlayerDeath += UI_ChooseName_onPlayerDeath;
        InitializeChar();
    }

    private void UI_ChooseName_onPlayerDeath()
    {
        isOpen = true;
    }


    void LateUpdate()
    {
        if (!isOpen)
        {
            return;
        }
        gameOverTimer += Time.deltaTime;

        if (gameOverTimer >= TIMER_GAME_OVER && gameOverTimer <= TIMER_CHOOSE_NAME)
        {
            gameOverMenu.SetActive(true);
        }

        if (gameOverTimer >= TIMER_CHOOSE_NAME && !isOpenChooseMenu)
        {
            gameOverMenu.SetActive(false);
            Open();
        }

        if (Input.GetAxisRaw("Horizontal") == 1 && Time.time >= nextChangeTime && isOpenChooseMenu)
        {
            nextChangeTime = Time.time + 0.2f;
            int oldIndex = currSelectedIndx;
            currSelectedIndx = Mathf.Clamp(++currSelectedIndx, 0, 2);
            charTexts[oldIndex].color = clr_Text;
        }
        if (Input.GetAxisRaw("Horizontal") == -1 && Time.time >= nextChangeTime && isOpenChooseMenu)
        {
            nextChangeTime = Time.time + 0.2f;
            int oldIndex = currSelectedIndx;
            currSelectedIndx = Mathf.Clamp(--currSelectedIndx, 0, 2);
            charTexts[oldIndex].color = clr_Text;
        }

        if (Input.GetAxisRaw("Vertical") == -1 && Time.time >= nextChangeTime && isOpenChooseMenu)
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

        if (Input.GetAxisRaw("Vertical") == 1 && Time.time >= nextChangeTime && isOpenChooseMenu)
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

        if (Input.GetButtonDown("Submit") && isOpenChooseMenu)
        {
            Confirm();
        }

        TextAnimation();
    }


    public void Open()
    {
        isOpenChooseMenu = true;
        if (player.score <= 0)
        {
            Debug.Log("Close Game score is zero");
            Close();
        }
        else
        {
            playerScore = player.score;
            t_Score.text = String.Format(scoreString, playerScore.ToString());
            scoreMenu.SetActive(true);
        }
    }

    public void Close()
    {
        isOpen = false;
        //scoreMenu.SetActive(false);
        GameManager.Instance.RestartArcadeGame();
    }

    private void TextAnimation()
    {
        Color clr_Text = charTexts[currSelectedIndx].color;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = maxTimer;
        }

        clr_Text.a = Mathf.Lerp(0, 1, animationCurve.Evaluate(timer));
        charTexts[currSelectedIndx].color = clr_Text;
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
        Score _score = new Score(charValues, playerScore);
        ranking.GetComponent<RankingSystem>().AddPlayerScore(_score);
        Close();
    }
}
