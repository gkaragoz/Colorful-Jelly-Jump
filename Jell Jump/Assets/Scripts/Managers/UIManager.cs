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

    public void UpdateLevelBar(float currentDistance, float maxPlayerPosY)
    {
        float playerPosY = GameManager.instance.MyCharacter.transform.localPosition.y;

        float minPlayerPosY = -2.11f;

        float minUIPosX = -295;

        float maxUIPosX = 320;

        float mappedUIPosX = ExtensionMethods.Map(playerPosY, minPlayerPosY, maxPlayerPosY - 3.25f, minUIPosX, maxUIPosX);

        _levelBar.rectTransform.anchoredPosition = new Vector2(mappedUIPosX, _levelBar.rectTransform.anchoredPosition.y);
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
        _totalGoldText.text = currentGold.ToString();

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
