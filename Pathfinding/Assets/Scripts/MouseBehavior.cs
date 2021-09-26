using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehavior : MonoBehaviour
{

    //public Transform target;
    Transform target;

    public CurveGenerator curveGenerator;

    Vector3 currentPos;
    Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        initialPos = transform.position;
        //target = curveGenerator.curvePoints[0].transform;
        ////target = curveGenerator.curvePoints[0].transform;

        //Vector3 dir = (target.transform.position - transform.position).normalized;
        //dir.y = 0;

        //Quaternion rot = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10 * Time.deltaTime);
    }

    // Update is called once per frame
    int i = -1;
    Vector3 targetPos;
    void Update()
    {
        if (i == -1) //primeiro ponto
        {
            i = 0;
        }
        else if (arrivedAt(i))
        {
            if (i < curveGenerator.curvePoints.Count - 1)
            {

                i++;
            }
        }

        target = curveGenerator.curvePoints[i].transform;
        targetPos = new Vector3(target.position.x, initialPos.y, target.position.z);

        Vector3 dir = (target.transform.position - transform.position).normalized;
        dir.y = 0;
      

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10 * Time.deltaTime);

        Debug.Log("i = " + i.ToString());

       
        transform.position = Vector3.Slerp(transform.position, targetPos, 100 * Time.deltaTime);
        currentPos = transform.position;
        //transform.position = new Vector3(transform.position.x, initialPos.y, transform.position.z);

        //transform.position = target.position;
        
    }

    bool arrivedAt(int iPos)
    {
        //Vector3 targetPos = curveGenerator.curvePoints[iPos].transform.position;
        //targetPos = new Vector3(targetPos.x, initialPos.y, targetPos.z);
        if (currentPos == targetPos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
