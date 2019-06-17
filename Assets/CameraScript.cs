using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float _shake;
    private Vector3 _startPos;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.localPosition;
    }
    

    // Update is called once per frame
    void Update()
        {
        
        transform.localPosition = _startPos + new Vector3(UnityEngine.Random.Range(-_shake, _shake), UnityEngine.Random.Range(-_shake, _shake));
        }
    
    
    }
