using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Player myPlayer { get; set; }
    public GameObject pos { get; set; }
    public bool isCarrying { get; set; }
    public bool onDesk { get; set; }

    void Start()
    {
    }
    void Update()
    {
        Carrying();
    }
    void Carrying()
    {
        if (pos != null)
            transform.position = pos.transform.position;
    }
}
