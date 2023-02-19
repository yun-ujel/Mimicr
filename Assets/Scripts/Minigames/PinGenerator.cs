using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PinGenerator", menuName = "PinGenerator")]
public class PinGenerator : ScriptableObject
{
    public int GeneratePin()
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

        // Run Loop Once without 0, so that first digit in code is never 0
        // There was an issue where the code would be 5 digits instead of 6 if the first digit was 0, and int.Parse() would remove it

        int selection = Random.Range(1, numbers.Count);
        code += numbers[selection].ToString();
        numbers.Remove(numbers[selection]);

        for (int i = 0; i < 5; i++)
        {
            selection = Random.Range(0, numbers.Count);

            code += numbers[selection].ToString();

            numbers.Remove(numbers[selection]);
        }

        return int.Parse(code);
    }
}
