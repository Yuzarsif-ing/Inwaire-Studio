using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batmonster : MonoBehaviour
{
    public Sprite[] anim;
    public Sprite[] animdead;
    public Sprite[] animtakehit;
    int animtimer = 0;
    int animtimerdead = 0;
    int animtimerhit = 0;
    public int health = 30;
    float timer=0;
    bool onetimework = true;
    public bool takehit = false;
    SpriteRenderer sprender;
    Rigidbody2D rbmonster;
    Vector3 vec;
    Vector3 vecscale = new Vector3(1, 1, 1);
    Transform monster;
    void Start()
    {
        sprender = GetComponent<SpriteRenderer>();
        rbmonster = GetComponent<Rigidbody2D>();
        monster = GetComponent<Transform>();
        vec = new Vector3(-1.5f, 0, 0);
    }

    
    void Update()
    {
        animasyon();
        rbmonster.velocity = vec;

    }

    void LateUpdate()
    {
        //animasyon();
    }

    void animasyon()
    {
        timer += Time.deltaTime;
        if (timer >= 0.08f&&!(health<=0)&&!takehit)
        {
            sprender.sprite = anim[animtimer++];
            
            if (animtimer == anim.Length - 1)
            {
                animtimer = 0;
            }
            timer = 0;
        }

        if (timer >= 0.08f && !(health <= 0) && takehit)
        {
            sprender.sprite = animtakehit[animtimerhit++];

            if (animtimerhit == animtakehit.Length )
            {
                animtimerhit = 0; takehit = false;
            }
            timer = 0;
        }

        if (timer >= 0.1f && health <= 0 && onetimework)
        {
            sprender.sprite = animdead[animtimerdead++];
            vec = Vector3.zero;
            if (animtimerdead == animdead.Length)
            {
                wait();
                Destroy(this.gameObject); animtimerdead = 0; onetimework=false;
            }
            timer = 0;
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "rotatepoint")
        { 
            vec.x = vec.x * -1;
            vecscale.x = vecscale.x*-1;
            vecscale.z = vecscale.z*-1;
            monster.localScale = vecscale;
        }

    }

    IEnumerator wait()
    { yield return new WaitForSeconds(0.1f); }
}
