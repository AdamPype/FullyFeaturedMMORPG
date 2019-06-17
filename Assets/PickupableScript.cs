using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableScript : MonoBehaviour
{
    [SerializeField] private int _damage;
    private Material _mat;
    private Color _startCol;
    private Color _clearCol;

    private int _lookAtTime;
    private Transform _pickupFollow;
    private Rigidbody _rb;
    private Vector3 _startScale;

    private LayerMask _initLayer;

    // Start is called before the first frame update
    void Start()
    {
        _mat = transform.GetComponentInChildren<MeshRenderer>().material;
        _startCol = _mat.GetColor("_OutlineColor");
        _clearCol = _mat.GetColor("_OutlineColor");
        _clearCol.a = 0;
        _initLayer = gameObject.layer;
        gameObject.layer = 0;

        _rb = GetComponent<Rigidbody>();
        _startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        _mat.SetColor("_OutlineColor", Color.Lerp(_mat.GetColor("_OutlineColor"), _lookAtTime > 0 ? _startCol : _clearCol, 15 * Time.deltaTime));
        _lookAtTime -= 1;

        if (_pickupFollow != null)
            {
            transform.position = Vector3.Lerp(transform.position, _pickupFollow.position, 7 * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 7 * Time.deltaTime);

            if (transform.localScale.magnitude <= 0.04f)
                {
                Inventory.Instance.AddToInventory(gameObject);
                _pickupFollow = null;
                transform.localScale = _startScale;
                if (_rb) _rb.isKinematic = false;
                }
            }
        else if (!_rb.isKinematic && _rb.velocity.magnitude <= 0.1f)
            {
            gameObject.layer = 0;
            }
            
    }

    private void OnCollisionEnter(Collision collision)
        {
        if (collision.gameObject.tag == "Damagable")
            {
            collision.gameObject.GetComponent<DamagableScript>().DoDamage(_damage);
            }
        }

    public void LookAt()
        {
        _lookAtTime = 2;
        }

    public void PickUp(Transform player)
        {
        _pickupFollow = player;
        if (_rb) _rb.isKinematic = true;
        //Debug.Log("Pickup");
        gameObject.layer = _initLayer;
        }
}
