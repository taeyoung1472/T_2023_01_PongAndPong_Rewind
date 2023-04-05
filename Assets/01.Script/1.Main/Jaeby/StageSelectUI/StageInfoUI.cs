using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    [SerializeField]
    private StageSelectUI _stageSelectUI = null;

    [SerializeField]
    private TextMeshProUGUI _stageNameText = null;
    [SerializeField]
    private TextMeshProUGUI _chapterNameText = null;
    [SerializeField]
    private TextMeshProUGUI _worldNameText = null;
    [SerializeField]
    private Image _stageImage = null;

    private Animator _animator = null;

    public void UIOn(StageDataSO data)
    {
        if (data == null)
            return;
        _stageSelectUI.Lock = true;
        _chapterNameText.SetText($"ц╘ем {_stageSelectUI.WorldIndex + 1}");
        _worldNameText.SetText(_stageSelectUI.CurStageWorld.WorldType.ToString());
        _stageImage.sprite = data.stageSprite;
        _stageNameText.SetText(data.stageName);

        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.Play("Enable");
    }

    public void UIDown()
    {
        _animator.Play("Disable");
    }

    public void GameObjectEnable()
    {
        gameObject.SetActive(true);
    }

    public void GameObjectDisable()
    {
        _stageSelectUI.Lock = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIDown();
        }
    }
}
