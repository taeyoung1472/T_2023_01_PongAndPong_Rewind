using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFog : MonoBehaviour
{
    ParticleSystem fogParticle;
    private void Awake()
    {
        fogParticle = GetComponent<ParticleSystem>();

        var shape = fogParticle.shape;
        float shapeSize = shape.scale.x * shape.scale.y * shape.scale.z;

        var emission = fogParticle.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(shapeSize);
    }
}
