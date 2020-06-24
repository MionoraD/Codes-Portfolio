using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour
{


    // Adjust the speed for the application.
    [Range(0.1f, 10f)] [SerializeField] private float speed = 1.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    [SerializeField] GameObject sprite;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject partPrefab;
    private bool hasDirection = false;
    public bool goBack = false;
    [SerializeField] AudioClip hitClip;
    GameManager gmc;

    // Make the enemy move towards direction
    public void StartMoving()
    {

    }

    private void Start()
    {
        //position on start is start position
        startPosition = transform.position;
        gmc = FindObjectOfType<GameManager>();
        waspPosition();
    }
    public void SetTarget(Transform target, Transform returnToDirection,float startspeed)
    {
        // provide the enemy with the target to move to (this gives possibility to diversify direction too)
        // startPosition = returnToDirection.position;
        startPosition = transform.position;
        targetPosition = target.position;
        speed = startspeed;
        hasDirection = true; //Flag it to go moving if there is a target selected
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            PartBehavior wallCode = collision.gameObject.GetComponent<PartBehavior>();
            if (wallCode != null)
            {
                if (wallCode.StealPart())
                {
                    waspPositionBack();
                }
            }
        }

        if (collision.gameObject.CompareTag("Core"))
        {
            gmc.removeHealth(1);
            this.gameObject.SetActive(false);
        }
    }
    void Move()
    {
        // Wait for direction
        if (hasDirection)
        {
            float step = speed * Time.deltaTime;
            //content is 'money'
            // If the enemy has not yet reached the player go to player
            if (goBack)
            {
                //change content to part
                targetPosition = startPosition;

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    gameObject.SetActive(false);
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }

    }
    private void OnMouseDown()
    {
        if (gmc.getMode() == inputMode.attack)
        {
            EnemyDies();
        }
    }

    public void EnemyDies()
    {
        AudioSource.PlayClipAtPoint(hitClip, transform.position);
        Tracker._instance.CatchEnemy();
        gmc.addPoints(10);
        this.gameObject.SetActive(false);
        if (goBack)
        {
            Instantiate(partPrefab, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(coinPrefab, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity);
        }
    }

    void SelectSpriteDirection()
    {
        //if currentpos lower than 
    }

    void waspPositionBack()
    {
        goBack = true;

        sprite.GetComponent<WaspAnimation>().goBack();
    }

    void waspPosition()
    {

        if (0 < startPosition.x && 0 < startPosition.z) //NW
        {
            sprite.GetComponent<WaspAnimation>().setDirection("NW");
            Debug.Log("NW");

        }
        else if (0 > startPosition.x && 0 < startPosition.z) //SW
        {
            sprite.GetComponent<WaspAnimation>().setDirection("SW");
            Debug.Log("SW");
        }
        else if (0 < startPosition.x && 0 > startPosition.z) //NE
        {
            sprite.GetComponent<WaspAnimation>().setDirection("NE");
            Debug.Log("NE");
        }
        else //SE
        {
            sprite.GetComponent<WaspAnimation>().setDirection("SE");
            Debug.Log("SE");
        }
    }
}
