using System.Collections.Generic;
using UnityEngine;

public class PinGenerator : MonoBehaviour
{
    public int GeneratePasscode()
    {
        List<int> numbers = new List<int>
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9
        };

        string code = "";
        for (int i = 0; i < 6; i++)
        {
            int selection = Random.Range(0, numbers.Count);

            code += numbers[selection].ToString();

            numbers.Remove(numbers[selection]);
        }

        return int.Parse(code);
    }
}
