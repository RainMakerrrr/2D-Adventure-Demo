using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PlayerController;
using UnityEngine;
using Cinemachine;

public class HouseEnter : MonoBehaviour
{
    [SerializeField] private GameObject _house;
    private CinemachineBrain _cameraBrain;
    public Vector3 PlayerPosition { get; private set; }

    private void Start() => _cameraBrain = FindObjectOfType<CinemachineBrain>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            PlayerPosition = player.transform.position;
            player.transform.position = _house.transform.position;
            player.InHouse = true;

            _cameraBrain.enabled = false;
            Camera.main.orthographicSize = 1.47f;
            Camera.main.transform.position = new Vector3(76.6f,-28.88f, -10f);
        }
    }
    
    

}
