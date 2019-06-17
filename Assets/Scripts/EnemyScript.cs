using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] private Transform _checkPoint1;
    [SerializeField] private Transform _checkPoint2;
    [SerializeField] private float _speed;
    private bool _moveToOne = true;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _checkPoint1.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * _speed);


        ChangeDirectionCheck();
    }

    private void ChangeDirectionCheck()
    {
        if (_moveToOne == true && Vector3.Distance(transform.position, _checkPoint1.position) <=0.1f)
        {
            direction = (_checkPoint2.position - _checkPoint1.position).normalized;
            _moveToOne = false;
        }
        else if (_moveToOne == false && Vector3.Distance(transform.position, _checkPoint2.position) <= 0.1f)
        {
            direction = (_checkPoint1.position - _checkPoint2.position).normalized;
            _moveToOne = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Destroy(other.gameObject);
        }
    }
}
