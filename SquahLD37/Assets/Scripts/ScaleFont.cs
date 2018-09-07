using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScaleFont : MonoBehaviour
{

    public Text text;

    public float ratio = 10;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
    }

    void OnGUI()
    {

        float finalSize = (float)Screen.width / ratio;
        text.fontSize = (int)finalSize;
    }


}