using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCube : MonoBehaviour
{
    private float changeRotateTime = 2;
    private float changeTimer = 0;

    private float rotate;
    private void Start() {
        //rotate = GetRandomRotate();
    }
    void Update()
    {
        if (changeTimer > changeRotateTime) {
            rotate = GetRandomRotate();
            changeTimer = 0;
        }
        changeTimer += Time.deltaTime;
        //transform.Rotate(Vector3.up * rotate * Time.deltaTime);
    }

    private float lowerBound1 = -90f;
    private float upperBound1 = -20f;
    private float lowerBound2 = 20f;
    private float upperBound2 = 90f;
     
    public float GetRandomRotate() { 
        bool chooseRange1 = Random.value > 0.5f; 
        float randomNumber;
        if (chooseRange1) { 
            randomNumber = Random.Range(lowerBound1, upperBound1);
        } else { 
            randomNumber = Random.Range(lowerBound2, upperBound2);
        }

        return randomNumber;
    }
}
