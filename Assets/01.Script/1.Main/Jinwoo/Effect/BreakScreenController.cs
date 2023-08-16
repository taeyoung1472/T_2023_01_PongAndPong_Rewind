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
            StartCoroutine(CourtineScreenShot());
        }
    }
    public void StartBreakScreen()
    {
        if (isBreaking)
        {
            Debug.Log("checkbreakscreen");
            slicesPrefabs.InitPos();
            isBreaking = false;
            slicesPrefabs.gameObject.SetActive(false);
            StopAllCoroutines();
        }
        StartCoroutine(CourtineScreenShot());
    }
    private IEnumerator CourtineScreenShot()
    {
        yield return new WaitForEndOfFrame();

        //Debug.Log("연출 스타또");
        slicesPrefabs.gameObject.SetActive(true);

        isBreaking = true;

        //현재 스크린 찍어놓는거
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture2D.ReadPixels(rect, 0, 0);
        screenshotTexture2D.Apply();

        shatterMaterial.SetTexture("_BaseMap", screenshotTexture2D);

        //블럭들 설정
        AudioManager.PlayAudioRandPitch(SoundType.OnReplayDisplay);
        yield return new WaitForSeconds(0.5f);
        slicesPrefabs.BreakScreen(slicesPrefabs.transform);
        yield return new WaitForSeconds(1f);
        slicesPrefabs.InitPos();
        isBreaking = false;
        slicesPrefabs.gameObject.SetActive(false);
    }

}
