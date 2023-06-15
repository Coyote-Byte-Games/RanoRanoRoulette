using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSlider : MonoBehaviour
 {
 
     Slider slider;
     Bounds cBounds;
     Bounds vBounds;
     public ScrollRect scrollRect;
 
     void Start()
     {
         slider = GetComponent<Slider>();
        
     }
    void Update()
    {
 Invoke("VerticalAutoHide", 0.01f);
    }
     public void SetScrollBarHandleSize(float size)
     {
         scrollRect.horizontalNormalizedPosition = slider.value;
     }
 
     public void VerticalAutoHide()
     {
         // print("VerticalAutoHide");
         cBounds = new Bounds(scrollRect.content.rect.center, scrollRect.content.rect.size);
         vBounds = new Bounds(scrollRect.viewport.rect.center, scrollRect.viewport.rect.size);
 
         print($"cBounds.size.x = {cBounds.size.x} and vBounds.size.x = {vBounds.size.x}");
         if (cBounds.size.x > vBounds.size.x + 0.01f)
         {
             //show slider
             print("show");
             this.gameObject.SetActive(true);
         }
         else
         {
             //hide slider
             print("hide");
             this.gameObject.SetActive(false);
         }
     }
 }
