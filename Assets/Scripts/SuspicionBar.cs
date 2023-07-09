using UnityEngine;
using UnityEngine.UI;

public class SuspicionBar : MonoBehaviour
{
    [SerializeField] private Casino _casino;
    [SerializeField] private Slider _slider;
    [SerializeField] Player _player;

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
        {
            _slider.value = 0;
            _player.Leave();
        }
        else if (suspicionLevel > 1)
        {
            _slider.value = 1;
        }
        else
        {
            _slider.value = suspicionLevel;

            if (suspicionLevel > _player.SuperHappyValue)
                _player.SuperHappy();
            else if (suspicionLevel > _player.HappyValue)
                _player.Happy();
            else if (suspicionLevel > _player.NormalValue)
                _player.Normal();
            else
                _player.Sad();
                
        }
    }
}
