using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class IndividualAccountDisplay : MonoBehaviour
{
    [Header("Account Info")]
    [SerializeField] private RawImage[] postGraphics = new RawImage[3];
    [SerializeField] private TextMeshProUGUI pINText;
    [SerializeField] private TextMeshProUGUI nameText;
    public void SetGraphics(AccountInfo accountInfo)
    {
        for (int i = 0; i < postGraphics.Length; i++)
        {
            postGraphics[i].texture = accountInfo.Posts[i];
            postGraphics[i].color = Color.white;
        }
        pINText.text = "PIN: " + accountInfo.PIN;
        nameText.text = "Account #" + (accountInfo.accountIndex + 1).ToString();
    }
}
