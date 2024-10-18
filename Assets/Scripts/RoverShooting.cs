using UnityEngine;
using UnityEngine.UI;

public class RoverShooting : MonoBehaviour
{
    public int PlayerNumber = 1;

    [SerializeField] private Rigidbody _shell;                   
    [SerializeField] private Transform _fireTransform;           
    [SerializeField] private Slider _aimSlider;                  
    [SerializeField] private AudioSource _shootingAudio;         
    [SerializeField] private AudioClip _chargingClip;            
    [SerializeField] private AudioClip _fireClip;                
    [SerializeField] private float _minLaunchForce = 15f;        
    [SerializeField] private float _maxLaunchForce = 30f;        
    [SerializeField] private float _maxChargeTime = 0.75f;       

    private string _fireButton;                
    private float _currentLaunchForce;         
    private float _chargeSpeed;                
    private bool _fired;                       

    private void OnEnable()
    {        
        _currentLaunchForce = _minLaunchForce;
        _aimSlider.value = _minLaunchForce;
    }
    private void Start()
    {       
        _fireButton = "Fire" + PlayerNumber;        
        _chargeSpeed = (_maxLaunchForce - _minLaunchForce) / _maxChargeTime;
    }
    private void Update()
    {        
        _aimSlider.value = _minLaunchForce;
        if (_currentLaunchForce >= _maxLaunchForce && !_fired)
        {           
            _currentLaunchForce = _maxLaunchForce;
            Fire();
        }        
        else if (Input.GetButtonDown(_fireButton))
        {           
            _fired = false;
            _currentLaunchForce = _minLaunchForce;
            _shootingAudio.clip = _chargingClip;
            _shootingAudio.Play();
        }        
        else if (Input.GetButton(_fireButton) && !_fired)
        {            
            _currentLaunchForce += _chargeSpeed * Time.deltaTime;
            _aimSlider.value = _currentLaunchForce;
        }        
        else if (Input.GetButtonUp(_fireButton) && !_fired)
        {            
            Fire();
        }
    }
    private void Fire()
    {       
        _fired = true;        
        Rigidbody shellInstance = Instantiate(_shell, _fireTransform.position, _fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = _currentLaunchForce * _fireTransform.forward;        
        _shootingAudio.clip = _fireClip;
        _shootingAudio.Play();        
        _currentLaunchForce = _minLaunchForce;
    }
}
