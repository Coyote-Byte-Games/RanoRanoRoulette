using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    public GameConfig config;
    // Start is called before the first frame update
    public void SetScrollSpeed(float speed)
    {
        config.scrollSpeed = ((int)(speed * 15));
    }
     public void SetZoom(float zoom)
    {
        config.zoom = ((int)((1 - zoom) * 50));
    }
}
