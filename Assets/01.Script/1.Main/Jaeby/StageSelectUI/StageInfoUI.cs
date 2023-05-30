using System.IO;
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

    public StageInfoUIState state = StageInfoUIState.None;

    public void UIOn(StageDataSO data)
    {
        if (data == null || state == StageInfoUIState.Animation || state == StageInfoUIState.On)
            return;

        gameObject.SetActive(true);
        //AudioManager.PlayAudio(SoundType.OnOpenStageInfo);
        _stageSelectUI.Lock = true;
        _chapterNameText.SetText("챕터 " + _stageSelectUI.CurStageWorld.ChapterName);
        _worldNameText.SetText(_stageSelectUI.CurStageWorld.WorldName);
        _stageImage.sprite = data.stageSprite;
        _stageNameText.SetText(data.stageName);

        SaveDataManager.Instance.LoadCollectionJSON();


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
        if (state == StageInfoUIState.Animation || state == StageInfoUIState.Off)
            return;
        //AudioManager.PlayAudio(SoundType.OnCloseStageInfo);
        _animator.Play("Disable");
        _isEnable = false;
    }

    public void StateChange(StageInfoUIState sta)
    {
        state = sta;
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
        if (Input.GetKeyDown(KeyCode.Escape) && _isEnable && !(state == StageInfoUIState.Animation))
        {
            UIDown();
        }
    }
}
