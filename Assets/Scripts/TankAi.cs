using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAi : MonoBehaviour {

    private GameObject Player;
    private Animator animator;
    private Ray ray;
    private RaycastHit hit;//레이케스트에 맞은애
    private float MaxDitanceToCheck = 6.0f;
    private float currentDistance;
    private Vector3 checkDirection;

    public Transform PointA;
    public Transform PointB;
    public NavMeshAgent navMeshAgent;// 인공지능이 달려있는 객체를 탱크 AI에게 달아준다? 

    public int currentTarget; //현재 타겟.
    public float DistanceFromTarget;//타겟과의 거리
    public Transform[] WayPoints = null;

    private void Awake()//스타트보다 먼저 실행됨. 그안에서 초기화를 시키는게 스타트보다 낫다.
    {
        Player = GameObject.FindWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        PointA = GameObject.Find("p1").transform;
        PointB = GameObject.Find("p2").transform;
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        WayPoints = new Transform[2]
        {
            PointA, PointB
        };
        //WayPoints = new Transform[2]; 위에랑 같은 뜻.
        //WayPoints[0] = PointA;
        //WayPoints[1] = PointB;

        currentTarget = 0;
        navMeshAgent.SetDestination(WayPoints[currentTarget].position);//지금 커런트타겟이 0으로 되어있으니 포인트 1의 포지션을 가진다.
    }

    private void FixedUpdate()
    {
        currentDistance = Vector3.Distance(Player.transform.position, transform.position);
        animator.SetFloat("distanceFromPlayer", currentDistance);//디스탄스프롬플레이어의 값을 커랜트디스탄스의 값을 플롯값으로 넣어준다

        checkDirection = Player.transform.position - transform.position;
        ray = new Ray(transform.position, checkDirection);//자신의 포지션에서 플레이어방향으로 레이를 쏜다.
        if(Physics.Raycast(ray, out hit, MaxDitanceToCheck))//레이케스트를 사거리 6 만큼 쐈을때 맞았다면 
        {
            if(hit.collider.gameObject == Player) // 그게 플레이어라면
            {
                animator.SetBool("isPlayerVisible", true);//애니메이터의 이즈플레이어비지블에 트루값을준다.
            }
            else
            {
                animator.SetBool("isPlayerVisible", false);
            }
        }
        else
        {
            animator.SetBool("isPlayerVisible", false);
        }

        DistanceFromTarget = Vector3.Distance(WayPoints[currentTarget].position, transform.position);
        animator.SetFloat("distanceFromWaypoint", DistanceFromTarget);
    }

    public void SetNextPoint()
    {
        switch (currentTarget)
        {
            case 0:
                currentTarget = 1;
                break;
            case 1:
                currentTarget = 0;
                break;
        }
        navMeshAgent.SetDestination(WayPoints[currentTarget].position);
    }


	void Start () {
		
	}
	
	

	void Update () {
		
	}
}
