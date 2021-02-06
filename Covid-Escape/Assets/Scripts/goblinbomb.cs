using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblinbomb : MonoBehaviour
{
    Rigidbody2D physic2d;
    Vector3 vec;
    public Animator animtor;
    float timer = 0;
    bool destroys;
    GameObject player;
    void Start()
    {
        physic2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("character");
        vec = player.transform.position - transform.position;
        physic2d.velocity = vec;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
    void FixedUpdate()
    {
        transform.GetChild(0).transform.Rotate(0, 0, -5f);
        if(destroys)
        { StartCoroutine(destroy()); }

        if (timer >= 10) { Destroy(this.gameObject); }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "character")
        {
            animtor.SetTrigger("explode");
            destroys = true;
        }
    }

   

    IEnumerator destroy() 
    {
        yield return new WaitForSeconds(0.07f);
        Destroy(this.gameObject);
    }
}
