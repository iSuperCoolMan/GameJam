using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Casino : MonoBehaviour
{
    [SerializeField] Game _game;
    [SerializeField] GamePanel _leavePanel;
    [SerializeField] GamePanel _offPanel;
    [SerializeField] GamePanel _winPanel;
    [SerializeField] private float _suspicionChangeByWin = 0.075f, _suspicionChangeByLose = 0.1f;
    [SerializeField] private int _coins = 100;

    private float _suspicionValue = 0.5f;
    private bool _isSecurityHere = false;

    public event UnityAction<float> SuspicionChanged;
    public event UnityAction<int> MoneyChanged;

    private void OnEnable()
    {
        _game.RewardCalculated += GetRollResult;
        _game.Started += RecieveCoins;
    }

    private void OnDisable()
    {
        _game.RewardCalculated -= GetRollResult;
        _game.Started -= RecieveCoins;
    }

    private void Start()
    {
        SuspicionChanged?.Invoke(_suspicionValue);
        MoneyChanged?.Invoke(_coins);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GetRollResult(uint coinsChange)
    {
        if (coinsChange != 0)
        {
            ChangeSuspicion(_suspicionChangeByWin * coinsChange);
            PayPlayer(coinsChange);

            if (!_isSecurityHere)
            {
                float randomNumber = Random.Range(0, 1f);

                if (randomNumber < _suspicionValue)
                    callSecurity();
            }
        }
        else
        {
            ChangeSuspicion(-_suspicionChangeByLose);

            if (_isSecurityHere)
                recallSecurity();
        }
    }

    private void ChangeSuspicion(float suspicionValue)
    {
        _suspicionValue += suspicionValue;
        SuspicionChanged?.Invoke(_suspicionValue);

        if (_suspicionValue < 0)
            _leavePanel.gameObject.SetActive(true);
        else if (_suspicionValue > 1f)
            _offPanel.gameObject.SetActive(true);
    }

    private void callSecurity()
    {
        _isSecurityHere = true;
    }

    private void recallSecurity()
    {
        _isSecurityHere = false;
    }

    private void PayPlayer(uint reward)
    {
        _coins -= (int)(reward * _game.Price);
        MoneyChanged?.Invoke(_coins);

        if (_coins <= 0)
            Win();
    }

    private void RecieveCoins(uint price)
    {
        _coins += (int)price;
        MoneyChanged?.Invoke(_coins);
    }

    private void Win()
    {
        _winPanel.gameObject.SetActive(true);
    }

    //private void Lose()
    //{
    //    Debug.Log("Lose");
    //}
}
