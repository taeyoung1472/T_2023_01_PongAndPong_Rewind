using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Highlighters;

public class BreakTheScreenSpawnExplode : MonoBehaviour
{
    [SerializeField] private Material shatterMaterial;

    [SerializeField] private GameObject slicesPrefabs;

    private GameObject slices;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            //slices = Instantiate(slicesPrefabs, spawntrm);
            //Debug.Log("?");
            //obj.SetActive(false);
            StartCoroutine(CourtineScreenShot());    
        }
    }

    private IEnumerator CourtineScreenShot()
    {
        yield return new WaitForEndOfFrame();
        slicesPrefabs.gameObject.SetActive(true);

        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture2D.ReadPixels(rect, 0, 0);
        screenshotTexture2D.Apply();

        shatterMaterial.SetTexture("_BaseMap", screenshotTexture2D);

        yield return new WaitForSeconds(1f);
        ScreenBreak sr= slicesPrefabs.GetComponent<ScreenBreak>();
        sr.BreakScreen(slicesPrefabs.transform);
        yield return new WaitForSeconds(4f);
        sr.InitPos();
        yield return new WaitForSeconds(.5f);
        slicesPrefabs.gameObject.SetActive(false);
    }

}
