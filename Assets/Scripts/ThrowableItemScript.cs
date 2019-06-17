using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItemScript : MonoBehaviour
{

    [SerializeField] private float _damage;

    [SerializeField] private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDespawn();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Damagable")
        {
            collision.gameObject.GetComponent<DamagableScript>().DoDamage(_damage);
        }
    }

    private void CheckDespawn()
    {
        _timer -= Time.deltaTime;
        if (_timer<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
