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
            
            yield return StartCoroutine(TurnToFace(location0.position, waitTime));

            yield return StartCoroutine(MoveToTarget(location0.position));

            yield return StartCoroutine(TurnToFace(location1.position, waitTime));
        }
    }

    IEnumerator TurnToFace(Vector3 target, float duration)
    {
        Vector3 directionToTarget = target - transform.position;
        directionToTarget.y = 0f; 

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);
            
            timeElapsed += Time.deltaTime;
            
            yield return null; 
        }

        transform.rotation = targetRotation;
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null; 
        }
    }
}
