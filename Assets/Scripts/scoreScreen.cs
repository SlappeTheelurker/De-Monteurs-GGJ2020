using UnityEngine;
using System.Collections;
using TMPro;

public class scoreScreen : MonoBehaviour
{
    public static int lastscore = 0;
    public TMP_InputField nameField;
    public GameObject enterNameScreen;
    public GameObject scorelist;
    public TextMeshProUGUI scoreslistText;

    void Start()
    {

    }

    public void SubmitName()
    {
        ScoreSaver.SaveScore(nameField.text,lastscore);
        scorelist.SetActive(true);
        enterNameScreen.SetActive(false);
        scoreslistText.SetText(ScoreSaver.LoadScores().Replace(",",": "));
    }
}
