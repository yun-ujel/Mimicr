using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random6 : MonoBehaviour
{
    int GeneratePasscode()
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

        float code = 0;
        for (int i = 0; i < 6; i++)
        {
            int selection = Random.Range(0, numbers.Count);

            code += (numbers[selection] * Mathf.Pow(10, i));

            numbers.Remove(numbers[selection]);
        }

        return Mathf.RoundToInt(code);
    }

    private void Start()
    {

    }
}
