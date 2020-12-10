using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PlayerController;
using UnityEngine;

public class HouseExit : MonoBehaviour
{
    private HouseEnter _enter;
    private CinemachineBrain _cameraBrain;

    private void Start()
    {
        _enter = FindObjectOfType<HouseEnter>();
        _cameraBrain = FindObjectOfType<CinemachineBrain>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            player.transform.position = _enter.PlayerPosition + new Vector3(0f, -1f, 0f);
            player.InHouse = false;

            _cameraBrain.enabled = true;

        }
    }
    
}
