using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;
public class EnemyTestMove : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float attackRange = 15;
    private Stack<Vector3Int> path;
    private Vector3 goal;
    private Vector3 destination;
    private Vector3 current;
    private Astar myAstar;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        
        
        myAstar = GetComponent<Astar>();

        Init();
    }

    void Init()
    {
        current = goal = current = targetPos = goal = new Vector3();
        target = FindObjectOfType<PlayerControlls>().gameObject.transform;
        targetPos = target.position;
        if (target.position != transform.position)
        {
            myAstar.Algorithm(out path, Vector3Int.FloorToInt(targetPos));
        }
        if (path != null)
        {
            current = path.Pop();
            destination = path.Pop();
            goal = target.transform.position;
        }    
    }
    // Update is called once per frame
    void Update()
    {
        if (path != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, 5 * Time.deltaTime);

            float distance = Vector2.Distance(destination, transform.position);
            if (distance <= 0f)
            {
                if (path.Count > 0)
                {
                    current = destination;
                    destination = path.Pop();

                    if (target.position != targetPos)
                    {
                        path = null;
                        Init();
                    }
                }
                else
                {
                    path = null;
                    Init();
                }
            }
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
            Init();
    }
}
