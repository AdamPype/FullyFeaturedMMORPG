using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowScript : MonoBehaviour
{
    [SerializeField] private GameObject _selectedItem;
    private GameObject _hand;
    [SerializeField] private float _throwingPower;

    // Start is called before the first frame update
    void Start()
    {
        _hand = this.gameObject;
        PutItemInHand();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject thrownItem = Instantiate<GameObject>(_selectedItem, _hand.transform.position + Camera.main.transform.forward*0.2f, Quaternion.identity);
            thrownItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            thrownItem.transform.parent = null;
            thrownItem.GetComponent<Rigidbody>().velocity = (Camera.main.transform.forward*Time.deltaTime*_throwingPower);
        }


    }


    private void DetectItemInHand()
    {
        _selectedItem = this.GetComponentInChildren<ThrowableItemScript>().gameObject;
    }

    private void PutItemInHand()
    {
        GameObject _itemInHand = Instantiate<GameObject>(_selectedItem.GetComponentInChildren<DummyItemModelScript>().gameObject, _hand.transform);
    }
}
