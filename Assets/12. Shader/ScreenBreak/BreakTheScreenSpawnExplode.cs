using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakTheScreenSpawnExplode : MonoBehaviour
{
    [SerializeField] private Transform explodeTransform;
    [SerializeField] private Material shatterMaterial;

    [SerializeField] private GameObject screenBreak;

    public Transform spawntrm;

    private GameObject obj;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            obj = Instantiate(screenBreak, spawntrm);
            //Debug.Log("?");
            //obj.SetActive(false);
            StartCoroutine(CourtineScreenShot());    
        }
    }

    private IEnumerator CourtineScreenShot()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture2D.ReadPixels(rect, 0, 0);
        screenshotTexture2D.Apply();

        shatterMaterial.SetTexture("_BaseMap", screenshotTexture2D);

        foreach (Transform child in obj.transform)
        {
            if (child.GetComponent<Transform>())
            {
                if(child.gameObject.GetComponent<MeshRenderer>())
                child.gameObject.GetComponent<MeshRenderer>().material = shatterMaterial;
            }
        }

        obj.gameObject.SetActive(true);
        obj.transform.position = spawntrm.position;
        yield return new WaitForSeconds(1f);
        ScreenBreak sr= obj.GetComponent<ScreenBreak>();
        sr.BreakScreen(obj.transform);
        yield return new WaitForSeconds(2f);
        sr.InitPos();
        obj.gameObject.SetActive(false);
    }

}
