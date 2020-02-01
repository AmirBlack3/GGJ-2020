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
    public Text ATxt;
    public Text BTxt;
    public Text XTxt;
    public Text YTxt;

    private Item item;
    private Desk desk;
    private Player player;
    private Item myItem;

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
        ATxt.gameObject.SetActive(false);
        BTxt.gameObject.SetActive(false);
        XTxt.gameObject.SetActive(false);
        YTxt.gameObject.SetActive(false);

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
        RepairBot(this);
        RepairBot(player);
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

                if (lh == 0)
                    lh = Input.GetAxis("Horizontal");
                if (lv == 0)
                    lv = Input.GetAxis("Vertical");
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
                    (playerNo == PlayerNo.player_2 && (Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.C))))
                {
                    if (myItem == null)
                    {
                        if (item.isCarrying == false)
                        {
                            myItem = item;
                            myItem.myPlayer = this;
                            myItem.isCarrying = true;

                            if (desk != null)
                                desk.myItem = null;
                        }
                    }
                    else
                    {
                        myItem.myPlayer = null;
                        myItem.isCarrying = false;

                        if (desk != null)
                        {
                            myItem.transform.position = desk.top.transform.position;
                            desk.myItem = myItem;
                        }
                        myItem = null;
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
                    (playerNo == PlayerNo.player_2 && (Input.GetKey(KeyCode.Joystick2Button1)) || Input.GetKeyDown(KeyCode.F)))
                {
                    if (desk.myItem.types[desk.myItem.typeIndex] == desk.correctItemType)
                    {
                        if (desk.myItem.repairPercent < 100)
                            desk.myItem.repairPercent++;
                        else
                        {
                            if (desk.myItem.typeIndex < desk.myItem.types.Length - 1)
                            {
                                desk.myItem.repairPercent = 0;
                                desk.myItem.typeIndex++;
                            }
                            else
                            {
                                desk.GetComponent<MeshRenderer>().material = GameManager.instance.greenMat;
                                Destroy(desk.myItem.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
    void ThrowItem()
    {
        if (health == 100)
        {
            if (myItem != null && myItem.isCarrying)
                if ((playerNo == PlayerNo.player_1 && Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
                    (playerNo == PlayerNo.player_2 && (Input.GetKeyDown(KeyCode.Joystick2Button2) || Input.GetKeyDown(KeyCode.E))))
                {
                    myItem.myPlayer = null;
                    myItem.isCarrying = false;
                    myItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
                    myItem.isThrowing = true;
                    myItem = null;
                }
        }
    }
    void RepairBot(Player p)
    {
        if (p != null)
        {
            if (p.health < 100)
            {
                if ((playerNo == PlayerNo.player_1 && Input.GetKey(KeyCode.Joystick1Button3)) ||
                    (playerNo == PlayerNo.player_2 && Input.GetKey(KeyCode.Joystick2Button3)))
                {
                    p.health++;
                    if (p.health == 100)
                        p.HealthState(true);
                }
            }
        }
    }
    void Txt()
    {
        if (item == null || health < 100 || (player != null && player.health < 100))
        {
            ATxt.gameObject.SetActive(false);
            BTxt.gameObject.SetActive(false);
            XTxt.gameObject.SetActive(false);
        }
        else
        {
            if (desk == null)
            {
                if (myItem == null)
                {
                    if (item.isCarrying == false)
                    {
                        ATxt.gameObject.SetActive(true);
                        XTxt.gameObject.SetActive(false);
                    }
                }
                else
                {
                    ATxt.gameObject.SetActive(false);
                    XTxt.gameObject.SetActive(true);
                }
            }
            else
            {
                XTxt.gameObject.SetActive(false);

                if (desk.myItem != null && desk.myItem.types[desk.myItem.typeIndex] == desk.correctItemType)
                {
                    ATxt.gameObject.SetActive(false);
                    BTxt.gameObject.SetActive(true);
                }
                else
                {
                    ATxt.gameObject.SetActive(true);
                    BTxt.gameObject.SetActive(false);
                }
            }
        }
        
        if (health < 100 || (player != null && player.health < 100))
            YTxt.gameObject.SetActive(true);
        else
            YTxt.gameObject.SetActive(false);

        healthTxt.text = health + "";

        ATxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y+7, transform.position.z));
        BTxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y+7, transform.position.z));
        XTxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y+7, transform.position.z));
        YTxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y+7, transform.position.z));
        healthTxt.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Desk")
            desk = other.gameObject.GetComponent<Desk>();

        if (other.gameObject.tag == "Item")
            item = other.gameObject.GetComponent<Item>();

        if (other.gameObject.tag == "Player")
            player = other.gameObject.GetComponent<Player>();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Desk")
            desk = null;

        if (other.gameObject.tag == "Item")
            item = null;

        if (other.gameObject.tag == "Player")
            player = null;
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Item")
        {
            var i = col.gameObject.GetComponent<Item>();
            if (i.isThrowing == true)
            {
                HealthState(false);
                health = 0;
                i.isThrowing = false;
            }
        }
    }
    void HealthState(bool isHealthy)
    {
        rigidbodies[0].isKinematic = !isHealthy;
        cols[0].isTrigger = !isHealthy;

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
