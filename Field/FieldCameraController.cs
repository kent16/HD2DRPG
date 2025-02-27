using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCameraController : MonoBehaviour
{
    // オフセット
    [SerializeField] private Vector3 offset;

    // フィールド上の味方キャラクター
    private GameObject ally;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(ally != null)
        {
            transform.position = ally.transform.position + offset;
        }
        else if(GameObject.FindWithTag("Player") != null)
        {
            ally = GameObject.FindWithTag("Player");
        }
    }
}
