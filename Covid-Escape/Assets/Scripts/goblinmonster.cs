using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblinmonster : MonoBehaviour
{
   
    public LayerMask playermask;
    public int health;
    float timer = 0;
    float attacktimer, attacktimer1 = 0;
    bool onetimework = true;
    bool onetimeattack,onetimeattack1 = false;
    bool monsterattack, monsterattack1 = false;
    public bool takehit = false;
    SpriteRenderer sprender;
    Rigidbody2D rbmonster;
    Vector3 vec;
    Vector3 vecsave = new Vector3(-2.5f, 0, 0);
    Vector3 vecscale = new Vector3(0.8f, 0.8f, 0.8f);
    Transform monster;
    public Transform bombtransform;
    GameObject player;
    RaycastHit2D hit;
    Animator animtor;
    ParticleSystem blood;
    public GameObject bomb;

    void Start()
    {
        sprender = GetComponent<SpriteRenderer>();
        rbmonster = GetComponent<Rigidbody2D>();
        monster = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("character");
        vec = new Vector3(-1.2f, 0, 0);
        animtor = transform.GetComponent<Animator>();
        blood = transform.GetChild(1).GetComponent<ParticleSystem>();
    }


    void Update()
    {

        rbmonster.velocity = vec;

        hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 200f, playermask);
        Debug.DrawLine(transform.position, hit.point, Color.green);
        //Debug.Log(hit.distance);

        if (8f>hit.distance && hit.distance > 4f)
        {
            onetimeattack1 = true; monsterattack1 = true; vec = Vector3.zero;
            if (0.05f < attacktimer1 && attacktimer1 < 2f)
            { animtor.SetBool("idle", true); }


        }

        else if (hit.distance < 1.4f)
        {
            onetimeattack = true; monsterattack = true; vec = Vector3.zero;
            if (0.05f < attacktimer && attacktimer < 0.09f)
            { animtor.SetBool("idle", true); }


        }
        
        else { animtor.SetBool("idle", false); }

    }

    void FixedUpdate()
    {
        animation();
    }
    void LateUpdate()
    {

    }

    void animasyon()
    {
        //timer += Time.deltaTime;
        //if (monsterattack)
        //{ attacktimer += Time.deltaTime; }

        //if (timer >= 0.1f && !(health <= 0) && !takehit && hit.distance > 2f && !monsterattack)
        //{
        //    sprender.sprite = anim[animtimer++];

        //    if (animtimer == anim.Length - 1)
        //    {
        //        animtimer = 0;
        //    }
        //    timer = 0;
        //}

        //if (timer >= 0.1f && !(health <= 0) && takehit)
        //{

        //    sprender.sprite = animtakehit[animtimerhit++];
        //    vec = Vector3.zero;
        //    if (animtimerhit == animtakehit.Length)
        //    {
        //        animtimerhit = 0; takehit = false;
        //    }
        //    timer = 0;
        //}

        //if (timer >= 0.1f && health <= 0 && onetimework)
        //{
        //    sprender.sprite = animdead[animtimerdead++];
        //    vec = Vector3.zero;
        //    if (animtimerdead == animdead.Length - 1)
        //    {
        //        wait();
        //        Destroy(this.gameObject); animtimerdead = 0; onetimework = false;
        //    }
        //    timer = 0;
        //}

        ////else if (player.transform.position.x - transform.position.x < 0) { transform.rotation = Quaternion.Euler(0, 0, 0); }
        ////else if (player.transform.position.x - transform.position.x > 0 && hit.distance < 2f) { transform.rotation = Quaternion.Euler(0, -180, 0); }
        ////else { transform.rotation = Quaternion.Euler(0,0, 0); }

        ////yaratık bize vururken 6. karede polygon collider aktif olup kapanıcak böylece yaratık bize vurmuş olucak + kılıcın 2 kere vurma sorunu düzeltilicek
        //if (timer >= 0.1f && !(health <= 0) && !takehit && onetimeattack && monsterattack && attacktimer > 0.5f)
        //{
        //    sprender.sprite = animattack[animtimerattack++];
        //    if (animtimerattack == animattack.Length)
        //    {
        //        animtimerattack = 0; attacktimer = 0; monsterattack = false; onetimeattack = false;
        //    }
        //    timer = 0;
        //}
        //if (!onetimeattack && !monsterattack && !takehit && !(health <= 0)) { vec = vecsave; }
    }

    void animation()
    {
        if (monsterattack|| monsterattack1)
        { attacktimer += Time.deltaTime; attacktimer1 += Time.deltaTime; }

        if (!(health <= 0) && takehit)
        {
            blood.Play();
            animtor.SetBool("takehit", true);
            vec = Vector3.zero;
        }
        else { animtor.SetBool("takehit", false); }

        if (!(health <= 0) && !takehit && onetimeattack && monsterattack && attacktimer > 0.09f)
        {
            animtor.SetBool("attack", true);
            monsterattack = false;
            onetimeattack = false;
            attacktimer = 0;
        }
        else { animtor.SetBool("attack", false); }

        if (!(health <= 0) && !takehit && onetimeattack1 && monsterattack1 && attacktimer1 > 2f)
        {
            animtor.SetBool("bombattack", true);
            monsterattack1 = false;
            onetimeattack1 = false;
            attacktimer1 = 0;
        }
        else { animtor.SetBool("bombattack", false); }

        if (health <= 0 && onetimework)
        {
            blood.Play();
            vec = Vector3.zero;
            animtor.SetBool("death", true);
            onetimework = false;
        }

        if (player.transform.position.x - transform.position.x > 0 && transform.localScale.x > 0 && (hit.distance < 1.4f|| 8f > hit.distance && hit.distance > 4f)) { transform.rotation = Quaternion.Euler(0, -180, 0); }
        else if (player.transform.position.x - transform.position.x < 0 && transform.localScale.x < 0 && (hit.distance < 1.4f|| 8f > hit.distance && hit.distance > 4f)) { transform.rotation = Quaternion.Euler(0, -180, 0); }
        else { transform.rotation = Quaternion.Euler(0, 0, 0); }

        StartCoroutine(wait());
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "rotatepoint" && transform.GetComponent<CapsuleCollider2D>().IsTouching(col))
        {
            vec.x = vec.x * -1;
            vecsave.x = vecsave.x * -1;
            vecscale.x = vecscale.x * -1;
            vecscale.z = vecscale.z * -1;
            monster.localScale = vecscale;
        }

    }
    void OnTriggerStay2D(Collider2D col)
    {
    }

    public void death()
    {
        Destroy(transform.parent.gameObject);
    }

    public void takehitt()
    {
        takehit = false;
    }

    public void attack()
    { player.GetComponent<charactermov>().underattack2 = true; }

    public void bombattacck()
    { Instantiate(bomb, bombtransform.transform.position, bomb.transform.rotation); }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        if (!onetimeattack1 && !monsterattack1 && !onetimeattack && !monsterattack && !takehit && !(health <= 0)) { vec = vecsave; }
    }
}
