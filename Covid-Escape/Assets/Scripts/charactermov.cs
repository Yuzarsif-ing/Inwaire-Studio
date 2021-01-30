using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class charactermov : MonoBehaviour
{
    public float speed = 1.5f;
    public GameObject Cameraa;
    Vector3 camfirstpos;
    Rigidbody2D rbphysic;
    public Animator animtorr;
    AudioSource SoundEffect;
    public AudioClip[] Swordeffects;
    SpriteRenderer sprender;
    bool onetimejump = true;
    bool onetimedamage = false;
    bool onetimedeath = true;
    public bool underattack,underattack2 = false;
    public bool onetimeunderattack;
    public Slider healthbar, staminabar;
    GameObject monsters;
    float attacktimer=0;
    public Camerashake camshake;
    void Start()
    {
        sprender = GetComponent<SpriteRenderer>();
        rbphysic = GetComponent<Rigidbody2D>();
        animtorr = GetComponent<Animator>();
        camfirstpos = Cameraa.transform.position - transform.position;
        SoundEffect = transform.GetChild(3).GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onetimejump == true)
            { rbphysic.AddForce(new Vector2(0, 400f)); }
        }
        animation();

        attacktimer += Time.deltaTime;
        if (staminabar.value <= 100)
        { staminabar.value += Time.deltaTime*7; }

        if(healthbar.value<=0&&onetimedeath)
        { animtorr.SetBool("death", true); onetimedeath = false; }
    }

    void FixedUpdate()
    {
        movement();
        
    }

    void LateUpdate()
    {
        cameramotion();
    }

    void animation()
    {
        

        if (Input.GetAxisRaw("Horizontal") != 0)
        { animtorr.SetBool("run", true); }
        else
        { animtorr.SetBool("run", false); }

        attack();

        if (Input.GetButtonDown("Jump"))
        {
            animtorr.SetTrigger("jump");
            animtorr.SetBool("isground", false);
            onetimejump = false;
        }
        else if (rbphysic.velocity.y < 0)
        { animtorr.SetTrigger("fall");
            if (rbphysic.velocity.y < -1f)
            { animtorr.SetBool("isground", false); }
        }
    }
    void movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        //transform.position += new Vector3(x * speed * Time.deltaTime,0,transform.position.z);
        rbphysic.velocity = new Vector2(x*Time.deltaTime*speed*10,rbphysic.velocity.y);
        if(Input.GetAxisRaw("Horizontal") < 0)
        { transform.localScale = new Vector3(-3, 3, -3); }
        else if(Input.GetAxisRaw("Horizontal") > 0)
        { transform.localScale = new Vector3(3, 3, 3); }

    }

    void cameramotion()
    {
        Cameraa.transform.position = Vector3.Lerp(Cameraa.transform.position, (camfirstpos + transform.position), 1.6f*Time.deltaTime);
    
    }

    void attack()
    {

        if (onetimejump == true && rbphysic.velocity.y > -3f)
        {
            if (attacktimer > 0.4f)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (staminabar.value >= 15f)
                    {
                        animtorr.SetTrigger("attack1"); transform.GetChild(0).gameObject.SetActive(true);
                        SoundEffect.clip = Swordeffects[0]; SoundEffect.Play(); staminabar.value -= 15f;
                        onetimedamage = true; attacktimer = 0; StartCoroutine(camshake.Shake(.15f, .4f));
                    }
                }
                else if (Input.GetButtonDown("Fire2"))
                {
                    if (staminabar.value >= 30f)
                    {
                        animtorr.SetTrigger("attack2"); transform.GetChild(1).gameObject.SetActive(true);
                        SoundEffect.clip = Swordeffects[1]; SoundEffect.Play(); staminabar.value -= 30f;
                        onetimedamage = true; attacktimer = 0; StartCoroutine(camshake.Shake(.15f, .4f));
                    } 
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    if (staminabar.value >= 20f)
                    {
                        animtorr.SetTrigger("attack3"); transform.GetChild(2).gameObject.SetActive(true);
                        SoundEffect.clip = Swordeffects[2]; SoundEffect.Play(); staminabar.value -= 20f;
                        onetimedamage = true; attacktimer = 0; StartCoroutine(camshake.Shake(.15f, .4f));
                    }
                }
                
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        //if (attackpoint == null)
        //    return;

        //Gizmos.DrawWireSphere(attackpoint.position, attackrange);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        onetimejump = true;
        animtorr.SetBool("isground", true);
    }
    void OnCollisionStay2D(Collision2D col)
    {
       
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (transform.GetComponent<CapsuleCollider2D>().IsTouching(col))
        {
            if ("batmonster" == col.gameObject.tag)
            { animtorr.SetTrigger("takehit"); healthbar.value -= 15f; }
        }
        if (transform.GetComponent<CapsuleCollider2D>().IsTouching(col))
        {
            if ("mushmonster" == col.gameObject.tag)
            { animtorr.SetTrigger("takehit"); healthbar.value -= 15f; }
        }
        if (transform.GetComponent<CapsuleCollider2D>().IsTouching(col))
        {
            if ("goblinmonster" == col.gameObject.tag)
            { animtorr.SetTrigger("takehit"); healthbar.value -= 10f; }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (transform.GetChild(0).GetComponent<PolygonCollider2D>().IsTouching(col) || transform.GetChild(1).GetComponent<PolygonCollider2D>().IsTouching(col) || transform.GetChild(2).GetComponent<PolygonCollider2D>().IsTouching(col))
        {
            if ("batmonster" == col.gameObject.tag)
            {


                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack1")&& onetimedamage)
                { col.GetComponent<batmonster>().health = col.GetComponent<batmonster>().health - 10; col.GetComponent<batmonster>().takehit = true;  onetimedamage = false; } 
                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && onetimedamage)
                { col.GetComponent<batmonster>().health = col.GetComponent<batmonster>().health - 20; col.GetComponent<batmonster>().takehit = true;  onetimedamage = false; }
                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && onetimedamage)
                { col.GetComponent<batmonster>().health = col.GetComponent<batmonster>().health - 15; col.GetComponent<batmonster>().takehit = true;  onetimedamage = false; }
            }

            if ("mushmonster" == col.gameObject.tag)
            {
                
                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && onetimedamage)
                { col.GetComponent<mushmonster>().health = col.GetComponent<mushmonster>().health - 10; col.GetComponent<mushmonster>().takehit = true; onetimedamage = false; } 
                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && onetimedamage)
                { col.GetComponent<mushmonster>().health = col.GetComponent<mushmonster>().health - 20; col.GetComponent<mushmonster>().takehit = true; onetimedamage = false; }
                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && onetimedamage)
                { col.GetComponent<mushmonster>().health = col.GetComponent<mushmonster>().health - 15; col.GetComponent<mushmonster>().takehit = true; onetimedamage = false; }
            }

            if ("goblinmonster" == col.gameObject.tag)
            {

                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && onetimedamage)
                { col.GetComponent<goblinmonster>().health = col.GetComponent<goblinmonster>().health - 10; col.GetComponent<goblinmonster>().takehit = true; onetimedamage = false; }
                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && onetimedamage)
                { col.GetComponent<goblinmonster>().health = col.GetComponent<goblinmonster>().health - 20; col.GetComponent<goblinmonster>().takehit = true; onetimedamage = false; }
                if (animtorr.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && onetimedamage)
                { col.GetComponent<goblinmonster>().health = col.GetComponent<goblinmonster>().health - 15; col.GetComponent<goblinmonster>().takehit = true; onetimedamage = false; }
            }
        }
        if (transform.GetComponent<CapsuleCollider2D>().IsTouching(col)&&underattack)
        {
            Debug.Log("testmush");
            if ("mushmonsterattack" == col.gameObject.tag)
            { animtorr.SetTrigger("takehit"); healthbar.value -= 20f; }
            underattack = false;
        }

        if (transform.GetComponent<CapsuleCollider2D>().IsTouching(col) && underattack2)
        {
            Debug.Log("testgoblin");
            if ("goblinmonsterattack" == col.gameObject.tag)
            { animtorr.SetTrigger("takehit"); healthbar.value -= 15f; }
            underattack2 = false;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {

    }
    public void attackclosing()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    IEnumerator reloadTime()
    { yield return new WaitForSeconds(0.2f); }
}
