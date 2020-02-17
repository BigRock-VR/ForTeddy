using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomizerLaser : MonoBehaviour
{
    public LineRenderer[] laserLines = new LineRenderer[MAX_LASER_LINE];
    public AnimationCurve animationCurve;
    public float time;
    [HideInInspector] public float laserRange = 0.0f;
    [HideInInspector] public Transform startPosition;
    private const int MAX_LASER_LINE = 4;
    private bool isLoading;
    private const float MAX_LASER_RANGE = 10.0f;


    // BEZIER CURVE CALCULATION FOR EVERY LINE RENDER
    private const int MAX_CURVE_POINT = 2;
    public Vector3[] curvePoints;

    private void Start()
    {
        curvePoints = new Vector3[MAX_CURVE_POINT];
        InitLineRender();
        time = animationCurve.keys[animationCurve.keys.Length - 1].time;
    }
    private void InitLineRender()
    {
        for (int i = 0; i < MAX_LASER_LINE; i++)
        {
            laserLines[i].positionCount = MAX_CURVE_POINT;
            laserLines[i].enabled = false;
        }
    }

    public void EnableLaser()
    {
        curvePoints[0] = startPosition.position;
        Vector3 dir = startPosition.position + (startPosition.forward * laserRange);
        curvePoints[MAX_CURVE_POINT - 1] = dir;
        if (laserRange < MAX_LASER_RANGE && !isLoading)
        {
            StartCoroutine(InitLaserRange());
        }
        if (MAX_CURVE_POINT > 3)
        {
            CalculateCurvePoint();
        }

        for (int i = 0; i < MAX_LASER_LINE; i++)
        {
            laserLines[i].useWorldSpace = true;
            // Insert the start position and the end position of the raycast
            laserLines[i].SetPositions(curvePoints);
            laserLines[i].enabled = true;
        }
    }


    // Initialize the laser range interpolation based on a curve
    IEnumerator InitLaserRange()
    {
        isLoading = true;
        float t = 0.0f; // TIME
        while (t < 1)
        {
            t += Time.deltaTime / time;
            laserRange = Mathf.Lerp(0, MAX_LASER_RANGE, animationCurve.Evaluate(t));
            yield return null;
        }

        isLoading = false;
    }

    /*
     * Hide Line Renderer
     */
    public void DisableLineRender()
    {
        for (int i = 0; i < MAX_LASER_LINE; i++)
        {
            laserLines[i].useWorldSpace = false;
            laserLines[i].enabled = false;
            laserRange = 0.0f;
        }
    }

    public Vector3 GetBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (1.0f - t) * (1.0f - t) * (1.0f - t) * p0
        + 3.0f * (1.0f - t) * (1.0f - t) * t * p1
        + 3.0f * (1.0f - t) * t * t * p2
        + t * t * t * p3;
    }
    public void CalculateCurvePoint()
    {
        // Calculate the Quadratic Curve
        for (int i = 1; i < MAX_CURVE_POINT - 1; i++)
        {
            curvePoints[i] = GetBezierCurve(curvePoints[i - 1], curvePoints[i], curvePoints[i + 1], curvePoints[MAX_CURVE_POINT - 1], 0.2f);
        }
    }
}
