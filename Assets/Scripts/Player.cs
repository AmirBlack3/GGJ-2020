using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerNo playerNo;

    public int speed;
    public int throwForce;
    public GameObject hand;

    public Text healthTxt;
    public Text ETxt;
    public Text FTxt;

    private Item item;
    private Desk desk;

    private Collider[] cols;
    private Rigidbody[] rigidbodies;
    private Quaternion[] rotations;
    private Vector3[] positions;

    private int health = 100;
    private bool isDashing;

    public enum PlayerNo
    {
        player_1,
        player_2
    }

    void Start()
    {
        ETxt.gameObject.SetActive(false);
        FTxt.gameObject.SetActive(false);

        cols = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        rotations = new Quaternion[rigidbodies.Length];
        positions = new Vector3[rigidbodies.Length];
    }
    void Update()
    {
        Move();
        CarryItem();
        RepairItem();
        ThrowItem();
        RepairSelf();
        //Dash();

        Txt();
    }
    void Move()
    {
        if (health == 100)
        {
            float lh = 0;
            float lv = 0;

            if (playerNo == PlayerNo.player_1)
            {
                lh = Input.GetAxis("Horizontal 1");
                lv = Input.GetAxis("Vertical 1");
            }
            else if(playerNo == PlayerNo.player_2)
            {
                lh = Input.GetAxis("Horizontal 2");
                lv = Input.GetAxis("Vertical 2");
            }

            var movement = new Vector3(lh * speed * Time.deltaTime, 0, lv * speed * Time.deltaTime);
            transform.position += movement;

            if (movement != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(movement);
        }
    }
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isDashing = true;

        if (isDashing == true)
            transform.position += Vector3.forward;
    }
    void CarryItem()
    {
        if (health == 100)
        {
            if (item != null)
            {
                if ((playerNo == PlayerNo.player_1 && Input.GetKeyDown(KeyCode.Joystick1Button0)) ||
                    (playerNo == PlayerNo.player_2 && Input.GetKeyDown(KeyCode.Joystick2Button0)))
                {
                    if (item.isCarrying == false)
                    {
                        item.myPlayer = this;
                        item.isCarrying = true;

                        if (desk != null)
                            desk.myItem = null;
                    }
                    else
                    {
                        item.myPlayer = null;
                        item.isCarrying = false;

                        if (desk != null)
                        {
                            item.transform.position = desk.top.transform.position;
                            desk.myItem = item;
                        }
                    }
                }
            }
        }
    }
    void RepairItem()
    {
        if (health == 100)
        {
            if (desk != null && desk.myItem != null)
            {
                if ((playerNo == PlayerNo.player_1 && Input.GetKey(KeyCode.Joystick1Button1)) ||
                    (playerNo == PlayerNo.player_2 && Input.GetKey(KeyCode.Joystick2Button1)))
                {
                    if (desk.myItem.gameObject == desk.correctItem)
                    {
                        desk.myItem.repairPercent++;
                    }
                }
            }
        }
    }
    void ThrowItem()
    {
        if (health == 100)
        {
            if (item != null && item.isCarrying)
                if ((playerNo == PlayerNo.player_1 && Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
                    (playerNo == PlayerNo.player_2 && Input.GetKeyDown(KeyCode.Joystick2Button2)))
                {
                    item.myPlayer = null;
                    item.isCarrying = false;
                    item.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
                    item.isThrowing = true;
                }
        }
    }
    void RepairSelf()
    {
        if (health < 100)
        {
            if (Input.GetKey(KeyCode.F))
            {
                health++;
                if (health == 100)
                    HealthState(true);
            }
        }
    }
    void Txt()
    {
        if (health < 100)
            FTxt.gameObject.SetActive(true);
        else
        {
            if (item != null)
            {
                if (item.isCarrying == false || desk != null)
                    ETxt.gameObject.SetActive(true);
                else
                    ETxt.gameObject.SetActive(false);
            }
            else
                ETxt.gameObject.SetActive(false);

            if (desk != null)
            {
                if (desk.myItem != null)
                    FTxt.gameObject.SetActive(true);
                else
                    FTxt.gameObject.SetActive(false);
            }
            else
                FTxt.gameObject.SetActive(false);
        }

        healthTxt.text = health + "";

        ETxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        FTxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        healthTxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Desk")
            desk = other.gameObject.GetComponent<Desk>();

        if (other.gameObject.tag == "Item")
            item = other.gameObject.GetComponent<Item>();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Desk")
            desk = null;

        if (other.gameObject.tag == "Item")
            item = null;
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Item")
        {
            if (col.gameObject.GetComponent<Item>().isThrowing == true)
            {
                HealthState(false);
                health = 0;
            }
        }
    }
    void HealthState(bool isHealthy)
    {
        rigidbodies[0].isKinematic = !isHealthy;
        cols[0].enabled = isHealthy;

        for (int i = 1; i < rigidbodies.Length; i++)
        {
            if (isHealthy)
            {
                rigidbodies[i].gameObject.transform.position = positions[i];
                rigidbodies[i].gameObject.transform.rotation = rotations[i];
            }
            else
            {
                rotations[i] = rigidbodies[i].gameObject.transform.rotation;
                positions[i] = rigidbodies[i].gameObject.transform.position;
            }

            rigidbodies[i].isKinematic = isHealthy;
            cols[i].enabled = !isHealthy;
        }
    }
}
