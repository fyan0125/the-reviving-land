using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public Text notification;
    private static NotificationManager instance;

    public Animator anim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartNotice(string notice)
    {
        instance.anim.SetTrigger("isOpened");
        instance.notification.text = notice;
    }
}