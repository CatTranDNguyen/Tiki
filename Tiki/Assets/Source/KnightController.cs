using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public partial class KnightController : MonoBehaviour
{
    public int playerNumer = 0;
    float gravity = 8;
    public int speed = 3;
    public int num_Bomb = 2;
    public int num_Length = 2;
    public int live = 2;
    public int count_bomb = 0;

    public Rigidbody Bomb;
    public Text liveText;

    Vector3 moveDir = Vector3.zero;

    CharacterController controller;
    Animator animator;
    
    private Rigidbody rb;

    const KeyCode PL0_Up = KeyCode.W;
    const KeyCode PL0_Down = KeyCode.S;
    const KeyCode PL0_Left = KeyCode.A;
    const KeyCode PL0_Right = KeyCode.D;
    const KeyCode PL0_Fire = KeyCode.LeftControl;



    const KeyCode PL1_Up = KeyCode.UpArrow;
    const KeyCode PL1_Down = KeyCode.DownArrow;
    const KeyCode PL1_Left = KeyCode.LeftArrow;
    const KeyCode PL1_Right = KeyCode.RightArrow;
    const KeyCode PL1_Fire = KeyCode.Space;

    private const int MAX_BOMB_NUM = 8;
    private const int MAX_SPEED = 8;
    private const int MAX_LENGTH = 8;

    private int posX = 0;
    private int posZ = 0;

    public GameObject plane;
    public Text DieText;
    public Text CoundownText;

    Map map;
    bool active = true;
    private AudioSource collectSource;

    bool StartGame = false;
    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        map = plane.GetComponent<Map>();
        liveText.text = live.ToString();
        updatePos();
        DieText.enabled = false;
        CoundownText.enabled = false;
        collectSource = GetComponent<AudioSource>();

    }
    int k = 1;

    void updatePos()
    {
        posX = intRound(transform.position.x);
        posZ = intRound(transform.position.z);

    }


    void KnightMoveUp()
    {
        //adjustPosX();
        animator.SetInteger("condition", 1);
        moveDir = new Vector3(0, 0, 1);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        moveDir *= speed;
        moveDir = transform.TransformDirection(moveDir);
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
        k = 1;
        updatePos();
    }

    void KnightMoveDown()
    {
        animator.SetInteger("condition", 1);
        // adjustPosX();
        moveDir = new Vector3(0, 0, 1);
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        moveDir *= speed;
        moveDir = transform.TransformDirection(moveDir);
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
        k = -1;
        updatePos();
    }

    void KnightMoveLeft()
    {
        animator.SetInteger("condition", 1); //chuyển animation
        //adjustPosZ();
        moveDir = new Vector3(0, 0, 1); //khoi tao mot vecto don vi
        transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0)); //gpc quay alpha sang trai
        moveDir *= speed;
        moveDir = transform.TransformDirection(moveDir); //quay vecto don vi mot goc alpha
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
        k = -1;
        updatePos();
    }

    void KnightMoveRight()
    {
        animator.SetInteger("condition", 1);
        //adjustPosZ();
        transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        moveDir = new Vector3(0, 0, 1);
        moveDir *= speed;
        moveDir = transform.TransformDirection(moveDir);
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
        k = 1;
        updatePos();
    }

    bool isUp()
    {
        if (playerNumer == 0)
            return Input.GetKey(PL0_Up);
        else
            return Input.GetKey(PL1_Up);
    }

    bool isGetUpKey()
    {
        if (playerNumer == 0)
            return Input.GetKeyUp(PL0_Up) || Input.GetKeyUp(PL0_Down) || Input.GetKeyUp(PL0_Left) || Input.GetKeyUp(PL0_Right);
        else
            return Input.GetKeyUp(PL1_Up) || Input.GetKeyUp(PL1_Down) || Input.GetKeyUp(PL1_Left) || Input.GetKeyUp(PL1_Right);

    }

    bool isDown()
    {
        if (playerNumer == 0)
            return Input.GetKey(PL0_Down);
        else
            return Input.GetKey(PL1_Down);
    }

    bool isLeft()
    {
        if (playerNumer == 0)
            return Input.GetKey(PL0_Left);
        else
            return Input.GetKey(PL1_Left);
    }

    bool isRight()
    {
        if (playerNumer == 0)
            return Input.GetKey(PL0_Right);
        else
            return Input.GetKey(PL1_Right);
    }

    private bool isFired()
    {
        if (playerNumer == 0)
            return Input.GetKeyDown(PL0_Fire);
        else
            return Input.GetKeyDown(PL1_Fire);
    }

    void KnightStopMove()
    {
        animator.SetInteger("condition", 0);
        moveDir = Vector3.zero;

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
        if (!isInteger(transform.position.x) || !isInteger(transform.position.z))
        {
            adjustPosZ();
            adjustPosX();
        }
        updatePos();

    }


    // Update is called once per frame
    float countdown = 5f;
    void Update()
    {
        if (!StartGame)
            return;
        if (!active)
        {
            CoundownText.enabled = true;
            countdown -= Time.deltaTime;
            if (countdown <= 0)
                countdown = 0;
            CoundownText.text = countdown.ToString("0");

            return;
        }
        CoundownText.enabled = false;
        int oldX = posX, oldZ = posZ;

        if (controller.isGrounded)
        {
            bool up, down, left, right, fired, upkey;
            up = isUp();
            down = isDown();
            left = isLeft();
            right = isRight();
            fired = isFired();
            upkey = isGetUpKey();

            if (up) KnightMoveUp();
            else if (down) KnightMoveDown();
            else if (left) KnightMoveLeft();
            else if (right) KnightMoveRight();
            else if (upkey) KnightStopMove();

            if (fired) KnightFire();




        }
        if ((posZ != oldZ || posX != oldX) && !map.isEmpty(posX, posZ))
        {
            Debug.Log("halo" + posX + " " + posZ + " old " + oldX + " " + oldZ);
            transform.position = new Vector3(oldX, 0, oldZ);
            posX = oldX; posZ = oldZ;
        }

    }

    private void KnightFire()
    {
        if (count_bomb < num_Bomb) //nếu số lượng bom đã đặt chưa vượt quá số bom cho phép
        {
            int tmpx = (int)Math.Round(transform.position.x);
            int tmpz = (int)Math.Round(transform.position.z);
            if (map.isOKToSpawnBomb(tmpx, tmpz)) //nếu vị trí đó có thể đặt bom
            {
                // tao bom
                Rigidbody bombInstance =
                                 Instantiate(Bomb, new Vector3(tmpx, 0.75f, tmpz), Quaternion.Euler(new Vector3(-15, 0, 20))) as Rigidbody; //Đặt bom
                count_bomb++; //tang so luong bom duoc dat
                Bomb bomb = bombInstance.GetComponent<Bomb>(); 
                // đặt các thuộc tính cho object Bomb vừa tạo
                bomb.length_bomb = num_Length;
                bomb.map = map;
                bomb.knight = this;
            }

        }

    }

    bool isInteger(float x)
    {
        return Mathf.Abs(Mathf.Round(x) - x) <= 0.00001f;
    }
    private int intRound(float x)
    {
        return (int)Mathf.Round(x);
    }
    private float Round(float x)
    {
        if (isInteger(x))
            return x;
        if (k == 1)
        {
            if (x < 0)
            {
                x = -(float)Math.Floor(-x);
                return x;
            }
            else
            {
                return (float)Math.Floor(x + 0.7);
            }

        }
        else
        {
            if (x < 0)
            {
                return -(float)Math.Floor(-x + 0.7);
            }
            else
            {
                return (float)Math.Floor(x);
            }
        }

    }
    private void adjustPosX()
    {
        Vector3 newPos = new Vector3(Round(transform.position.x), transform.position.y, transform.position.z);
        transform.position = newPos;
    }
    private void adjustPosZ()
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, Round(transform.position.z));
        transform.position = newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active)
            return;

        if (other.gameObject.CompareTag("item_Bomb"))
        {
            collectSource.Play();
            other.gameObject.SetActive(false);
            if (num_Bomb < MAX_BOMB_NUM)
                num_Bomb++;
        }

        if (other.gameObject.CompareTag("item_LengthBomb"))
        {
            collectSource.Play();
            other.gameObject.SetActive(false);
            if (num_Length < MAX_LENGTH)
                num_Length++;
        }

        if (other.gameObject.CompareTag("item_Speed"))
        {
            collectSource.Play();
            other.gameObject.SetActive(false);
            if (speed < MAX_SPEED)
                speed++;
        }

        if (other.gameObject.CompareTag("item_Live"))
        {
            collectSource.Play();
            other.gameObject.SetActive(false);
            live++;
            liveText.text = live.ToString();
        }

        if ( other.gameObject.CompareTag("explose"))
        {
            active = false;
            live--;
            liveText.text = live.ToString();
            SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            render.enabled = false;
            StartCoroutine(LateCall());
            Debug.Log("DIE");
            DieText.enabled = true;
            DieText.GetComponent<Animator>().SetBool("start", true);

        }

    }

    IEnumerator LateCall()
    {
        Vector2 newpos = map.RandomEmptyPosition();
        transform.position = new Vector3(newpos.x, 0.0f, newpos.y);
        Debug.Log(newpos);
        yield return new WaitForSeconds(5);
        SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        render.enabled = true;
        DieText.enabled = false;
        active = true;
        countdown = 5f;
        DieText.GetComponent<Animator>().SetBool("start", false);

    }

    public bool isDead()
    {
        return live == 0;
    }

    public int CalRes(KnightController other)
    {
        if (live == other.live)
            return 0;
        if (live < other.live)
            return -1;
        return 1;
    }

    public void YouCanStart()
    {
        StartGame = true;
    }
}
