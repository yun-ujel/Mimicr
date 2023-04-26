using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class IndividualAccountDisplay : MonoBehaviour
{
    [Header("Account Info")]
    [SerializeField] private GameObject[] posts = new GameObject[3];
    private RawImage[] postGraphics = new RawImage[3];
    private Outline[] postConfirmOutline = new Outline[3];
    private RawImage[] postConfirmGraphics = new RawImage[3];
    [SerializeField] private RawImage pINConfirmGraphic;

    [SerializeField] private TextMeshProUGUI pINText;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Graphics")]
    [SerializeField] private Texture2D correctGraphic;
    [SerializeField] private Texture2D incorrectGraphic;

    public void SetGraphics(AccountInfo accountInfo)
    {
        if (accountInfo.CurrentWindowIndex > 0)
        {
            pINConfirmGraphic.texture = correctGraphic;
            pINConfirmGraphic.color = new Color(1, 1, 1, 1);
        }
        else if (accountInfo.isFinished)
        {
            pINConfirmGraphic.texture = incorrectGraphic;
            pINConfirmGraphic.color = new Color(1, 1, 1, 1);
        }
        else
        {
            pINConfirmGraphic.color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < postGraphics.Length; i++)
        {
            postGraphics[i].texture = accountInfo.Posts[i];
            postGraphics[i].color = Color.HSVToRGB
            (
                accountInfo.CorrectPostColours[i].x,
                accountInfo.CorrectPostColours[i].y,
                accountInfo.CorrectPostColours[i].z
            );

            if (accountInfo.CurrentPostIndex > i)
            {
                postConfirmOutline[i].effectColor = new Color(0.08627451f, 0.827451f, 0.4078431f, 1);
                postConfirmGraphics[i].texture = correctGraphic;
                postConfirmGraphics[i].color = new Color(1, 1, 1, 1);
            }
            else if (accountInfo.isFinished)
            {
                postConfirmOutline[i].effectColor = new Color(1, 0.3254902f, 0.2901961f, 1);
                postConfirmGraphics[i].texture = incorrectGraphic;
                postConfirmGraphics[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                postConfirmOutline[i].effectColor = new Color(1, 1, 1, 0);
                postConfirmGraphics[i].color = new Color(1, 1, 1, 0);
            }
        }
        pINText.text = "PIN: " + accountInfo.PIN;
        nameText.text = "Account #" + (accountInfo.AccountIndex + 1).ToString();



    }

    private void Awake()
    {
        for (int i = 0; i < posts.Length; i++)
        {
            postConfirmOutline[i] = posts[i].GetComponent<Outline>();
            postGraphics[i] = posts[i].transform.GetChild(0).GetComponent<RawImage>();
            postConfirmGraphics[i] = posts[i].transform.GetChild(1).GetComponent<RawImage>();
        }
    }
}
