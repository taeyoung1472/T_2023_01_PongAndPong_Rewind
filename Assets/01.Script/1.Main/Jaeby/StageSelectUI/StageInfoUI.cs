using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : MonoBehaviour
{
    [SerializeField]
    private StageSelectUI _stageSelectUI = null;
    [SerializeField]
    private TextMeshProUGUI _collectionText = null;
    [SerializeField]
    private TextMeshProUGUI _stageNameText = null;
    [SerializeField]
    private TextMeshProUGUI _chapterNameText = null;
    [SerializeField]
    private TextMeshProUGUI _worldNameText = null;
    [SerializeField]
    private Image _stageImage = null;

    private Animator _animator = null;

    private bool _isEnable = false;
    public bool IsEnable => _isEnable;

    public void UIOn(StageDataSO data)
    {
        if (data == null)
            return;
        _stageSelectUI.Lock = true;
        _chapterNameText.SetText("챕터 " + _stageSelectUI.CurStageWorld.ChapterName);
        _worldNameText.SetText(_stageSelectUI.CurStageWorld.WorldName);
        _stageImage.sprite = data.stageSprite;
        _stageNameText.SetText(data.stageName);

        int colllectionCnt = 0;
        foreach (var item in data.stageCollection)
        {
            if (item)
            {
                colllectionCnt++;
            }
        }
        _collectionText.SetText("수집품 개수[" + colllectionCnt + "/" + data.stageCollection.Count + "]");

        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.Play("Enable");
        _isEnable = true;
    }

    public void UIDown()
    {
        _animator.Play("Disable");
        _isEnable = false;
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
        if (Input.GetKeyDown(KeyCode.Escape) && _isEnable)
        {
            UIDown();
        }
    }
}
