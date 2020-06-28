using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    private Image _levelBar = null;

    [SerializeField]
    private Image _levelBackground = null;

    [SerializeField]
    private TextMeshProUGUI _totalGoldText = null;

    [SerializeField]
    private TextMeshProUGUI _comboPointText = null;

    [SerializeField]
    private GameObject _startGameUI = null;

    [SerializeField]
    private GameObject _restartGameUI = null;

    [SerializeField]
    private GameObject _inGameUI = null;

    [SerializeField]
    private GameObject _endGameUI = null;

    // Stores UI's current state
    private UIState _state = UIState.UI_STARTGAME;

    public void UpdateGoldAndScore()
    {
        UpdateTotalGold(GameManager.instance.MyCharacter.GetTotalGold());

        UpdateTotalScore(GameManager.instance.MyCharacter.GetTotalGold());
    }

    public void UpdateLevelBar(float currentDistance, float distanceMaxLimit)
    {
        float BGLength = _levelBackground.GetComponent<RectTransform>().rect.width;

        float BGHight = _levelBackground.GetComponent<RectTransform>().rect.height;

        float PBRate = currentDistance / distanceMaxLimit;

        float PBLength = BGLength * PBRate;

        _levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(PBLength, BGHight);
    }

    public void UpdateTotalScore(int currentTotalScore)
    {
        _totalScoreText.text = "Best: " + currentTotalScore.ToString();
    }

    public void UpdateLevelScore(int currentScore)
    {
        _levelScoreText.text = currentScore.ToString();
    }

    public void UpdateTotalGold(int currentGold)
    {
        _totalGoldText.text = "Gold: " + currentGold.ToString();

        Debug.Log("Gold: " + currentGold.ToString());
    }

    public void UpdateComboPoint(int point)
    {
        _comboPointText.text = "x" + point.ToString();
    }

    // Deactivate to ComboText
    public void DisableComboText()
    {
        _comboPointText.text = "";
    }

    // Deactivate to UI_STARTGAME
    public void DisableStartGameUI()
    {
        _startGameUI.SetActive(false);
    }

    // Deactivate to UI_InGame
    public void DisableOnGameUI()
    {
        _inGameUI.SetActive(false);
    }

    // Activate to UI_STARTGAME
    public void EnableStartGameUI()
    {
        _startGameUI.SetActive(true);
    }

    // Activate to UI_InGame
    public void EnableOnGameUI()
    {
        _inGameUI.SetActive(true);
    }
}
