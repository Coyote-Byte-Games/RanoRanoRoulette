
using UnityEngine;

public class BuffableBehaviour : MonoBehaviour
{
    public enum BuffIcon
    {
        GenericBuff,
        GenericDebuff,
        Clean
    }
 public GameObject[] buffIcons;

    private GameObject popup;
    public void Awake()
    {
        popup = Resources.Load("Prefabs/Popup") as GameObject;
    }

      public void CreatePopup(string text, BuffIcon icon)
    {
        var popupInst = Instantiate(popup, transform.position, Quaternion.identity);
        Destroy(popupInst, 1f);
        var script = popupInst.GetComponent<CharacterPopupScript>();
        script.SetText(text);
        script.SetImageExtra(Instantiate(Resources.Load($"Buff Icons/{icon.ToString()}") as GameObject));
        //someday ill make a central manager for these, Or just use resoures again


    }
    public void CreatePopup(string text)
    {
        var popupInst = Instantiate(popup, transform.position, Quaternion.identity);
        Destroy(popupInst, 1f);
        var script = popupInst.GetComponent<CharacterPopupScript>();
        script.SetText(text);
        script.SetImageExtra(null);
        //someday ill make a central manager for these, Or just use resoures again


    }
     public void GetSpeedDownGraphic()
    {

    }
}