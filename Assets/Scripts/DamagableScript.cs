using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableScript : MonoBehaviour
{

    [SerializeField] private int _health;
    [SerializeField] private ParticleSystem _poofImGone;

    // Update is called once per frame
    void Update()
    {
        CheckDead();
    }


    public void DoDamage(int damage)
    {
        _health -= damage;
        Debug.Log("Hit!");
        }

    private void CheckDead()
    {
        if (_health <=0)
        {
            ParticleSystem PoofSystem = Instantiate<ParticleSystem>(_poofImGone, transform);
            PoofSystem.transform.parent = null;
            Destroy(this.transform.root.gameObject);
        }
    }
    }
