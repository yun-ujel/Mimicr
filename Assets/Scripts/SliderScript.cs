using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;

    void Start()
    {
        Debug.Log(slider.value);
    }

    void Update()
    {
        
    }
}
