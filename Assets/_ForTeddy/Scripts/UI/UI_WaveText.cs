using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_WaveText : MonoBehaviour
{
    public Text text;
    public Font stringFont;
    public Font numberFont;
    public WaveManager waveManager;
    public string waveStart = "Wave Start !";
    public string waveEnd = "Wave Complete !";
    private Animator anim;
    private const float COUNT_DOWN_TIMER = 3.0f;
    private WaitForSeconds waitForOneSecond = new WaitForSeconds(1.0f);

    void Start()
    {
        anim = GetComponent<Animator>();
        waveManager.onEndWave += WaveManager_onEndWave;
        waveManager.onStartGame += WaveManager_onStartGame;
    }

    private void WaveManager_onStartGame()
    {
        text.font = numberFont;
        StartCoroutine(StartCountDown());
    }

    private void WaveManager_onEndWave()
    {
        text.font = stringFont;
        text.text = waveEnd;
        anim.SetTrigger("Show");
    }

    IEnumerator StartCountDown()
    {
        int countdown = (int)COUNT_DOWN_TIMER;
        while (countdown >= 0)
        {
            if (countdown == 0)
            {
                text.font = stringFont;
                text.text = waveStart;
                anim.SetTrigger("Show");
            }
            else
            {
                text.text = countdown.ToString();
                anim.SetTrigger("Show");
            }

            countdown--;
            yield return waitForOneSecond;
        }
    }
}
