using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PowerUpDisplay : MonoBehaviour
{
    [Header("ScriptableObject")]
    public PowerUp powerUp;

    [Header("TextField")]
    public Text cost_Txt;
    public Text powerUpName_Txt;

    [Header("ImageField")]
    public Image powerUp_Img;


    // Start is called before the first frame update
    void Start()
    {
        cost_Txt.text = powerUp.cost.ToString();
        powerUpName_Txt.text = powerUp.powerUpName.ToString();

        powerUp_Img.sprite = powerUp.powerUpImage;
    }
}
