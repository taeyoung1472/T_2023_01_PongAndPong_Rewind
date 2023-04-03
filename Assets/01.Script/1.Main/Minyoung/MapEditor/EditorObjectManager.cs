using UnityEngine;
using UnityEngine.EventSystems;

public class EditorObjectManager : MonoBehaviour
{
    [Header("[Grid]")]
    [SerializeField] private Transform content;
    [SerializeField] private EditorObjectGrid gridPrefab;
    [SerializeField] private EditorObjectDataBase database;

    [Header("[Object]")]
    [SerializeField] private GameObject virtualDrawObject;
    private EditorObjectGrid prevSelectedGrid;
    private bool isHoldingObject;
    private Camera mainCam;
    private EditorObjectData curSelectingData;

    private void Start()
    {
        for (int i = 0; i < database.objectList.Count; i++)
        {
            Instantiate(gridPrefab, content).Init(database.objectList[i], this);
        }
        mainCam = Camera.main;
    }

    public void OnClickGrid(EditorObjectGrid grid)
    {
        SelectGrid(grid);
    }

    public void Update()
    {
        Rotate();
        Draw();
    }

    private void Rotate()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !isHoldingObject)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            virtualDrawObject.transform.Rotate(Vector3.forward, -15);
        }
        else if (scroll < 0)
        {
            virtualDrawObject.transform.Rotate(Vector3.forward, 15);
        }
    }

    private void Draw()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isHoldingObject && !EventSystem.current.IsPointerOverGameObject())
        {
            InsertObject();
            SelectGrid(null);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelectGrid(null);
        }

        if (EventSystem.current.IsPointerOverGameObject() || !isHoldingObject)
        {
            virtualDrawObject.SetActive(false);
        }
        else if (!EventSystem.current.IsPointerOverGameObject() && isHoldingObject)
        {
            virtualDrawObject.SetActive(true);
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCam.transform.position.z);
        virtualDrawObject.transform.position = mainCam.ScreenToWorldPoint(mousePos);
    }

    private void InsertObject()
    {
        for (int i = 0; i < virtualDrawObject.transform.childCount; i++)
        {
            Transform target = virtualDrawObject.transform.GetChild(0);
            Destroy(target.gameObject);
        }

        if(curSelectingData != null) 
        {
            CreateObjCommanad cmd = new CreateObjCommanad(curSelectingData, 
                virtualDrawObject.transform.position, 
                virtualDrawObject.transform.rotation);
            CommandManager.Instance.ExcuteCommand(cmd);
        }

        virtualDrawObject.transform.rotation = Quaternion.identity;
    }

    private void SelectGrid(EditorObjectGrid grid)
    {
        if (grid == null)
        {
            if (prevSelectedGrid)
                prevSelectedGrid.FocusGrid(false);

            isHoldingObject = false;
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
            isHoldingObject = true;

            for (int i = 0; i < virtualDrawObject.transform.childCount; i++)
            {
                Destroy(virtualDrawObject.transform.GetChild(0).gameObject);
            }
            GameObject _prefab = Instantiate(grid.Data.prefab, virtualDrawObject.transform.position, virtualDrawObject.transform.rotation);
            _prefab.transform.SetParent(virtualDrawObject.transform);
            curSelectingData = grid.Data;
        }
    }
}

