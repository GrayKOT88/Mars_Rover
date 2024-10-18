using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _numRoundsToWin = 5;
    [SerializeField] private float _startDelay = 3f;             
    [SerializeField] private float _endDelay = 3f;
    [SerializeField] private CameraControl _cameraControl;       
    [SerializeField] private TextMeshProUGUI _messageText;                  
    [SerializeField] private GameObject _roverPrefab;            
    [SerializeField] private RoverManager[] _rovers;               

    private int _roundNumber;                  
    private WaitForSeconds _startWait;        
    private WaitForSeconds _endWait;          
    private RoverManager _roundWinner;         
    private RoverManager _gameWinner;          

    private void Start()
    {        
        _startWait = new WaitForSeconds(_startDelay);
        _endWait = new WaitForSeconds(_endDelay);
        SpawnAllTanks();
        SetCameraTargets();        
        StartCoroutine(GameLoop());
    }
    private void SpawnAllTanks()
    {        
        for (int i = 0; i < _rovers.Length; i++)
        {            
            _rovers[i].Instance = Instantiate(_roverPrefab, _rovers[i].SpawnPoint.position, _rovers[i].SpawnPoint.rotation) as GameObject;
            _rovers[i].PlayerNumber = i + 1;
            _rovers[i].Setup();
        }
    }
    private void SetCameraTargets()
    {        
        Transform[] targets = new Transform[_rovers.Length];        
        for (int i = 0; i < targets.Length; i++)
        {           
            targets[i] = _rovers[i].Instance.transform;
        }        
        _cameraControl.m_Targets = targets;
    }    
    private IEnumerator GameLoop()
    {        
        yield return StartCoroutine(RoundStarting());        
        yield return StartCoroutine(RoundPlaying());        
        yield return StartCoroutine(RoundEnding());        
        if (_gameWinner != null)
        {            
            SceneManager.LoadScene(0);
        }
        else
        {           
            StartCoroutine(GameLoop());
        }
    }
    private IEnumerator RoundStarting()
    {        
        ResetAllTanks();
        DisableTankControl();        
        _cameraControl.SetStartPositionAndSize();        
        _roundNumber++;
        _messageText.text = "ÐÀÓÍÄ " + _roundNumber;       
        yield return _startWait;
    }
    private IEnumerator RoundPlaying()
    {        
        EnableTankControl();        
        _messageText.text = string.Empty;        
        while (!OneTankLeft())
        {            
            yield return null;
        }
    }
    private IEnumerator RoundEnding()
    {        
        DisableTankControl();        
        _roundWinner = null;        
        _roundWinner = GetRoundWinner();        
        if (_roundWinner != null)
            _roundWinner.Wins++;        
        _gameWinner = GetGameWinner();        
        string message = EndMessage();
        _messageText.text = message;        
        yield return _endWait;
    }    
    private bool OneTankLeft()
    {        
        int numTanksLeft = 0;        
        for (int i = 0; i < _rovers.Length; i++)
        {            
            if (_rovers[i].Instance.activeSelf)
                numTanksLeft++;
        }        
        return numTanksLeft <= 1;
    }    
    private RoverManager GetRoundWinner()
    {        
        for (int i = 0; i < _rovers.Length; i++)
        {            
            if (_rovers[i].Instance.activeSelf)
                return _rovers[i];
        }        
        return null;
    }    
    private RoverManager GetGameWinner()
    {        
        for (int i = 0; i < _rovers.Length; i++)
        {            
            if (_rovers[i].Wins == _numRoundsToWin)
                return _rovers[i];
        }        
        return null;
    }    
    private string EndMessage()
    {        
        string message = "ÍÈ×Üß!"; 
        if (_roundWinner != null)
            message = _roundWinner.ColoredPlayerText + " ÂÛÈÃÐÛÂÀÅÒ ÐÀÓÍÄ!";        
        message += "\n\n\n\n";        
        for (int i = 0; i < _rovers.Length; i++)
        {
            message += _rovers[i].ColoredPlayerText + ": " + _rovers[i].Wins + " ÂÛÈÃÐÛÂÀÅÒ\n"; 
        }        
        if (_gameWinner != null)
            message = _gameWinner.ColoredPlayerText + " ÂÛÈÃÐÛÂÀÅÒ ÈÃÐÓ!";
        return message;
    }    
    private void ResetAllTanks()
    {
        for (int i = 0; i < _rovers.Length; i++)
        {
            _rovers[i].Reset();
        }
    }
    private void EnableTankControl()
    {
        for (int i = 0; i < _rovers.Length; i++)
        {
            _rovers[i].EnableControl();
        }
    }
    private void DisableTankControl()
    {
        for (int i = 0; i < _rovers.Length; i++)
        {
            _rovers[i].DisableControl();
        }
    }
}
