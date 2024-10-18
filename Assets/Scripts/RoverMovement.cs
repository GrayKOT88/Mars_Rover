using UnityEngine;

public class RoverMovement : MonoBehaviour
{
    public int PlayerNumber = 1;

    [SerializeField] private float _speed = 12f;
    [SerializeField] private float _turnSpeed = 180f;
    [SerializeField] private AudioSource _movementAudio;
    [SerializeField] private AudioClip _engineIdling;            
    [SerializeField] private AudioClip _engineDriving;           
    [SerializeField] private float _pitchRange = 0.2f;        

    private string _movementAxisName;          
    private string _turnAxisName;              
    private Rigidbody _rigidbody;              
    private float _movementInputValue;         
    private float _turnInputValue;             
    private float _originalPitch;              

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {       
        _rigidbody.isKinematic = false;       
        _movementInputValue = 0f;
        _turnInputValue = 0f;
    }
    private void OnDisable()
    {      
        _rigidbody.isKinematic = true;
    }
    private void Start()
    {        
        _movementAxisName = "Vertical" + PlayerNumber;
        _turnAxisName = "Horizontal" + PlayerNumber;       
        _originalPitch = _movementAudio.pitch;
    }
    private void Update()
    {        
        _movementInputValue = Input.GetAxis(_movementAxisName);
        _turnInputValue = Input.GetAxis(_turnAxisName);
        EngineAudio();
    }
    private void EngineAudio()
    {        
        if (Mathf.Abs(_movementInputValue) < 0.1f && Mathf.Abs(_turnInputValue) < 0.1f)
        {            
            if (_movementAudio.clip == _engineDriving)
            {                
                _movementAudio.clip = _engineIdling;
                _movementAudio.pitch = Random.Range(_originalPitch - _pitchRange, _originalPitch + _pitchRange);
                _movementAudio.Play();
            }
        }
        else
        {           
            if (_movementAudio.clip == _engineIdling)
            {               
                _movementAudio.clip = _engineDriving;
                _movementAudio.pitch = Random.Range(_originalPitch - _pitchRange, _originalPitch + _pitchRange);
                _movementAudio.Play();
            }
        }
    }
    private void FixedUpdate()
    {       
        Move();
        Turn();
    }
    private void Move()
    {      
        Vector3 movement = transform.forward * _movementInputValue * _speed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }
    private void Turn()
    {        
        float turn = _turnInputValue * _turnSpeed * Time.deltaTime;       
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
    }
}
