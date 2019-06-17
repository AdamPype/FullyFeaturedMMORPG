using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLookScript : MonoBehaviour
{
    [SerializeField] private float _lookDistance;
    [SerializeField] private float _throwPower;

    // Update is called once per frame
    void Update()
    {
        if (Inventory.Instance.HasItem() && Input.GetMouseButtonDown(0))
            {
            GameObject itemDropped = Inventory.Instance.RemoveCurrItem();
            itemDropped.transform.position = Camera.main.transform.position;
            itemDropped.GetComponent<Rigidbody>()?.AddForce(Camera.main.transform.forward * _throwPower, ForceMode.Impulse);
            }
        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, _lookDistance))
            {
            PickupableScript pickup = hit.collider?.GetComponent<PickupableScript>();
            if (pickup)
                {
                pickup.LookAt();

                if (Input.GetMouseButtonDown(0) && !Inventory.Instance.HasItem())
                    {
                    pickup.PickUp(transform);
                    }
                }
            }
        
    }
}
