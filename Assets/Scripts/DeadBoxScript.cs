using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBoxScript : MonoBehaviour
{

    [SerializeField] private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-transform.forward*Time.deltaTime*_speed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Destroy(other.gameObject);
        }
    }
}
