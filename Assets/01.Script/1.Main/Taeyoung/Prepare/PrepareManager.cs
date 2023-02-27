using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrepareManager : MonoBehaviour
{
    [Header("[Grid]")]
    [SerializeField] private Transform content;
    [SerializeField] private PrepareMemoGrid gridPrefab;
    [SerializeField] private PrepareMemoDatabase database;

    [Header("[DrawMemo]")]
    [SerializeField] private GameObject virtualIconObject;
    private PrepareMemoGrid prevSelectedGrid;
    private bool isHoldingMemo;
    private Camera mainCam;

    [Header("[WritingText]")]
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_FontAsset fontAsset;
    PrepareTextMemo curWritingMemo;

    [Header("[Focus]")]
    private Transform curFocusingMemo;

    private void Start()
    {
        for (int i = 0; i < database.memoList.Count; i++)
        {
            Instantiate(gridPrefab, content).Init(database.memoList[i], this);
        }
        mainCam = Camera.main;
        inputField.onEndEdit.AddListener(EndWrite);
    }

    public void OnClickGrid(PrepareMemoGrid grid)
    {
        SelectGrid(grid);
    }

    public void Update()
    {
        Rotate();
        Draw();
        Delete();
        Focus();
    }

    private void Rotate()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !isHoldingMemo)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            virtualIconObject.transform.Rotate(Vector3.forward, -15);
        }
        else if (scroll < 0)
        {
            virtualIconObject.transform.Rotate(Vector3.forward, 15);
        }
    }

    private void Delete()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(curFocusingMemo != null)
            {
                Destroy(curFocusingMemo.gameObject);
                curFocusingMemo = null;
            }
        }
    }

    private void Focus()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask memoLayer = 1 << LayerMask.NameToLayer("Memo");

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 1000, Color.red);

        if (Physics.Raycast(ray, out hit, 10000, memoLayer))
        {
            if (hit.transform == curFocusingMemo)
                return;
            else if(curFocusingMemo != null)
                curFocusingMemo.DOScale(1, 0.2f).SetUpdate(true);

            curFocusingMemo = hit.transform;
            curFocusingMemo.DOScale(1.2f, 0.1f).SetUpdate(true);
        }
        else
        {
            if (curFocusingMemo != null)
            {
                curFocusingMemo.DOScale(1, 0.2f).SetUpdate(true);
                curFocusingMemo = null;
            }
        }
    }

    private void Draw()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isHoldingMemo && !EventSystem.current.IsPointerOverGameObject())
        {
            InsertMemo();
            SelectGrid(null);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelectGrid(null);
        }

        if (EventSystem.current.IsPointerOverGameObject() || !isHoldingMemo)
        {
            virtualIconObject.SetActive(false);
        }
        else if (!EventSystem.current.IsPointerOverGameObject() && isHoldingMemo)
        {
            virtualIconObject.SetActive(true);
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCam.transform.position.z);
        virtualIconObject.transform.position = mainCam.ScreenToWorldPoint(mousePos);
    }

    private void InsertMemo()
    {
        GameObject obj = Instantiate(virtualIconObject, virtualIconObject.transform.position, virtualIconObject.transform.rotation);
        obj.GetComponent<SpriteRenderer>().color = Color.white;
        obj.AddComponent<SphereCollider>();
        virtualIconObject.transform.rotation = Quaternion.identity;

        switch (prevSelectedGrid.Data.memoType)
        {
            case MemoType.Icon:
                break;
            case MemoType.Memo:
                TextMeshPro tmp = new GameObject().AddComponent<TextMeshPro>();
                tmp.fontSize = 12;
                tmp.transform.SetParent(obj.transform);
                tmp.transform.localPosition = new Vector3(0, 0, -0.1f);
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.font = fontAsset;
                tmp.gameObject.layer = LayerMask.NameToLayer("Memo");
                tmp.color = Color.black;
                obj.AddComponent<PrepareTextMemo>().Init(tmp, this);
                break;
            default:
                break;
        }
    }

    private void SelectGrid(PrepareMemoGrid grid)
    {
        if (grid == null)
        {
            if (prevSelectedGrid)
                prevSelectedGrid.FocusGrid(false);

            isHoldingMemo = false;
            prevSelectedGrid = null;
        }
        else
        {
            // 선택 취소
            if (grid == prevSelectedGrid)
            {
                SelectGrid(null);
                return;
            }

            if (prevSelectedGrid)
                prevSelectedGrid.FocusGrid(false);
            grid.FocusGrid(true);

            prevSelectedGrid = grid;
            isHoldingMemo = true;
            virtualIconObject.GetComponent<SpriteRenderer>().sprite = grid.Data.icon;
        }
    }

    public void StartWriting(PrepareTextMemo textMemo)
    {
        curWritingMemo = textMemo;
        inputPanel.SetActive(true);
        inputField.ActivateInputField();
    }

    public void EndWrite(string str)
    {
        curWritingMemo.SetText(str);
        inputPanel.SetActive(false);
        inputField.text = "";
    }
}
