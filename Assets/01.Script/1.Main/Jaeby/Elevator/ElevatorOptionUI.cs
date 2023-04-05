using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorOptionUI : MonoBehaviour
{
    private ElevatorInteract _elevator = null;
    public ElevatorInteract Elevator { get => _elevator; set => _elevator = value; }

    private bool _locked = false;
    public bool Locked { get => _locked; set { _locked = value; _lockImage.enabled = _locked; } }

    private Image _lockImage = null;
    private TextMeshProUGUI _text = null;
    private Button _button = null;

    private string _originString = "";

    public void OptionUIInit(string originString)
    {
        _lockImage = transform.Find("LockedIcon").GetComponentInChildren<Image>();
        _lockImage.enabled = _locked;
        _text = transform.GetComponentInChildren<TextMeshProUGUI>();
        _button = transform.GetComponentInChildren<Button>();
        _originString = originString;
        _text.SetText(_originString);
    }

    public void ButtonMapping()
    {
        ElevatorManager.Instance.TargetElevator = _elevator;
        ElevatorManager.Instance.CurElevator.ElevatorAnimation();
    }

    public void TextChange(ElevatorInteract curElevator)
    {
        if(curElevator == _elevator)
            _text.SetText("(현재 위치)     " + _originString);
        else
            _text.SetText(_originString);

        _button.enabled = CanRide(curElevator);
    }

    public bool CanRide(ElevatorInteract elevator)
    {
        return _elevator != elevator;
    }
}
