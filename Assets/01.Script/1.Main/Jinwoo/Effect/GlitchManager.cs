using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URPGlitch.Runtime.DigitalGlitch;
using URPGlitch.Runtime.AnalogGlitch;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GlitchManager : MonoSingleTon<GlitchManager>
{
    [SerializeField] Volume volume;

    private DigitalGlitchVolume _digitalGlitchFeature;
    private AnalogGlitchVolume _analogGlitchFeature;

    [Header("Digital")]
    [SerializeField, Range(0f, 1f)] public float _intensity = default;

    [Header("Analog")]
    [SerializeField, Range(0f, 1f)] public float _scanLineJitter = default;
    [SerializeField, Range(0f, 1f)] public float _verticalJump = default;
    [SerializeField, Range(0f, 1f)] public float _horizontalShake = default;
    [SerializeField, Range(0f, 1f)] public float _colorDrift = default;


    public Animator anime;
    private void Awake()
    {
        anime = GetComponent<Animator>();
    }
    private void Start()
    {
        volume.profile.TryGet<DigitalGlitchVolume>(out _digitalGlitchFeature);
        volume.profile.TryGet<AnalogGlitchVolume>(out _analogGlitchFeature);

        

    }

    void Update()
    {


        _digitalGlitchFeature.intensity.value = _intensity;

        _analogGlitchFeature.scanLineJitter.value = _scanLineJitter;
        _analogGlitchFeature.verticalJump.value = _verticalJump;
        _analogGlitchFeature.horizontalShake.value = _horizontalShake;
        _analogGlitchFeature.colorDrift.value = _colorDrift;


    }

    public void ZeroValue()
    {
        _intensity = 0;

        _scanLineJitter = 0;
        _verticalJump = 0;
        _horizontalShake = 0;
        _colorDrift = 0;
    }
    public void CoroutineColorDrift()
    {
        anime.Play("ColorDrift", 0);
        //StartCoroutine(ColorDriftValue());
    }
    IEnumerator ColorDriftValue()
    {
        while(_colorDrift < 0.7f)
        {

            _colorDrift += 0.05f;

            yield return new WaitForSeconds(0.02f);
        }
        ZeroValue();
    }

}
