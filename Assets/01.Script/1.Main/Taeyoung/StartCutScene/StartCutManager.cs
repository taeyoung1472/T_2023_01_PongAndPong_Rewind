using DG.Tweening;
using UnityEngine;

public class StartCutManager : MonoSingleTon<StartCutManager>
{
    [SerializeField] private Transform pictureEndPos;
    [SerializeField] private GameObject picturePrefab;
    [SerializeField] private Material blackMat;

    private Transform curPicture;
    private Transform prevPicture;
    private float curZValue = -8.75f;

    private bool isActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPicture();
        }
    }

    public void SpawnPicture()
    {
        Debug.Log(StartCutStageController.Instance.curPlayingStage);
        Debug.Log(isActive);
        //Debug.Log(!StartCutStageController.Instance.curPlayingStage.isActive);

        if (StartCutStageController.Instance.curPlayingStage)
        {
            if (isActive && !StartCutStageController.Instance.curPlayingStage.isActive)
                return;
        }

        isActive = true;

        if(curPicture != null)
        {
            prevPicture = curPicture;
            Material mat = prevPicture.GetComponent<MeshRenderer>().materials[0];
            DOTween.To(() => mat.color, color => mat.color = color, new Color(0, 0, 0, 1), 1f);
            DOTween.To(() => mat.GetColor("_EmissionColor"), color => mat.SetColor("_EmissionColor", color), new Color(0, 0, 0, 1), 1f)
                .OnComplete(() =>
                {
                    prevPicture.GetComponent<MeshRenderer>().enabled = false;
                    Debug.Log("³¡³²");
                });
        }

        Vector2 randPos = Random.insideUnitCircle.normalized * 3f;
        GameObject picture = Instantiate(picturePrefab, new Vector3(pictureEndPos.position.x + randPos.x, pictureEndPos.position.y + randPos.y, curZValue), Quaternion.identity);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.Append(picture.transform.DOMove(new Vector3(pictureEndPos.position.x, pictureEndPos.position.y, curZValue), 1.5f));
        seq.Join(picture.transform.DORotate(new Vector3(0, 0, Random.Range(-2.5f, 2.5f)), 1.5f));
        seq.AppendCallback(() => StartCutStageController.Instance.PlayStage());
        seq.AppendCallback(() => curPicture = picture.transform);
        seq.AppendCallback(() => isActive = false);
        curZValue -= 0.001f;
    }
}
