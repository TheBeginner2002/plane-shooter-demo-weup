using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreDisplay;

    private PlayerManager _player;

    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreDisplay.text = "Score: " + _player.Point.ToString();
    }
}
