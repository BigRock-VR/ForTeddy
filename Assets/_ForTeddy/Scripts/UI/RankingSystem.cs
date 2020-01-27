using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections.Generic;

public class RankingSystem : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<Transform> entryTemplates = new List<Transform>(MAX_PLAYER_IN_RANKING);
    [SerializeField]
    private ScoreWrapper playerScores;


    private const float SPACE_BETWEEN_LINE = 20.0f;
    private const int MAX_PLAYER_IN_RANKING = 10;


    void Start()
    {
        // Check existing ranking table on the current PC
        if (!PlayerPrefs.HasKey("rankingTable"))
        {
            InitEmpityRanking(); // Initialize a default empity ranking
            LoadRanking();
        }
        else
        {
            LoadRanking();
        }
        SortRankingByScore();
        InitRanking(playerScores.scores);
    }



    void InitRanking(Score[] scores)
    {
        entryTemplate.gameObject.SetActive(false);

        for (int i = 0; i < MAX_PLAYER_IN_RANKING; i++)
        {
            entryTemplates.Add( Instantiate(entryTemplate, entryContainer));
            //entryTemplates[i].name = "RankingEntryTemplate_" + i;
            RectTransform entryRectTransform = entryTemplates[i].GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -SPACE_BETWEEN_LINE * i);
            entryTemplates[i].gameObject.SetActive(true);

            int rank = i + 1;

            entryTemplates[i].Find("pos_txt").GetComponent<Text>().text = rank.ToString();
            entryTemplates[i].Find("name_txt").GetComponent<Text>().text = scores[i].playerName;
            entryTemplates[i].Find("score_txt").GetComponent<Text>().text = scores[i].playerScore.ToString();

        }
    }

    void InitEmpityRanking()
    {
        ScoreWrapper empityList;
        empityList.scores = new Score[MAX_PLAYER_IN_RANKING];

        for (int i = 0; i < MAX_PLAYER_IN_RANKING; i++)
        {
            empityList.scores[i] = new Score("---", 0);
        }

        PlayerPrefs.SetString("rankingTable", JsonUtility.ToJson(empityList));
    }

    private void LoadRanking()
    {
        var JSON = PlayerPrefs.GetString("rankingTable");
        playerScores = JsonUtility.FromJson<ScoreWrapper>(JSON);
    }

    private void SaveRanking()
    {
        PlayerPrefs.SetString("rankingTable", JsonUtility.ToJson(playerScores));
    }

    public Score GetLowerScore()
    {
        return playerScores.scores.Last();
    }

    // Try to add some player to the ranking
    public void AddPlayerScore(Score _score)
    {
        Score lowerScore = GetLowerScore();

        // Insufficient score for the ranking
        if (_score.playerScore <= lowerScore.playerScore)
        {
            return;
        }

        playerScores.scores[MAX_PLAYER_IN_RANKING - 1] = _score;
        SortRankingByScore();
        SaveRanking();
        UpdateRanking();
    }

    private void UpdateRanking()
    {
        for (int i = 0; i < MAX_PLAYER_IN_RANKING; i++)
        {
            int rank = i + 1;

            entryTemplates[i].Find("pos_txt").GetComponent<Text>().text = rank.ToString();
            entryTemplates[i].Find("name_txt").GetComponent<Text>().text = playerScores.scores[i].playerName;
            entryTemplates[i].Find("score_txt").GetComponent<Text>().text = playerScores.scores[i].playerScore.ToString();
        }
    }

    private void SortRankingByScore()
    {
        playerScores.scores = playerScores.scores.OrderByDescending(player => player.playerScore).ToArray();
    }


    public void ResetScores()
    {
        PlayerPrefs.DeleteKey("rankingTable");
    }
}

[Serializable]
public class Score
{
    public string playerName;
    public int playerScore;

    // Costruct for the Char Name
    public Score(char[] c, int _score)
    {
        playerName = new string(c);
        playerScore = _score;
    }

    // Basic Construct
    public Score(string name, int _score)
    {
        playerName = name;
        playerScore = _score;
    }
}
//UNITY IS A SHIT ENGINE DONT SUPPORT JSON SERIALIZATION OF A CUSTOM LIST/ARRAY
[Serializable]
public struct ScoreWrapper
{
    public Score[] scores;
}
