using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableScript : MonoBehaviour
{

    [SerializeField] private float _health;
    [SerializeField] private ParticleSystem _poofImGone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDead();
    }


    public void DoDamage(float Damage)
    {
        _health -= Damage;
    }

    private void CheckDead()
    {
        if (_health <=0)
        {
            ParticleSystem PoofSystem = Instantiate<ParticleSystem>(_poofImGone, transform);
            PoofSystem.transform.parent = null;
            Destroy(this.gameObject);
        }
    }
}
