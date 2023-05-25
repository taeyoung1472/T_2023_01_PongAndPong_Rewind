using Cinemachine;
using DG.Tweening;
using Shapes;
using System.Collections;
using UnityEngine;

public class LabtopStart : MonoBehaviour
{
    [Header("[Camera]")]
    [SerializeField] private CinemachineVirtualCamera menuCam;
    [SerializeField] private float loadingEndFov;

    [Header("[Loading]")]
    [SerializeField] private Disc loadingDisc;
    [SerializeField] private float loadingSpeed = 10;
    [SerializeField] private AnimationCurve loadingSpeedCurve;

    [Header("[MainMenu]")]
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;

    [Header("[AudioClip]")]
    [SerializeField] private AudioClip loadingEnd;

    IEnumerator Start()
    {
        while (true)
        {   
            yield return null;
            loadingDisc.DashOffset += Time.deltaTime * loadingSpeed * loadingSpeedCurve.Evaluate(Time.time % 1);
            if (loadingDisc.DashOffset > 9)
            {
                //AudioManager.PlayAudio(loadingEnd);
                DOTween.To(() => loadingDisc.Color, x => loadingDisc.Color = x, new Color(1, 1, 1, 0), 1f);
                DOTween.To(() => loadingDisc.Radius, x => loadingDisc.Radius = x, loadingDisc.Radius * 2.5f, 1f);
                DOTween.To(() => loadingDisc.DashOffset, x => loadingDisc.DashOffset = x, loadingDisc.DashOffset + 2, 1f);
                DOTween.To(() => loadingDisc.Thickness, x => loadingDisc.Thickness = x, 0, 1f);
                //Sequence seq = DOTween.Sequence();
                //seq.Append(DOTween.To(() => menuCam.m_Lens.FieldOfView, x => menuCam.m_Lens.FieldOfView = x, loadingEndFov + 15, 0.25f));
                DOTween.To(() => menuCam.m_Lens.FieldOfView, x => menuCam.m_Lens.FieldOfView = x, loadingEndFov, 1);
                break;
            }
        }
        yield return new WaitForSeconds(1.5f);
        mainMenuCanvasGroup.interactable = true;
        DOTween.To(() => mainMenuCanvasGroup.alpha, x => mainMenuCanvasGroup.alpha = x, 1f, 1f);
    }
}
