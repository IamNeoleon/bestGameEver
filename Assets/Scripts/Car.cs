using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed = 1.0f;
    private Vector3 direction = new Vector3(1, 0, 0);

    public GameObject[] spawnPoint;
    private int pointNow = 0;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void changeRoad()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        pointNow += 1;
        transform.position = spawnPoint[pointNow].transform.position;

        if (pointNow >= 1)
        {
            pointNow = 0;
        }
    }

    private void OnTriggerStay(UnityEngine.Collider other)
    {
        if (other.CompareTag("flip"))
        {
            if (pointNow == 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            transform.position = spawnPoint[pointNow].transform.position;
            pointNow += 1;

            if (pointNow > 1)
            {
                pointNow = 0;
            }
        }
    }
}
