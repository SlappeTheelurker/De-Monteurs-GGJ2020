using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class TextBubble : MonoBehaviour
{

    public TextMeshProUGUI textBox;
    public Image image;
    
    public void DisplayText(string text)
    {
        image.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);
        textBox.SetText(text);
        Invoke("Hide",3f);
    }

    public void Hide()
    {
        image.gameObject.SetActive(false);
        textBox.gameObject.SetActive(false);
    }
}
