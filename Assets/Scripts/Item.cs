using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemType[] types;
    public Text repairTxt;

    public Player myPlayer { get; set; }

    public int typeIndex { get; set; }
    public int repairPercent { get; set; }

    public bool isCarrying { get; set; }
    public bool isThrowing { get; set; }

    private Rigidbody myRigidbody;
    private MeshFilter myMeshFilter;

    public enum ItemType
    {
        salem,
        jush,
        chakosh,
        areh,
        //jush_areh,
        //jush_chakosh,

        gear_salem,
        gear_bokhar,
        gear_chakosh,
        //gear_chakosh_bokhar
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myMeshFilter = GetComponent<MeshFilter>();
    }
    void Update()
    {
        Carrying();

        Txt();

        Shape();
    }
    void Carrying()
    {
        if (isCarrying == true)
        {
            transform.position = myPlayer.hand.transform.position;
            myRigidbody.useGravity = false;
        }
        else
            myRigidbody.useGravity = true;
    }
    void Txt()
    {
        repairTxt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        repairTxt.text = repairPercent + "";
    }
    void Shape()
    {
        if (types[typeIndex] == ItemType.salem)
        {
            myMeshFilter.mesh = GameManager.instance.itemMeshs[0];
        }
        else if (types[typeIndex] == ItemType.jush)
        {
            if (types[typeIndex + 1] == ItemType.salem)
                myMeshFilter.mesh = GameManager.instance.itemMeshs[1];
            else if (types[typeIndex + 1] == ItemType.chakosh)
                myMeshFilter.mesh = GameManager.instance.itemMeshs[4];
        }
        else if (types[typeIndex] == ItemType.chakosh)
        {
            myMeshFilter.mesh = GameManager.instance.itemMeshs[2];
        }
        else if (types[typeIndex] == ItemType.areh)
        {
            myMeshFilter.mesh = GameManager.instance.itemMeshs[3];
        }

        if (types[typeIndex] == ItemType.gear_salem)
        {
            myMeshFilter.mesh = GameManager.instance.itemMeshs[5];
        }
        else if (types[typeIndex] == ItemType.gear_bokhar)
        {
            myMeshFilter.mesh = GameManager.instance.itemMeshs[6];
        }
        else if (types[typeIndex] == ItemType.gear_chakosh)
        {
            myMeshFilter.mesh = GameManager.instance.itemMeshs[7];
        }
    }
    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag == "Desk")
        //    myRigidbody.isKinematic = true;

        if (col.gameObject.tag == "Plane")
            isThrowing = false;
    }
    void OnCollisionExit(Collision col)
    {
        //if (col.gameObject.tag == "Desk")
        //    myRigidbody.isKinematic = false;
    }
}
