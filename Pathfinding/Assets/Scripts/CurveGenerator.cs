using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveGenerator : MonoBehaviour
{
    public List<GameObject> controlPoints = new List<GameObject>();
    public List<GameObject> curvePoints = new List<GameObject>();

    public GameObject controlPointPrefab;
    public GameObject curvePointPrefab;

    public int nPoints = 10; //Numero de pontos que a curva terá

    // Start is called before the first frame update
    void Start()
    {
        //Teste de como criar dinamicamente
        //Vector3 testPosition;
        //testPosition = controlPoints[0].transform.position + (controlPoints[1].transform.position - controlPoints[0].transform.position) / 2;
        //GameObject newPoint = Instantiate(curvePointPrefab, testPosition, Quaternion.identity) as GameObject;
        //newPoint.transform.rotation = controlPointPrefab.transform.rotation;
        //curvePoints.Add(newPoint);

        Matrix4x4 M = new Matrix4x4(new Vector4(-1, 3, -3, 1),
                      new Vector4(2, -5, 4, -1),
                      new Vector4(-1, 0, 1, 0),
                      new Vector4(0, 2, 0, 0));

        float step = 1.0f / nPoints;

        for (int i = 0; i <= controlPoints.Count - 4; i++)
        {
            
            for (float t = 0.0f; t <= 1.0f; t += step)
            {
                Vector3 p;

                Vector4 T = new Vector4(t* t*t, t* t, t, 1);

                Matrix4x4 G = new Matrix4x4(
                    new Vector4(controlPoints[i].transform.position.x, controlPoints[i].transform.position.y, controlPoints[i].transform.position.z,1.0f),
                    new Vector4(controlPoints[i + 1].transform.position.x, controlPoints[i + 1].transform.position.y, controlPoints[i + 1].transform.position.z, 1.0f),
                    new Vector4(controlPoints[i + 2].transform.position.x, controlPoints[i + 2].transform.position.y, controlPoints[i + 2].transform.position.z, 1.0f),
                    new Vector4(controlPoints[i + 3].transform.position.x, controlPoints[i + 3].transform.position.y, controlPoints[i + 3].transform.position.z, 1.0f)
                    );

                p = G * M * T;  //---------
                p = new Vector3(p.x * 0.5f, p.y* 0.5f, p.z * 0.5f);

                GameObject newPoint = Instantiate(curvePointPrefab, p, Quaternion.identity) as GameObject;
                newPoint.transform.rotation = controlPointPrefab.transform.rotation;
                curvePoints.Add(newPoint);
                //curve.push_back(p);
            }
}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
