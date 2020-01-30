using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int speed;
    public Text interactTxt;
    public GameObject hand;

    private Item myItem;
    private bool isDashing;

    void Start()
    {
        interactTxt.gameObject.SetActive(false);
    }
    void Update()
    {
        Move();

        Dash();

        interactTxt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
    void Move()
    {
        var lh = Input.GetAxis("Horizontal");
        var lv = Input.GetAxis("Vertical");
        var movement = new Vector3(lh * speed * Time.deltaTime, 0, lv * speed * Time.deltaTime);
        transform.position += movement;

        if (movement != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(movement);
    }
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isDashing = true;

        if (isDashing == true)
            transform.position += Vector3.forward;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Desk" || other.gameObject.tag == "Item")
            interactTxt.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.gameObject.tag == "Desk")
            {
                if (myItem != null)
                {
                    myItem.pos = other.gameObject.GetComponent<Desk>().top;
                    //myItem = null;
                }
            }

            if (other.gameObject.tag == "Item")
            {
                if (myItem == null)
                {
                    myItem = other.gameObject.GetComponent<Item>();
                    myItem.pos = hand;
                }
                else
                {
                    if (myItem.pos == hand)
                        myItem.pos = null;
                    myItem = null;
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item" || other.gameObject.tag == "Desk")
            interactTxt.gameObject.SetActive(false);
    }
}
