using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerScript : MonoBehaviour
{
    [SerializeField] private MeshFilter _preview; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Inventory.Instance.HasItem())
            {
            GameObject item = Inventory.Instance.GetItem();
            _preview.mesh = item.GetComponentInChildren<MeshFilter>().mesh;
            _preview.GetComponent<MeshRenderer>().material = new Material(item.GetComponentInChildren<MeshRenderer>().material);
            }

        _preview.gameObject.SetActive(Inventory.Instance.HasItem());
    }
}
