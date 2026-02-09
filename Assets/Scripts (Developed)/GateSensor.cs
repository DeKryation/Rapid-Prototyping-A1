using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSensor : MonoBehaviour
{
    public bool isInSensor = false;
    public bool isMoving = false;

    private Vector3 curPos, endPos;
    private float speed = 5f;
    private float passedTime;

    
    void Start()
    {
        curPos = transform.parent.position;
        endPos = curPos + new Vector3(0f, 6f, 0f);
        passedTime = 0f;
    }
    void Update()
    {
        RaiseGate();
    }
    public void RaiseGate()
    {
        if (Input.GetKeyDown(KeyCode.F) && isInSensor)
        {
            if (GameManager.coins >= 1)
            {
                GameManager.coins -- ;
                isMoving = true;
                
            }
        }
        if (isMoving)
        {
            passedTime += Time.deltaTime;
            transform.parent.position = Vector3.Lerp(curPos, endPos, speed * passedTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInSensor = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInSensor = false;
        }
    }
}
