using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TriangleComplexUpdate : MonoBehaviour {

    public TriangleComplex triangleComplex;
    public Line referenceLine;

    public void RemoveDeactivatedElements()
    {
        List<Line> tmpLines = new List<Line>();
        foreach(Line line in triangleComplex.lines)
        {
            if(line)
            {
                if (line.gameObject.activeSelf)
                {
                    bool allTpNull = true;
                    foreach (TrianglePiece tp in line.trianglePieces)
                    {
                        if (tp && tp.gameObject.activeSelf)
                        {
                            allTpNull = false;
                            break;
                        }
                            
                    }
                    if(allTpNull)
                    {
                        DestroyImmediate(line.gameObject);
                    }
                    else
                    {
                        tmpLines.Add(line);
                    }
                }
                else
                {
                    DestroyImmediate(line.gameObject);
                }
            }
        }
        triangleComplex.lines = tmpLines;

        List<TrianglePiece> tmpTrianglePieces = new List<TrianglePiece>();
        foreach (TrianglePiece trianglePiece in triangleComplex.triangles)
        {
            if(trianglePiece)
            {
                if (trianglePiece.gameObject.activeSelf)
                {
                    tmpTrianglePieces.Add(trianglePiece);
                }
                else
                {
                    DestroyImmediate(trianglePiece.gameObject);
                }
            }
        }
        triangleComplex.triangles = tmpTrianglePieces;
    }

    public void UpdateFunctionalityImage()
    {
        foreach (TrianglePiece tp in triangleComplex.triangles)
        {
            FlexibleElementData data = tp.GetComponentInChildren<Triangle>().data;

            int i = 0;
            while (i++ < 10)
            {
                Transform functionalityImage = tp.transform.Find("FunctionalityImage");
                if(functionalityImage)
                {
                    DestroyImmediate(functionalityImage.gameObject);
                }
                else
                {
                    break;
                }
            }

            switch (tp.functionality)
            {
                case Functionality.normal:
                    GameObject n = null;
                    if (tp.points == 1)
                        continue;
                    else if (tp.points < 0)
                    {
                        n = new GameObject("FunctionalityImage");

                        GameObject n1 = new GameObject("Minus", typeof(SpriteRenderer));
                        n1.transform.parent = n.transform;
                        SpriteRenderer n1Sr = n1.GetComponent<SpriteRenderer>();
                        n1Sr.color = data.numberColor;
                        n1Sr.sortingOrder = 1;

                        GameObject n2 = new GameObject("Number", typeof(SpriteRenderer));
                        n2.transform.parent = n.transform;
                        SpriteRenderer n2Sr = n2.GetComponent<SpriteRenderer>();
                        n2Sr.color = data.numberColor;
                        n2Sr.sortingOrder = 1;

                        n1Sr.sprite = data.minus;
                        switch (tp.points)
                        {
                            case -1:
                                n1.transform.localPosition = Vector3.left * 0.25f;
                                n2Sr.sprite = data.one;
                                n2.transform.localPosition = Vector3.right * 0.15f;
                                break;
                            case -2:
                                n1.transform.localPosition = Vector3.left * 0.32f;
                                n2Sr.sprite = data.two;
                                n2.transform.localPosition = Vector3.right * 0.18f;
                                break;
                            case -3:
                                n1.transform.localPosition = Vector3.left * 0.32f;
                                n2Sr.sprite = data.three;
                                n2.transform.localPosition = Vector3.right * 0.18f;
                                break;
                            default:
                                break;
                        }
                        n.transform.parent = tp.transform;
                        n.transform.localPosition = Vector3.zero;
                        n.transform.localScale = data.numberSize;
                    }
                    else
                    {
                        n = new GameObject("FunctionalityImage", typeof(SpriteRenderer));
                        SpriteRenderer nSr = n.GetComponent<SpriteRenderer>();
                        switch (tp.points)
                        {
                            case 2:
                                nSr.sprite = data.two;
                                break;
                            case 3:
                                nSr.sprite = data.three;
                                break;
                            default:
                                break;
                        }
                        nSr.color = data.numberColor;
                        nSr.sortingOrder = 1;
                        n.transform.parent = tp.transform;
                        n.transform.localPosition = Vector3.zero;
                        n.transform.localScale = data.numberSize;
                    }
                    break;
                case Functionality.bomb:
                    GameObject b = new GameObject("FunctionalityImage", typeof(SpriteRenderer));
                    SpriteRenderer bSr = b.GetComponent<SpriteRenderer>();
                    bSr.sprite = data.bomb;
                    bSr.color = data.bombColor;
                    bSr.sortingOrder = 1;
                    b.transform.parent = tp.transform;
                    b.transform.localPosition = Vector3.zero;
                    b.transform.localScale = data.bombSize;
                    break;
                case Functionality.conquerNext:
                    GameObject c = new GameObject("FunctionalityImage", typeof(SpriteRenderer));
                    SpriteRenderer cSr = c.GetComponent<SpriteRenderer>();
                    cSr.sprite = data.arrow;
                    cSr.color = data.arrowColor;
                    cSr.sortingOrder = 1;
                    c.transform.parent = tp.transform;
                    c.transform.localPosition = Vector3.zero;
                    switch (tp.conquerNextDirection)
                    {
                        case Direction.normal:
                            break;
                        case Direction.up:
                            break;
                        case Direction.upRight:
                            c.transform.localRotation = Quaternion.Euler(0, 0, -60);
                            break;
                        case Direction.right:
                            c.transform.localRotation = Quaternion.Euler(0, 0, -90);
                            break;
                        case Direction.downRight:
                            c.transform.localRotation = Quaternion.Euler(0, 0, -120);
                            break;
                        case Direction.down:
                            c.transform.localRotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case Direction.downLeft:
                            c.transform.localRotation = Quaternion.Euler(0, 0, 120);
                            break;
                        case Direction.left:
                            c.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            break;
                        case Direction.upLeft:
                            c.transform.localRotation = Quaternion.Euler(0, 0, 60);
                            break;
                    }
                    c.transform.localScale = data.arrowSize;
                    break;
                default:
                    break;
            }
        }
    }

    public void UpdateParticleSystem()
    {
        ParticleSystem refSystem = referenceLine.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule refSetting = refSystem.main;
        ParticleSystem.EmissionModule refEmission = refSystem.emission;
        ParticleSystem.RotationOverLifetimeModule refRotationLifeTime = refSystem.rotationOverLifetime;
        ParticleSystem.SizeOverLifetimeModule refSizeLifetime = refSystem.sizeOverLifetime;
        ParticleSystemRenderer refRenderer = refSystem.GetComponent<ParticleSystemRenderer>();
        ParticleSystem.Burst refBurst = refEmission.GetBurst(0);

        foreach (Line line in triangleComplex.lines)
        {
            ParticleSystem particleSystem = line.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule particleSettings = particleSystem.main;
            ParticleSystem.EmissionModule particleEmission = particleSystem.emission;
            ParticleSystem.RotationOverLifetimeModule particleRotationLifeTime = particleSystem.rotationOverLifetime;
            ParticleSystem.SizeOverLifetimeModule particleSizeLifetime = particleSystem.sizeOverLifetime;
            ParticleSystemRenderer particleRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();

            particleSettings.customSimulationSpace = refSetting.customSimulationSpace;
            particleSettings.duration = refSetting.duration;
            particleSettings.emitterVelocityMode = refSetting.emitterVelocityMode;
            particleSettings.flipRotation = refSetting.flipRotation;
            particleSettings.gravityModifier = refSetting.gravityModifier;
            particleSettings.gravityModifierMultiplier = refSetting.gravityModifierMultiplier;
            particleSettings.loop = refSetting.loop;
            particleSettings.maxParticles = refSetting.maxParticles;
            particleSettings.playOnAwake = refSetting.playOnAwake;
            particleSettings.prewarm = refSetting.prewarm;
            particleSettings.scalingMode = refSetting.scalingMode;
            particleSettings.simulationSpace = refSetting.simulationSpace;
            particleSettings.simulationSpeed = refSetting.simulationSpeed;
            particleSettings.startColor = refSetting.startColor;
            particleSettings.startDelay = refSetting.startDelay;
            particleSettings.startDelayMultiplier = refSetting.startDelayMultiplier;
            particleSettings.startLifetime = refSetting.startLifetime;
            particleSettings.startLifetimeMultiplier = refSetting.startLifetimeMultiplier;
            particleSettings.startRotation = refSetting.startRotation;
            particleSettings.startRotation3D = refSetting.startRotation3D;
            particleSettings.startRotationMultiplier = refSetting.startRotationMultiplier;
            particleSettings.startRotationX = refSetting.startRotationX;
            particleSettings.startRotationXMultiplier = refSetting.startRotationXMultiplier;
            particleSettings.startRotationY = refSetting.startRotationY;
            particleSettings.startRotationYMultiplier = refSetting.startRotationYMultiplier;
            particleSettings.startRotationZ = refSetting.startRotationZ;
            particleSettings.startRotationZMultiplier = refSetting.startRotationZMultiplier;
            particleSettings.startSize = refSetting.startSize;
            particleSettings.startSize3D = refSetting.startSize3D;
            particleSettings.startSizeMultiplier = refSetting.startSizeMultiplier;
            particleSettings.startSizeX = refSetting.startSizeX;
            particleSettings.startSizeXMultiplier = refSetting.startSizeXMultiplier;
            particleSettings.startSizeY = refSetting.startSizeY;
            particleSettings.startSizeYMultiplier = refSetting.startSizeYMultiplier;
            particleSettings.startSizeZ = refSetting.startSizeZ;
            particleSettings.startSizeZMultiplier = refSetting.startSizeZMultiplier;
            particleSettings.startSpeed = refSetting.startSpeed;
            particleSettings.startSpeedMultiplier = refSetting.startSpeedMultiplier;
            particleSettings.stopAction = refSetting.stopAction;
            particleSettings.useUnscaledTime = refSetting.useUnscaledTime;

            particleEmission.burstCount = refEmission.burstCount;
            particleEmission.enabled = refEmission.enabled;
            particleEmission.rateOverDistance = refEmission.rateOverDistance;
            particleEmission.rateOverDistanceMultiplier = refEmission.rateOverDistanceMultiplier;
            particleEmission.rateOverTime = refEmission.rateOverTime;
            particleEmission.rateOverTimeMultiplier = refEmission.rateOverTimeMultiplier;

            particleRotationLifeTime.enabled = refRotationLifeTime.enabled;
            particleRotationLifeTime.separateAxes = refRotationLifeTime.separateAxes;
            particleRotationLifeTime.x = refRotationLifeTime.x;
            particleRotationLifeTime.xMultiplier = refRotationLifeTime.xMultiplier;
            particleRotationLifeTime.y = refRotationLifeTime.y;
            particleRotationLifeTime.yMultiplier = refRotationLifeTime.yMultiplier;
            particleRotationLifeTime.z = refRotationLifeTime.z;
            particleRotationLifeTime.zMultiplier = refRotationLifeTime.zMultiplier;

            particleSizeLifetime.enabled = refSizeLifetime.enabled;
            particleSizeLifetime.separateAxes = refSizeLifetime.separateAxes;
            particleSizeLifetime.size = refSizeLifetime.size;
            particleSizeLifetime.sizeMultiplier = refSizeLifetime.sizeMultiplier;
            particleSizeLifetime.x = refSizeLifetime.x;
            particleSizeLifetime.xMultiplier = refSizeLifetime.xMultiplier;
            particleSizeLifetime.y = refSizeLifetime.y;
            particleSizeLifetime.yMultiplier = refSizeLifetime.yMultiplier;
            particleSizeLifetime.z = refSizeLifetime.z;
            particleSizeLifetime.zMultiplier = refSizeLifetime.zMultiplier;

            particleRenderer.minParticleSize = refRenderer.minParticleSize;

            particleEmission.SetBursts(new ParticleSystem.Burst[] { refBurst });
        }
    }
}
