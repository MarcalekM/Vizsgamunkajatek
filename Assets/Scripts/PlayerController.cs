using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

public class PlayerController : MonoBehaviour
{
    public float Lv = 1;
    [SerializeField] public float MaxHP = 50;
    [SerializeField] public float HP = 25;
    [SerializeField] public float MeeleDamage = 10;
    [SerializeField] public float MagicDamage = 12;
    public float kills = 0;
    public float SP = 3;

    [SerializeField] public bool PlayerSpotted = false;
    
    [SerializeField] private float defaultSpeed = 6f;
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private float movementSpeed = 6f;
    bool isFacingRight = true;
    [SerializeField] float jumpPower = 25f;
    protected Collider2D MainCollider;
    public bool canGoToNextLevel = false;

    bool isJumping
    {
        get
        {
            var lenghtToFloor = (MainCollider.bounds.size.y / 2) + 0.5f;
            //Debug.DrawRay(transform.position, Vector2.down * lenghtToFloor, Color.green, 1, false);
            var hit = Physics2D.Raycast(
                transform.position,
                Vector2.down,
                lenghtToFloor,
                LayerMask.GetMask("Ground", "Enemy"));
            if (hit) return false;
            return true;
        }
    }

    [SerializeField] private Transform Magic;
    [SerializeField] private GameObject fireball;
    [SerializeField] private float fireRate = .5f;

    Rigidbody2D rb;
    public Animator animator;
    private float nextFire;

    [SerializeField] Transform Shield;
    public float MaxShield = 10;
    public float ShieldHP;
    public bool ShieldActive = false;
    public bool ShieldAlive = true;

    //[SerializeField] TextMeshProUGUI UI_HP;
    //[SerializeField] TextMeshProUGUI UI_Kill;
    //[SerializeField] TextMeshProUGUI UI_SP;

    [SerializeField] AudioSource Walk;

