using UnityEngine;
using UnityEngine.AI;

//npcState == 1 去找player 第一次對話
//npcState == 2 帶玩家走到鹿群旁邊
//npcState == 3 叫玩家選一隻鹿
//npcState == 4 玩家選鹿
//npcState == 5 靈魂進入鹿
public class level6Manager : DialogueTrigger
{
    public Conversation convo1;
    public Conversation convo2;
    public Conversation convo3;

    private ThirdPersonChar player;
    private NavMeshAgent navMeshAgent;

    private Transform firstTarget;
    public Transform target;
    public float nearTarget;
    public LayerMask playerLayer;
    public LayerMask targetLayer;

    private GameObject level6UI;
    public GameObject chooseDeer;

    public override void Start()
    {
        SwitchSkills.getSkill = 4;
        navMeshAgent = GetComponent<NavMeshAgent>();
        level6UI = GameObject.Find("Level6UI");
        level6UI.SetActive(false);
        firstTarget = GameObject.Find("Player").transform;
        player = GameObject.Find("Player").GetComponent<ThirdPersonChar>();
        player.MoveToTarget(new Vector3(17f, -9.4f, 13.7f), new Vector3(0, 180, 0));
    }

    private void Update()
    {
        bool isNearTarget =
            npcState == 1
                ? Physics.CheckSphere(transform.position, nearTarget, playerLayer)
                : Physics.CheckSphere(transform.position, nearTarget, targetLayer);

        if (npcState == 1)
        {
            if (!isNearTarget)
            {
                ChaseTarget(firstTarget.position);
            }
            else
            {
                navMeshAgent.speed = 0;
            }
        }

        if (npcState == 2 && DialogueManager.EndConversation())
        {
            if (!isNearTarget)
            {
                ChaseTarget(target.position);
                npcState = 3;
            }
        }

        if (npcState == 3)
        {
            if (isNearTarget)
            {
                navMeshAgent.speed = 0;
                StartConvo();
            }
        }
    }

    public override void StartConvo()
    {
        switch (npcState)
        {
            case 1:
                convo = convo1;
                npcState = 2;
                break;
            case 3:
                convo = convo2;
                npcState = 4;
                break;
            case 5:
                convo = convo3;
                npcState = 6;
                break;
            default:
                convo = convo1;
                break;
        }
        DialogueManager.StartConversation(convo);
    }

    private void ChaseTarget(Vector3 target = default(Vector3))
    {
        navMeshAgent.speed = 5;
        navMeshAgent.destination = target;
    }

    public void ShowUI(GameObject deer)
    {
        level6UI.SetActive(true);
        chooseDeer = deer;
    }

    // finished 是否完成選擇
    public void DisableUI(bool finished = false)
    {
        level6UI.SetActive(false);
        chooseDeer = finished ? chooseDeer : null;
    }

    public void ConfirmDeer()
    {
        Debug.Log("玩家選擇的鹿: " + chooseDeer);
        DisableUI(true);
        npcState = 5;
        StartConvo();
    }
}
