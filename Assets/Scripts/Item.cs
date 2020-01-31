using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemType[] type;
    public Text repairTxt;

    public Player myPlayer { get; set; }

    public int typeIndex { get; set; }
    public int repairPercent { get; set; }

    public bool isCarrying { get; set; }
    public bool isThrowing { get; set; }

    private Rigidbody myRigidbody;
    private MeshRenderer myMeshRenderer;

    public enum ItemType
    {
        item_1,
        item_2,
        item_3
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myMeshRenderer = GetComponent<MeshRenderer>();
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
        myMeshRenderer.material = GameManager.instance.itemMaterials[typeIndex];
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
