using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonInteract : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool isSelected;
    private Color alphaOff = new Color(1, 1, 1, 0);
    public Animator bannerImgAnim;

    [SerializeField] private FMODPlaySound m_FMODPlaySound;

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;

        if(m_FMODPlaySound != null)
        {
            m_FMODPlaySound.Play();
        }

        if (bannerImgAnim)
        {
            bannerImgAnim.SetTrigger("Select");
        }
        gameObject.GetComponent<Image>().color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        gameObject.GetComponent<Image>().color = alphaOff;
    }

}