    [SerializeField] ParticleSystem Flamethrower;
    [SerializeField] GameObject FlamethrowerHitbox;
    private bool FlamethrowerActived = false;
    private float horizontalMovementDirection = 0f;
    private float rawInputDirection = 0f;
    private bool isRunning = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //LoadCharacterStats();
        ShieldHP = MaxShield;
        nextFire = 0f;
        MainCollider = GetComponent<Collider2D>();
        FlamethrowerInactive();
        // így lehet menteni
        // Menu_UI_Manager.UserData.json_save = "{\"asd\": 1}";
        // Menu_UI_Manager.SaveUserToDB(this);
    }

    private void Update()
    {
        //UI_HP.text = "HP: " + HP;
        //UI_Kill.text = "Kills: " + kills;
        //UI_SP.text = "SP: " + SP;
        animator.SetFloat("VerticalSpeed", rb.velocity.y);
        
        if (!isJumping) animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        else animator.SetFloat("Speed", 0);
        
        if (!ShieldAlive) DeactivateShield();

        if(HP <= 0)
        {
            gameObject.SetActive(false);
        }

        if(isJumping) FlamethrowerInactive();
        
       HandleMovement();
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(horizontalMovementDirection * movementSpeed, rb.velocity.y);
        if (rawInputDirection > 0.5f || rawInputDirection < -0.5f || isRunning) movementSpeed = runningSpeed;
        else movementSpeed = defaultSpeed;
    }

    private void FixedUpdate()
    {
        if (!ShieldAlive) AddShieldHP();
        if (ShieldAlive && !ShieldActive && ShieldHP != MaxShield) AddShieldHP();
        if(HP < MaxHP && !PlayerSpotted) AddHP();
    }

    void FlipCharacter(bool right)
    {
        isFacingRight = right;
        Vector3 ls = transform.localScale;
        if (Math.Abs(ls.x) > ls.x && isFacingRight) ls.x *= -1;
        else if (Mathf.Approximately(Math.Abs(ls.x), ls.x) && !isFacingRight) ls.x *= -1;
        transform.localScale = ls;
    }
    
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isJumping)
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    public void OnHorizontalMove(InputAction.CallbackContext ctx)
    {
        if (ctx.started) Walk.Play();
        else if (ctx.performed || ctx.canceled) Walk.Stop();
        rawInputDirection = ctx.ReadValue<float>();
        // analóg érték konvertálása digitálissá
        if (rawInputDirection > 0.15f) horizontalMovementDirection = 1;
        else if (rawInputDirection < -0.15f) horizontalMovementDirection = -1;
        else horizontalMovementDirection = 0;
        if (horizontalMovementDirection > 0.15f) FlipCharacter(true);
        else if (horizontalMovementDirection < -0.15f) FlipCharacter(false);
    }

    public void OnRun(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled || ctx.performed) isRunning = false;
        else if(ctx.started) isRunning = true;
    }
    public void OnMeeleAttack(InputAction.CallbackContext ctx)
    {
        if (!ShieldActive && !FlamethrowerActived && ctx.started)
            animator.SetTrigger("MeeleAttack");
    }

    public void OnSummonFireball(InputAction.CallbackContext ctx)
    {
        if (Time.time >= nextFire && !ShieldActive && !FlamethrowerActived)
        {
            nextFire = Time.time + fireRate;
            Instantiate(fireball, Magic.position,
                Quaternion.Euler(x: 0, y: 0, z: isFacingRight ? 0 : 180));
        }
    }

    public void OnMenu(InputAction.CallbackContext ctx)
    {
        FindObjectOfType<UI_ManagerScript>().OpenSubMenu();
    }

    public void OnActivateShield(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled || ctx.performed) DeactivateShield();
        else if(ctx.started) ActivateShield();
    }
    void ActivateShield()
    {
        if (!FlamethrowerActived && ShieldAlive)
        {
            Shield.gameObject.SetActive(true);
            ShieldActive = true;
            animator.SetBool("ShieldActive", true);
        }
    }

    void DeactivateShield()
    {
        Shield.gameObject.SetActive(false);
        ShieldActive = false;
        animator.SetBool("ShieldActive", false);
    }

    void AddShieldHP()
    {
        if (ShieldHP + MagicDamage / 100 > MaxShield) ShieldHP = MaxShield;
        else ShieldHP += MagicDamage / 100;
        if (ShieldHP == MaxShield) ShieldAlive = true;
    }
    void AddHP()
    {
        if (HP + MeeleDamage / 100 > MaxHP) HP = MaxHP;
        else HP += MeeleDamage / 100;
    }

    public void GetDamage(float damage)
    {
        if (HP - damage > 0) HP -= damage;
        else HP = 0;
    }

    public void OnFlameThrowerActive(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled || ctx.performed) FlamethrowerInactive();
        else if (ctx.started) FlamethrowerActive();
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started && canGoToNextLevel)
        {
            var lvlMove = FindObjectOfType<LevelMove>();
            lvlMove?.StartCoroutine(lvlMove.NextSceneTransform());
        }
    }
    public void FlamethrowerActive()
    {
        if (!ShieldActive && !isJumping)
        {
            Flamethrower.Play();
            FlamethrowerHitbox.SetActive(true);
            FlamethrowerActived = true;
            animator.SetBool("FlamethrowerActive", true);
        }
    }

    public void FlamethrowerInactive()
    {
        Flamethrower.Stop();
        FlamethrowerHitbox.SetActive(false);
        FlamethrowerActived = false;
        animator.SetBool("FlamethrowerActive", false);
    }

    /*void LoadCharacterStats()
    {
        string filepath = Application.persistentDataPath + "/stats.txt";
        using StreamReader sr = new(
            path: filepath,
            encoding: System.Text.Encoding.UTF8);
        string[] text = sr.ReadLine().Split(',');
        Lv = float.Parse(text[0]);
        HP = float.Parse(text[1]);
        MeeleDamage = float.Parse(text[2]);
        MagicDamage = float.Parse(text[3]);
        kills = float.Parse(text[4]);
        SP = float.Parse(text[5]);
        MaxShield = float.Parse(text[6]);
        SaveToJson();
    }*/

    public class User
    {
        public string Name;
        public string Password;
    }

    public void SaveToJson()
    {
        User newbie = new();
        newbie.Name = "Sufi";
        newbie.Password = "12345";
        User vki = new();
        vki.Name = "Laci";
        vki.Password = "67890";
        string text = "";
        text += JsonUtility.ToJson(newbie);
        text += JsonUtility.ToJson(vki);
        using StreamWriter sw = new(
            path: @"Assets/src/Users.json",
            append: false);
        sw.Write(text);
    }
}
