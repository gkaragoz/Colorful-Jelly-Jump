using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        else if (instance != this) { Destroy(gameObject); }
    }

    #endregion

    [SerializeField]
    private TextMeshProUGUI _levelScoreText = null;

    [SerializeField]
    private TextMeshProUGUI _totalScoreText = null;

    [SerializeField]
    private Image _healthBar = null;

    [SerializeField]
    private Image _healthBackground = null;

    [SerializeField]
    private Image _levelhBar = null;

    [SerializeField]
    private Image _levelBackground = null;

    // Stores UI's current state
    private UIState _state = UIState.UI_STARTGAME;

    public void UpdateHealthBar(float currentHealth, float healthMaxLimit)
    {
        float BGLength = _healthBackground.GetComponent<RectTransform>().rect.width;

        float BGHight = _healthBackground.GetComponent<RectTransform>().rect.height;

        float PBRate = currentHealth / healthMaxLimit;

        float PBLength = BGLength * PBRate;

        _healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(PBLength, BGHight);
    }

    public void UpdateLevelBar(float currentDistance)
    {
        // TODO
    }

    public void UpdateTotalScore(int currentTotalScore)
    {
        _totalScoreText.text = "Best: " + currentTotalScore.ToString();
    }

    public void UpdateLevelScore(int currentScore)
    {
        _levelScoreText.text = currentScore.ToString();
    }
}
