using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonInteract : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool isSelected;
    private Color alphaOff = new Color(1, 1, 1, 0);
    public Animator bannerImgAnim;

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        if (bannerImgAnim)
        {
            bannerImgAnim.SetTrigger("Select");
        }
        this.gameObject.GetComponent<Image>().color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        this.gameObject.GetComponent<Image>().color = alphaOff;
    }

}
