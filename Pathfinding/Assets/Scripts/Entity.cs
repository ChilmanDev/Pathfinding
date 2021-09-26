using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Transform target;
    float speed = 10;
    Vector3[] path;
    int targetIndex;
    float r, g, b;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        r = Random.Range(0.0f, 1f);
        g = Random.Range(0.0f, 1f);
        b = Random.Range(0.0f, 1f);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSucessful)
    {
        if (pathSucessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = new Vector4(r, g, b, 1);
            for(int i = targetIndex; i < path.Length; i++)
            {
                //Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.6f, 0.6f, 0.6f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
