using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public float speed = 3f;
    public float waitTime = 1.5f;

    private Transform location0;
    private Transform location1;

    public void Initialize(Transform startLocation, Transform endLocation)
    {
        location0 = startLocation;
        location1 = endLocation;

        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true) 
        {
            yield return StartCoroutine(MoveToTarget(location1.position));
            
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(MoveToTarget(location0.position));

            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            
            yield return null; 
        }
    }
}
