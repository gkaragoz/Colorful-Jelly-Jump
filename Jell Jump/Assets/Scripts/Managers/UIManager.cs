using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _healtText;

    [SerializeField]
    private TextMeshProUGUI _pointText;

    private Character _myCharacter;

    private void Awake()
    {
        _myCharacter = FindObjectOfType<Character>();
    }

    private void Update()
    {
        _healtText.text = "Health: " + _myCharacter.Health();

        _pointText.text = "Point: " + _myCharacter.TotalPoint();
    }
}
