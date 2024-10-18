using System;
using UnityEngine;

[Serializable]
public class RoverManager
{
    public Color PlayerColor;                             
    public Transform SpawnPoint;                          
    [HideInInspector] public int PlayerNumber;            
    [HideInInspector] public string ColoredPlayerText;    
    [HideInInspector] public GameObject Instance;         
    [HideInInspector] public int Wins;                    

    private RoverMovement _movement;                        
    private RoverShooting _shooting;                        
    private GameObject _canvasGameObject;                  

    public void Setup()
    {        
        _movement = Instance.GetComponent<RoverMovement>();
        _shooting = Instance.GetComponent<RoverShooting>();
        _canvasGameObject = Instance.GetComponentInChildren<Canvas>().gameObject;        
        _movement.PlayerNumber = PlayerNumber;
        _shooting.PlayerNumber = PlayerNumber;        
        ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerColor) + ">»√–Œ  " + PlayerNumber + "</color>";       
        MeshRenderer[] renderers = Instance.GetComponentsInChildren<MeshRenderer>();
        renderers[0].materials[1].color = PlayerColor;         
    }   
    public void DisableControl()
    {
        _movement.enabled = false;
        _shooting.enabled = false;
        _canvasGameObject.SetActive(false);
    }    
    public void EnableControl()
    {
        _movement.enabled = true;
        _shooting.enabled = true;
        _canvasGameObject.SetActive(true);
    }    
    public void Reset()
    {
        Instance.transform.position = SpawnPoint.position;
        Instance.transform.rotation = SpawnPoint.rotation;
        Instance.SetActive(false);
        Instance.SetActive(true);
    }
}
