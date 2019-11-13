using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    #region VARIABLES
    //public enum Functions { Sine, Multiple_Sine};
    const float PI = Mathf.PI;
    float step;
    Vector3 scale;
    Vector3 position;

    Transform[] points;
    public Transform pointPrefab;

    [Range(1, 40)]
    public int resolution;
    int realRes; // resolution * 2

    [Range(1, 5)]
    public int hRange;


    [Header("Runtime Parameters")]

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
            PulsingSphere,
            Torus,
            InterestingTorus};

    [Range(0, 1)]
    public float uFactor = 1f;

    [Range(0, 1)]
    public float vFactor = 1f;

    [Range(0, 5)]
    public int piFactor = 1;

    [Range(-1, 1)]
    public float animationSpeed = 1f;
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
                position = f(u * uFactor, v * vFactor, PI * piFactor, Time.time * animationSpeed);
                points[i].localPosition = position;
            }
        }
    }




    static Vector3 SineFunction(float u, float v, float pi, float t)
    {
        Vector3 p;
        p.x = u;

        p.y = Mathf.Sin(pi * (u + t));

        p.z = v;
        return p;
    }


    static Vector3 MultiSineFunction(float u, float v, float pi, float t)
    {
        Vector3 p;
        p.x = u;

        float y = Mathf.Sin(pi * (u + t));
        y += Mathf.Sin((2 * pi) * (u + (2 * t))) / 2;
        y *= 2f / 3f;
        p.y = y;

        p.z = v;
        return p;
    }

    static Vector3 Sine2DFunction(float u, float v, float pi, float t)
    {

        Vector3 p;
        p.x = u;

        float y = Mathf.Sin(pi * (u + t));
        y += Mathf.Sin(pi * (v + t));
        y *= 0.5f;
        p.y = y;

        p.z = v;
        return p;
    }

    static Vector3 MultiSine2DFunction(float u, float v, float pi, float t)
    {
        Vector3 p;
        p.x = u;

        float y = 4f * Mathf.Sin(pi * (u + v + t * 0.5f));
        y += Mathf.Sin(pi * (u + t));
        y += Mathf.Sin(2f * pi * (v + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        p.y = y;

        p.z = v;
        return p;
    }

    static Vector3 Ripple(float u, float v, float pi, float t)
    {
        Vector3 p;
        p.x = u;

        float d = Mathf.Sqrt(u * u + v * v);

        //-t = moving outward
        //+t = moving inward
        float y = Mathf.Sin(pi * (4f * d - t));
        y /= 1f + 10f * d;
        p.y = y;

        p.z = v;
        return p;
    }


    static Vector3 Cylinder(float u, float v, float pi, float t)
    {
        float radius = 1f;

        Vector3 p;
        p.x = radius * Mathf.Sin(pi * u + t);
        p.y = v;
        p.z = radius * Mathf.Cos(pi * u + t);
        return p;
    }




    static Vector3 WobblyCylinder(float u, float v, float pi, float t)
    {
        float radius = 1f + Mathf.Sin(6f * pi * u) * 0.2f;

        Vector3 p;
        p.x = radius * Mathf.Sin(pi * u + t);
        p.y = v;
        p.z = radius * Mathf.Cos(pi * u + t);
        return p;
    }

    static Vector3 TwistingCylinder(float u, float v, float pi, float t)
    {
        //to make sure that the radius doesn't exceed 1, reduce its baseline to 4/5
        float radius = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v)) * 0.2f;

        Vector3 p;
        p.x = radius * Mathf.Sin(pi * u + t);
        p.y = v;
        p.z = radius * Mathf.Cos(pi * u + t);
        return p;
    }

    static Vector3 Ellipse(float u, float v, float pi, float t)
    {
        float radiusU = 1f;
        float radiusV = 2f;

        Vector3 p;
        p.x = radiusU * Mathf.Sin(pi * u + t);
        p.y = v;
        p.z = radiusV * Mathf.Cos(pi * u + t);
        return p;
    }


    static Vector3 Sphere(float u, float v, float pi, float t)
    {
        float radius = Mathf.Cos(pi * 0.5f * v);

        Vector3 p;
        p.x = radius * Mathf.Sin(pi * u + t);
        p.y = Mathf.Sin(pi * 0.5f * v);
        p.z = radius * Mathf.Cos(pi * u + t);
        return p;
    }


    static Vector3 PulsingSphere(float u, float v, float pi, float t)
    {
        float radius = 0.8f + Mathf.Sin(pi * (6f * u)) * 0.1f;

        Vector3 p;
        radius += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
        float s = radius * Mathf.Cos(pi * 0.5f * v);
        p.x = s * Mathf.Sin(pi * u + t);
        p.y = radius * Mathf.Sin(pi * 0.5f * v);
        p.z = s * Mathf.Cos(pi * u + t);
        return p;
    }


    static Vector3 Torus(float u, float v, float pi, float t)
    {
        float radius1 = 1f;
        float radius2 = 0.5f;
        float s = radius2 * Mathf.Cos(pi * v) + radius1;

        Vector3 p;
        p.x = s * Mathf.Sin(pi * u + t);
        p.y = radius2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u + t);
        return p;
    }


    static Vector3 InterestingTorus(float u, float v, float pi, float t)
    {
        float radius1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
        float radius2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
        float s = radius2 * Mathf.Cos(pi * v) + radius1;

        Vector3 p;
        p.x = s * Mathf.Sin(pi * u );
        p.y = radius2 * Mathf.Sin(pi * v);
        p.z = s * Mathf.Cos(pi * u);
        return p;
    }
}