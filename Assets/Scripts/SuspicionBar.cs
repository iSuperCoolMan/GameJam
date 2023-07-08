using UnityEngine;
using UnityEngine.UI;

public class SuspicionBar : MonoBehaviour
{
    [SerializeField] private Casino _casino;
    [SerializeField] private Slider _slider;

    private void OnEnable()
    {
        _casino.SuspicionChanged += ChangeSuspicion;
    }

    private void OnDisable()
    {
        _casino.SuspicionChanged -= ChangeSuspicion;
    }

    public void ChangeSuspicion(float suspicionLevel)
    {
        if (suspicionLevel < 0)
            _slider.value = 0;
        else if (suspicionLevel > 1)
            _slider.value = 1;
        else
            _slider.value = suspicionLevel;
    }
}
