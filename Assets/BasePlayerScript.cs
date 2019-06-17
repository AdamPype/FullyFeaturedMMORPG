using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//[RequireComponent(typeof(CharacterController))]
public class BasePlayerScript : MonoBehaviour {


    public float Acceleration;
    [SerializeField] private bool _moveWithPlatforms = true;
    [SerializeField] private float _drag;
    [SerializeField] private float _maximumXZVelocity = (30 * 1000) / (60 * 60); //[m/s] 30km/h
    [SerializeField] private float _jumpHeight;
    public float MouseSensitivity { get; set; }
    public bool SlipperyMode { get; set; }
    private Vector3 _mouseVel;

    private Transform _absoluteTransform;
    private CharacterController _char;

    private Vector3 _velocity = Vector3.zero; // [m/s]
    private Vector3 _inputMovement;
    private bool _jump;
    private bool _isJumping;

    private Vector3 _axisStartPos;
    //private SoundManager _snd;
    private bool _run;

    private int _footStepFrames;

    //private MeshRenderer _rend;

    void Start ()
        {
        //attach components
        _char = GetComponent<CharacterController>();
        //_rend = transform.GetChild(0).GetComponent<MeshRenderer>();
        _absoluteTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //_snd = GetComponent<SoundManager>();

        //set init vars
        _axisStartPos = _absoluteTransform.parent.localPosition;

        //dependency error
#if DEBUG
        Assert.IsNotNull(_char, "DEPENDENCY ERROR: CharacterController missing from PlayerScript");
        #endif

        }

    private void Update()
        {
        _inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetButtonDown("Jump") && !_isJumping)
            {
            _jump = true;
            }

        _run = Input.GetButton("Run");

        
        
        //move cam
        _mouseVel = new Vector3(Input.GetAxis("Mouse X") * MouseSensitivity * 10 * Time.deltaTime, -Input.GetAxis("Mouse Y") * MouseSensitivity * 10 * Time.deltaTime, 0);
        _absoluteTransform.parent.eulerAngles += Vector3.up * _mouseVel.x;
        
        //clamp cam
        float mouselook = _absoluteTransform.localEulerAngles.x + _mouseVel.y;
        mouselook = ClampAngle(mouselook, -75, 90);
        _absoluteTransform.localRotation = Quaternion.AngleAxis(mouselook, Vector3.right);
        }

    void FixedUpdate ()
        {
        SetParent();
        ApplyGround();
        ApplyGravity();
        ApplyMovement();
        ApplyDragOnGround();
        ApplyJump();
        LimitXZVelocity();

        Animate();
        //Footsteps();

        DoMovement();
        }

    private void SetParent()
        {
        if (_moveWithPlatforms && _char.isGrounded)
            {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.1f) && hit.collider)
                {
                if (hit.collider.CompareTag("Platform"))
                    transform.parent = hit.collider.transform;
                else
                    transform.parent = null;
                }
            }
        }

    //private void Footsteps()
    //    {
    //    if (_inputMovement != Vector3.zero)
    //        {
    //        if (_footStepFrames % (_run ? 25 : 30) == 0)
    //            _snd.Play("Step", false, 0);
    //        _footStepFrames++;
    //        }
    //    if (_inputMovement == Vector3.zero) _footStepFrames = 0;
    //    }

    private void Animate()
        {
        if (_velocity.magnitude > 0.02f)
            {
            _absoluteTransform.parent.localPosition = _axisStartPos + Vector3.up * 0.02f * _velocity.magnitude * (_run ? 1.5f : 1) * Mathf.Sin(FixedTime.fixedFrameCount * 0.2f * (_run ? 1.5f : 1));
            }
        else
            {
            _absoluteTransform.parent.localPosition = Vector3.Lerp(_absoluteTransform.parent.localPosition, _axisStartPos, 0.2f);
            }
        }

    private void ApplyGround()
        {
        if (_char.isGrounded)
            {
            //ground velocity
            _velocity -= Vector3.Project(_velocity, Physics.gravity);

            }
        }

    private void ApplyGravity()
        {
        if (!_char.isGrounded)
            {
            //apply gravity
            _velocity += Physics.gravity * Time.deltaTime;
            }
        }

    private void ApplyMovement()
        {
        if (_char.isGrounded)
            {
            //get relative rotation from camera
            Vector3 xzForward = Vector3.Scale(_absoluteTransform.forward, new Vector3(1, 0, 1));
            Quaternion relativeRot = Quaternion.LookRotation(xzForward);

            //move in relative direction
            Vector3 relativeMov = relativeRot * _inputMovement;
            _velocity += relativeMov * Acceleration * Time.deltaTime;
            }

        }

    private void LimitXZVelocity()
        {
        Vector3 yVel = Vector3.Scale(_velocity, Vector3.up);
        Vector3 xzVel = Vector3.Scale(_velocity, new Vector3(1, 0, 1));

        xzVel = Vector3.ClampMagnitude(xzVel, _maximumXZVelocity);

        _velocity = xzVel + yVel;
        }

    private void ApplyDragOnGround()
        {
        if (_char.isGrounded)
            {
            //drag
            _velocity = _velocity * (1 - _drag * Time.deltaTime); //same as lerp
            }
        }

    private void ApplyJump()
        {
        if (_char.isGrounded && _jump)
            {
            _velocity.y += Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
            _isJumping = true;
            }
        else if (_char.isGrounded)
            {
            _isJumping = true;
            }
        }

    private void DoMovement()
        {
        //do velocity / movement on character controller
        if (_velocity.magnitude > 0.1f)
            {
            Vector3 movement = _velocity * Time.deltaTime * (_run ? 1.5f : 1);
            _char.Move(movement);
            }
        }

    public static float ClampAngle(float angle, float min, float max)
        {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
            {
            inverse = !inverse;
            tmin -= 180;
            }
        if (angle > 180)
            {
            inverse = !inverse;
            tangle -= 180;
            }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
            {
            inverse = !inverse;
            tangle -= 180;
            }
        if (max > 180)
            {
            inverse = !inverse;
            tmax -= 180;
            }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
        }
    }
