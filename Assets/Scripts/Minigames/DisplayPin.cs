using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DisplayPin : MonoBehaviour
{
    [Header("PIN")]
    [SerializeField] private TextMeshProUGUI text;
    [Header("Buttons")]
    [SerializeField] private GameObject buttonsLocation;
    [SerializeField] private GameObject[] buttons;
    [Header("Shaker")]
    [SerializeField] private UIShaker shaker;

    private string typedPin = string.Empty;
    private string targetPin = string.Empty;

    private List<GameObject> currentButtons;

    private void OnStackStart(AccountInfo accountInfo)
    {
        ResetPin(accountInfo.PIN);
    }

    private void Update()
    {
        GetNumInputs();

        if (typedPin == targetPin && typedPin != string.Empty && targetPin != string.Empty)
        {
            gameObject.BroadcastMessage("OnWindowComplete");
            typedPin = string.Empty;
        }
        else if (typedPin.Length > 5)
        {
            ClearType();
        }

        
        text.text = typedPin;
        
        
    }

    public void AddToType(int input)
    {
        typedPin += input.ToString();
    }

    public void ClearType()
    {
        shaker.TriggerShake(new Vector2(0.25f, 10f));
        typedPin = string.Empty;
    }

    void AddButton(int index)
    {
        GameObject button = Instantiate(buttons[index], buttonsLocation.transform);
        button.GetComponent<PinButton>().GetDisplay(this, index);
        currentButtons.Add(button);
    }

    void SetPin(int sixDigitPin)
    {
        targetPin = sixDigitPin.ToString();

        int[] digitsInPasscode = sixDigitPin.GetDigits();
        List<int> pin = new List<int>(sixDigitPin.GetDigits());
        int[] shuffledPin = pin.Shuffle<int>().ToArray();

        currentButtons = new List<GameObject>();

        for (int i = 0; i < shuffledPin.Length; i++)
        {
            AddButton(shuffledPin[i]);
        }
    }

    public void ResetPin(int sixDigitPin)
    {
        typedPin = string.Empty;
        if (currentButtons != null)
        {
            int count = currentButtons.Count;
            for (int i = 0; i < count; i++)
            {
                Destroy(currentButtons[i]);
            }
        }

        SetPin(sixDigitPin);
    }

    void GetNumInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            AddToType(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            AddToType(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            AddToType(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            AddToType(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            AddToType(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            AddToType(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            AddToType(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            AddToType(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            AddToType(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            AddToType(9);
        }
    }
}
