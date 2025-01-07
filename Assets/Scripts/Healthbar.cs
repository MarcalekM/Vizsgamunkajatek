using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{ 
    [SerializeField] private PlayerController player;
    [SerializeField] private RectTransform _barRect;
    [SerializeField] private RectMask2D _mask;

    private float _maxRightMask;
    private float _initialRightMask;

    void Start()
    {
        _maxRightMask = 200;
        _initialRightMask = 0;
        Debug.Log(player.MaxHp);
    }

    void Update(){
        _mask.padding = new Vector4(100, 0, 200 - (_maxRightMask / (player.MaxHp - player.HP)), 0) ;
    }
}
