using System.Collections;
using UnityEngine;

public class BreakScreenController : MonoSingleTon<BreakScreenController>
{
    [SerializeField] private Material shatterMaterial;

    [SerializeField] private ScreenBreak slicesPrefabs;

    public bool isBreaking = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            //slices = Instantiate(slicesPrefabs, spawntrm);
            //obj.SetActive(false);
            StartCoroutine(CourtineScreenShot());
        }
    }
    public void StartBreakScreen()
    {
        StartCoroutine(CourtineScreenShot());
    }
    private IEnumerator CourtineScreenShot()
    {
        yield return new WaitForEndOfFrame();

        //Debug.Log("���� ��Ÿ��");
        slicesPrefabs.gameObject.SetActive(true);

        isBreaking = true;

        //���� ��ũ�� �����°�
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture2D.ReadPixels(rect, 0, 0);
        screenshotTexture2D.Apply();

        shatterMaterial.SetTexture("_BaseMap", screenshotTexture2D);

        //���� ����
        AudioManager.PlayAudioRandPitch(SoundType.OnReplayDisplay);
        yield return new WaitForSeconds(1f);
        slicesPrefabs.BreakScreen(slicesPrefabs.transform);
        yield return new WaitForSeconds(1.5f);
        slicesPrefabs.InitPos();
        isBreaking = false;
        slicesPrefabs.gameObject.SetActive(false);
    }

}
