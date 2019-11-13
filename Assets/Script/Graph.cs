using System.Collections;
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
            MultiSine2DFunction };

    float step;
    Vector3 scale;
    Vector3 position;

    public float xFactor = 1f;
    public float yFactor = 1f;
    public float PIFactor = 1f;
    public float speed = 1f;
    #endregion




    // Start is called before the first frame update
    void Start()
    {

        /*
         * Transform.localPosition;
         * Position of the transform relative to the parent transform.
         * If the transform has no parent, it is the same as Transform.position.
        */

        step = (hRange * 1f) / resolution;
        scale = Vector3.one * step;
        position.z = 0;

        //array in X and Z axis
        realRes = resolution * 2;
        points = new Transform[realRes * realRes];

        for (int i = 0, z = 0; z < realRes; z++)
        {
            position.z = (z + 0.5f) * step - hRange;

            for (int x = 0; x < realRes; x++, i++)
            {
                //Instantiate always return T where T is the Type of the object
                points[i] = Instantiate(pointPrefab);
                position.x = (x + 0.5f) * step - hRange;
                points[i].localPosition = position;
                points[i].localScale = scale;
                points[i].name = "Point #" + i;
                points[i].SetParent(transform, false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        GraphFunctions f = functions[(int)function];


        for (int i = 0; i < points.Length; i++)
        {
            Vector3 position = points[i].localPosition;

            /*
            switch (functions)
            {
                case Functions.Sine:
                    position.y = SineFunction(position.x, Time.time);
                    break;
                case Functions.Multiple_Sine:
                    position.y = MultiSineFunction(position.x, Time.time);
                    break;
            }
            */

            position.y = f(position.x, position.z, Time.time);

            points[i].localPosition = position;
        }
    }




    static float SineFunction(float x, float z, float t)
    {
        //return Mathf.Sin((Mathf.PI * PIFactor) * ((x * xFactor) + (t * speed))) * yFactor; 
        return Mathf.Sin(PI * (x + t));
    }


    static float MultiSineFunction(float x, float z, float t)
    {
        float y = Mathf.Sin(PI * (x + t));
        y += Mathf.Sin((2 * PI) * (x + (2 * t))) / 2;
        y *= 2f / 3f;
        return y;
    }

    static float Sine2DFunction(float x, float z, float t)
    {
        //return Mathf.Sin(PI * (x + z + t));
        float y = Mathf.Sin(PI * (x + t));
        y += Mathf.Sin(PI * (z + t));
        y *= 0.5f;
        return y;
    }

    static float MultiSine2DFunction(float x, float z, float t)
    {
        float y = 4f * Mathf.Sin(PI * (x + z + t * 0.5f));
        y += Mathf.Sin(PI * (x + t));
        y += Mathf.Sin(2f * PI * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        return y;
    }
}
