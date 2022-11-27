using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class craneObject : CharactorStats
{
    public string sound;
    public level5Manager Level5Manager;
    public GameObject floatingTextPrefab;
    PlayerStats playerStats;
    private Animator anim;
    public int getDamageNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerStats = player.GetComponent<PlayerStats>();
        anim = player.GetComponentInChildren<Animator>();
        Level5Manager = GameObject.Find("Level5Manager").GetComponent<level5Manager>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (getDamageNum == 3)
        {
            Die();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        if (
            anim.GetCurrentAnimatorStateInfo(0).IsName("Melee")
        )
        {
            TakeDamage(0);
        }
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("LanternHurt");
        getDamageNum += 1;
        Debug.Log(getDamageNum);
        if (floatingTextPrefab != null)
        {
            ShowFloatingText();
        }
    }

    private void ShowFloatingText()
    {
        var floatText = Instantiate(
            floatingTextPrefab,
            transform.position,
            Quaternion.identity,
            transform
        );
        floatText.GetComponent<TextMeshPro>().text = sound;
    }

    public override void Die()
    {
        base.Die();
        //Add ragdoll affect / death animation

        //For level 3
        NotificationManager.instance.count++;
        NotificationManager.instance.UpdateCount();
        Level5Manager.GameComplete();

        Destroy(gameObject);
    }
}