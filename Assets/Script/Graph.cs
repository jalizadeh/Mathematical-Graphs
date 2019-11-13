﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    #region VARIABLES
    //public enum Functions { Sine, Multiple_Sine};
    const float PI = Mathf.PI;

    Transform[] points;
    public Transform pointPrefab;

    [Range(1, 20)]
    public int resolution;
    int realRes; // resolution * 2

    [Range(1, 5)]
    public int hRange;


    //public Functions functions;
    public GraphFunctionName function;
    static GraphFunctions[] functions =
            { SineFunction,
            Sine2DFunction,
            MultiSineFunction,
            MultiSine2DFunction,
            Ripple,
            Cylinder,
            WobblyCylinder,
            TwistingCylinder,
            Ellipse,
            Sphere,
            PulsingSphere};

    float step;
    Vector3 scale;
    Vector3 position;

    public float xFactor = 1f;
    public float yFactor = 1f;
    public float PIFactor = 1f;
    public float speed = 1f;
    #endregion



    void Start()
    {
        /*
         * Transform.localPosition;
         * Position of the transform relative to the parent transform.
         * If the transform has no parent, it is the same as Transform.position.
        */
        
        //array in X and Z axis
        realRes = resolution * 2;
        points = new Transform[realRes * realRes];

        step = (hRange * 1f) / resolution;
        scale = Vector3.one * step;


        for (int i = 0; i < points.Length; i++)
        {
            //Instantiate always return T where T is the Type of the object
            points[i] = Instantiate(pointPrefab);
            points[i].localScale = scale;
            points[i].name = "Point #" + i;
            points[i].SetParent(transform, false);
        }
    }

   
    void Update()
    {
        GraphFunctions f = functions[(int)function];


        for (int i = 0, z = 0; z < realRes; z++)
        {
            float v = (z + 0.5f) * step - hRange;
            for (int x = 0; x < realRes; x++, i++)
            {
                float u = (x + 0.5f) * step - hRange;
                Vector3 position = points[i].localPosition;
                position = f(u, v, Time.time);
                points[i].localPosition = position;
            }
        }
    }




    static Vector3 SineFunction(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;

        p.y = Mathf.Sin(PI * (u + t));

        p.z = v;
        return p;
    }


    static Vector3 MultiSineFunction(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;

        float y = Mathf.Sin(PI * (u + t));
        y += Mathf.Sin((2 * PI) * (u + (2 * t))) / 2;
        y *= 2f / 3f;
        p.y = y;
        
        p.z = v;
        return p;
    }

    static Vector3 Sine2DFunction(float u, float v, float t)
    {
        
        Vector3 p;
        p.x = u;

        float y = Mathf.Sin(PI * (u + t));
        y += Mathf.Sin(PI * (v + t));
        y *= 0.5f;
        p.y = y;

        p.z = v;
        return p;
    }

    static Vector3 MultiSine2DFunction(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;

        float y = 4f * Mathf.Sin(PI * (u + v + t * 0.5f));
        y += Mathf.Sin(PI * (u + t));
        y += Mathf.Sin(2f * PI * (v + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        p.y = y;

        p.z = v;
        return p;
    }

    static Vector3 Ripple(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;

        float d = Mathf.Sqrt(u * u + v * v);

        //-t = moving outward
        //+t = moving inward
        float y = Mathf.Sin(PI * (4f * d - t));
        y /= 1f + 10f * d;
        p.y = y;

        p.z = v;
        return p;
    }


    static Vector3 Cylinder(float u, float v, float t)
    {
        float radius = 1f;

        Vector3 p;
        p.x = radius * Mathf.Sin(PI * u + t);
        p.y = v;
        p.z = radius * Mathf.Cos(PI * u + t);
        return p;
    }


    

    static Vector3 WobblyCylinder(float u, float v, float t)
    {
        float radius = 1f + Mathf.Sin(6f * PI * u + t) * 0.2f;

        Vector3 p;
        p.x = radius * Mathf.Sin(PI * u);
        p.y = v;
        p.z = radius * Mathf.Cos(PI * u);
        return p;
    }

    static Vector3 TwistingCylinder(float u, float v, float t)
    {
        //to make sure that the radius doesn't exceed 1, reduce its baseline to 4/5
        float radius = 0.8f + Mathf.Sin(PI * (6f * u + 2f * v + t)) * 0.2f;

        Vector3 p;
        p.x = radius * Mathf.Sin(PI * u);
        p.y = v;
        p.z = radius * Mathf.Cos(PI * u);
        return p;
    }

    static Vector3 Ellipse(float u, float v, float t)
    {
        float radiusU = 1f;
        float radiusV = 2f;

        Vector3 p;
        p.x = radiusU * Mathf.Sin(PI * u);
        p.y = v;
        p.z = radiusV * Mathf.Cos(PI * u);
        return p;
    }


    static Vector3 Sphere(float u, float v, float t)
    {
        float radius = Mathf.Cos(PI * 0.5f * v);

        Vector3 p;
        p.x = radius * Mathf.Sin(PI * u + t);
        p.y = Mathf.Sin(PI * 0.5f * v);
        p.z = radius * Mathf.Cos(PI * u + t);
        return p;
    }


    static Vector3 PulsingSphere(float u, float v, float t)
    {
        float radius = 0.8f + Mathf.Sin(PI * (6f * u + t)) * 0.1f;

        Vector3 p;
        radius += Mathf.Sin(PI * (4f * v + t)) * 0.1f;
        float s = radius * Mathf.Cos(PI * 0.5f * v);
        p.x = s * Mathf.Sin(PI * u);
        p.y = radius * Mathf.Sin(PI * 0.5f * v);
        p.z = s * Mathf.Cos(PI * u);
        return p;
    }
}