using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using UnityEngine.AI;
using System.IO;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Behaviour : MonoBehaviour
{   
    
    public NavMeshAgent agent;
    public Transform plane;
    private NavMeshHit hit;
    private float maxDistance = 30;
    private float radius = 50;
    private int areaMask = NavMesh.AllAreas;

    private Vector3 lastPosition;
    private Vector3 goPosition;
    private string state = "day";

    private float standartSpeed = 5;
    private float runSpeed = 15;
    public float rayDistance = 10f;
    public LayerMask layerMask;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetPoint();

        lastPosition = transform.position;
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && state == "day")
        {
            GetPoint();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (state == "day")
            {
                state = "night";
                goToHome();
            }
            else 
            {
                state = "day";
                GetComponent<MeshRenderer>().enabled = true;
                GetPoint();
            }
        }

    }

    private void GetPoint()
    {
        Vector3 targetPosition = transform.position;
        while (Vector3.Distance(targetPosition, transform.position) < 40)
        {
            targetPosition = transform.position + Random.insideUnitSphere * radius;
        }

        if (NavMesh.SamplePosition(targetPosition, out hit, maxDistance, areaMask))
        {
            lastPosition = transform.position;
            goPosition = hit.position;
            agent.SetDestination(hit.position);
        }
    }

    private void goToHome()
    {
        GameObject nearestObject = GetNearestObjectWithTag("door");
        agent.SetDestination(nearestObject.transform.position);
    }

    public void runAway()
    {
        agent.SetDestination(lastPosition);
    }

    GameObject GetNearestObjectWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestObject = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objectsWithTag)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestObject = obj;
            }
        }

        return nearestObject;
    }

    private void changeDirection(string changeVersion)
    {   
        if (changeVersion == "walk")
        {
            agent.speed = standartSpeed;
        }
        else if (changeVersion == "run")
        {
            agent.speed = runSpeed;

            float prevPosition = Vector3.Distance(transform.position, lastPosition);
            float nextPos = Vector3.Distance(transform.position, goPosition);
            

            if (prevPosition > nextPos)
            {
                agent.SetDestination(goPosition);
            }
            else
            {
                agent.SetDestination(lastPosition);
            }
        }
    }

    private void OnTriggerStay(UnityEngine.Collider other)
    {
        if (other.CompareTag("door") && state == "night")
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }


    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.CompareTag("car"))
        {
            changeDirection("walk");
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.CompareTag("car"))
        {
            changeDirection("run");
        }
    }

}
