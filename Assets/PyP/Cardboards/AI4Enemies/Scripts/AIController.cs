using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour
{

    public Transform target;
    public float targetPointHeight = 1.0f;
    public Transform spawnPoint;
    public GameObject projectile;
    private Transform myTargetPoint;
    private Transform otherTargetPoint;       
    public float movingSpeed = 5;
    public float rotationSpeed = 0.2f;
    public float fieldOfView = 180;
    public float viewDistance = 30;
    public float attakingDistance = 20;
    public float minimalDistance = 5;

    [HideInInspector]
    public string idleAnimation = "";
    [HideInInspector]
    public string walkAnimation = "";
    [HideInInspector]
    public string fightAnimation = "";
    [HideInInspector]
    public string hitAnimation = "";
    [HideInInspector]
    public string dieAnimation = "";
    

    public bool crossFadeAnimations = false;       
    public float shootingPause = 2.0f;
    public float deathDelayTime = 4;
    //public List<CollisionDamage> CollisionDamages;    
    public float projectileForce = 1000;
    //public float projectileLifeTime = 2;
    public float fightEffectsDelay = 0.4f;                       
    public bool calculationDamage = true;
    public int minDamage = 10;
    public int maxDamage = 15;
    public int hitChance = 90;
    public float health = 100;     
    public bool playSound = true;
    public SoundObject fightSound = new SoundObject();
    public SoundObject hitSound = new SoundObject();
    public SoundObject dieSound = new SoundObject();           
    public bool idleStart = false;
    public float waitingMaxTime = 5;
    
  
    public LayerMask visibleLayers = -1;
    //Event 1
    public delegate void HitHandler(AIEventArgs e);
    public static HitHandler onHit;        
    //Event 2
    public delegate void IsDyingHandler(AIEventArgs e);
    public static IsDyingHandler onIsDying;             
    //Event 3
    public delegate void IsDeadHandler(AIEventArgs e);
    public static IsDeadHandler onIsDead;

    
    //[HideInInspector]
    public List<Transform> waypoints; 

    //Additional parameters
    private float gravityFactor = 2;
    
    private float waypointDistance = 5;
    private float obstacleAvoidanceDistance = 1;
    private int targetingTolerance = 10; 

    private bool isfollowingTarget = false;
    private bool isAttaking = false;
    private Vector3 lastVisiblePosition = Vector3.zero;
    private float minimalDistanceStandard;
    private int currentWaypoint = 0;
    private bool waypointModus = true;
    private AnimateState characterAnimationState = AnimateState.idle;
    private bool isShooting = false;
    private bool isHit = false;
    private bool isWaiting = false;
    private bool isDead = false;
    private float shootAnimationLength = 0;
    private bool isAlive = true;
    private bool isWalkingToLastPosition = false;
    enum AnimateState
    {
        idle,
        run,
        shoot,
        hit,
        die
    }

    protected void Awake()
    {
        minimalDistanceStandard = minimalDistance;
        //Mein  TargetPoint-Transform zuweisen (Ausgangspunkt fuer alle Raycasts)
        if (GetComponentInChildren<TargetPointBehaviour>() != null)
        {
            TargetPointBehaviour tpScript = GetComponentInChildren<TargetPointBehaviour>();
            myTargetPoint = tpScript.gameObject.transform;
        }
        else
        {
            Debug.LogError("AI Character has no TargetPoint.");
        }

        if (target.gameObject.GetComponentInChildren<TargetPointBehaviour>() != null)
        {
            //TargetPoint-Transform des Ziels zuweisen (Ziel aller Raycasts)
            TargetPointBehaviour otherTpScript = target.gameObject.GetComponentInChildren<TargetPointBehaviour>();
            otherTargetPoint = otherTpScript.gameObject.transform;
        }
        else
        {
            Debug.LogError("Target has no TargetPoint.");
        }
                    
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        if (spawnPoint == null)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {                
                if (child.gameObject.name == "SpawnPoint")
                {
                    spawnPoint = child;
                    //Debug.Log("spawnpoint found");
                }
            }        
        }

        if (spawnPoint != null)
        {
            if (spawnPoint.GetComponent<Collider>() != null)
            {
                Destroy(spawnPoint.GetComponent<Collider>());
            }
        }

        //wenn es noch kein Rigidbody gibt, dann fuegen wir es hier hinzu
        //Wenn es schon eins gibt, dann erscheint zwar eine Hinweismeldung, das war es aber auch.       

        fightSound.gameObject = gameObject;
        hitSound.gameObject = gameObject;
        dieSound.gameObject = gameObject;
        fightSound.Init();
        hitSound.Init();
        dieSound.Init();
        
    }

    protected void Start()
    {
        //weil der WP-Editor in der Awake-Fkt die Liste fuellt, muessen wir dies erst in Start machen
        if (idleStart)
        {
            DoIdleStart();
        }

        InitAnimations();
        StartCoroutine(Shoot());

    }

    protected void FixedUpdate()
    {
        
        FindTarget();

        if (isAlive)
        {
            if (!waypointModus && !isDead)
            {
                                
                if (lastVisiblePosition != Vector3.zero)
                {
                    
                    RotateTo(lastVisiblePosition);
                    Walk(lastVisiblePosition);

                    //Hinderniss umlaufen                
                    CheckBarredWay(lastVisiblePosition);
                                                
                    //if (characterAnimationState == AnimateState.idle && !isfollowingTarget)

                    
                    if (characterAnimationState == AnimateState.idle && !isAttaking && !isWalkingToLastPosition)
                    {
                        waypointModus = true;
                        //Debug.Log("true");
                    }
                   
                }
                else
                {
                    StopMoving();
                }
            }
            else
            {

                if (waypoints.Count > 1)
                {
                    if (waypoints[currentWaypoint] == null)
                    {
                        ClearWaypoints();
                    }
                }
                
                //StopMoving();
                if (waypoints.Count == 0)
                {
                    SetDynamicWaypoints();
                }

                if (waypoints.Count > 1)
                {
                    RotateTo(waypoints[currentWaypoint].position);
                    WaypointWalk();
                    //Hinderniss umlaufen            
                    CheckBarredWay(waypoints[currentWaypoint].position);
                }
                
            }

            ObstacleAvoidance();

        }

        SurfaceDetection();
        PlayAnimation();
       
    }

    void InitAnimations()
    {
        GetComponent<Animation>().wrapMode = WrapMode.Loop;
        
        if (dieAnimation != "")
        {
            GetComponent<Animation>()[dieAnimation].wrapMode = WrapMode.ClampForever;
            GetComponent<Animation>()[dieAnimation].layer = 4;
            GetComponent<Animation>().Stop(dieAnimation); //2011-05-28-1
            
        }

        if (hitAnimation != "")
        {
            GetComponent<Animation>()[hitAnimation].wrapMode = WrapMode.Once;
            GetComponent<Animation>()[hitAnimation].layer = 3;
            GetComponent<Animation>().Stop(hitAnimation); //2011-05-28-1
        }

        GetComponent<Animation>()[fightAnimation].wrapMode = WrapMode.Once;
        GetComponent<Animation>()[fightAnimation].layer = 2;
        shootAnimationLength = GetComponent<Animation>()[fightAnimation].length;
        GetComponent<Animation>().Stop(fightAnimation); //2011-05-28-1
        GetComponent<Animation>()[idleAnimation].layer = 1;
        GetComponent<Animation>().Stop(idleAnimation); //2011-05-28-1
        GetComponent<Animation>()[walkAnimation].layer = 1;
        GetComponent<Animation>().Stop(walkAnimation); //2011-05-28-1
    }
    
    IEnumerator Shoot()
    {
        bool hasShooted = false; //2011-05-26

        if (shootingPause > 0)
        {
            float pastTime;
            while (isAlive)
            {
                pastTime = 0;
                isShooting = false; //2011-05-26
                hasShooted = false; //2011-05-26
                //if (isfollowingTarget)
                if (isAttaking && !isHit == true)
                {
                    Quaternion toTargetRotation;
                    Vector3 relativePos;
                    //relativePos = lastVisiblePosition - transform.position;
                    ////relativePos = lastVisiblePosition - aiCollider.position;                    
                    ////toTargetRotation = Quaternion.LookRotation(relativePos);
                    ////toTargetRotation = Quaternion.LookRotation(relativePos);
                    Vector3 lastVisiblePositionTemp = lastVisiblePosition;
                    lastVisiblePositionTemp.y = myTargetPoint.position.y;
                    relativePos = lastVisiblePositionTemp - myTargetPoint.position;
                    toTargetRotation = Quaternion.LookRotation(relativePos);

                    //Wert 20 kann auch durch eine Variable ersetzt werden
                    //if (EqualRotations(toTargetRotation, transform.rotation, 10))

                    if (EqualRotations(toTargetRotation, myTargetPoint.rotation, targetingTolerance))
                    {

                        //Debug.Log("shoot - isEqualRotations");
                        isShooting = true;

                        if (crossFadeAnimations)
                        {
                            GetComponent<Animation>().CrossFade(fightAnimation);                                                       
                        }
                        else
                        {
                            //animation.Play(fightAnimation);
                            GetComponent<Animation>().PlayQueued(fightAnimation, QueueMode.PlayNow);
                        }

                        characterAnimationState = AnimateState.idle;
                        yield return new WaitForSeconds(fightEffectsDelay);

                        if (projectile != null)
                        {
                            GameObject proj;
                            //proj = (GameObject)Instantiate(projectile, spawnPoint.position, gameObject.transform.rotation);
                            //proj = (GameObject)Instantiate(projectile, spawnPoint.position, myTargetPoint.transform.rotation);
                            proj = (GameObject)Instantiate(projectile, spawnPoint.position, spawnPoint.transform.rotation);
                            //Destroy(proj, projectileLifeTime);

                            if (projectileForce > 0)
                            {
                                proj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * projectileForce);
                            }
                        }
                        //Debug.Log("shoot");
                        if (playSound)
                        {
                            fightSound.Play();
                        }

                        if (calculationDamage)
                        {
                            //damage by Calculation
                            if (Random.Range(0, 100) <= hitChance)
                            {
                                target.SendMessage("ShotDamage", Random.Range(minDamage, maxDamage));
                            }
                        }
                        
                        if (shootAnimationLength < fightEffectsDelay)
                        {
                            yield return new WaitForSeconds(shootAnimationLength - fightEffectsDelay);
                            pastTime = shootAnimationLength;
                        }

                        if (shootAnimationLength > fightEffectsDelay)
                        {
                            yield return new WaitForSeconds(fightEffectsDelay);
                            pastTime = fightEffectsDelay;
                            //Debug.Log("'Projectile Delay' is shorter than the fight animation lenght");
                        }

                        //2011-05-26 -B
                        isShooting = false;
                        if (shootingPause > pastTime)
                        {
                            yield return new WaitForSeconds(shootingPause - pastTime);
                            //hasShooted = true;//2011-05-26 -2
                        }
                        //2011-05-26 -E
                        hasShooted = true;//2011-05-26 -2
                    }

                }

                //2011-05-26 -B
                //isShooting = false;
                //if (shootingPause > pastTime)
                //{
                //    yield return new WaitForSeconds(shootingPause - pastTime);
                //    
                //}
                if (hasShooted == false)
                {
                    yield return new WaitForSeconds(0.5f);
                }
                //2011-05-26 -E
            }
        }
        else
        {
            Debug.LogError("Shooting Pause have to be longer than 0 seconds.");
        }

    }

    protected void PlayAnimation()
    {
        switch (characterAnimationState)
        {
            case AnimateState.idle:
                GetComponent<Animation>().CrossFade(idleAnimation);
                break;
            case AnimateState.run:
                GetComponent<Animation>().CrossFade(walkAnimation);
                break;
        }
    }

    protected void RotateTo(Vector3 targetPosition)
    {

        //transform.LookAt(targetPosition);
        Quaternion destRotation;
        Vector3 relativePos;
        //relativePos = targetPosition - transform.position;

        Vector3 tap;
        Vector3 trp;
        tap = targetPosition;
        //trp = transform.position;
        trp = myTargetPoint.position;
        tap.y = 0;
        trp.y = 0;
        relativePos = tap - trp;

        destRotation = Quaternion.LookRotation(relativePos);
        //transform.rotation = Quaternion.Slerp(transform.rotation,destRotation, speedRotation *Time.fixedDeltaTime);		
        transform.rotation = Quaternion.Slerp(transform.rotation, destRotation, rotationSpeed );        

    }

    protected void Walk(Vector3 targetPosition)
    {
        Vector3 velocity;
        //Vector3 moveDirection = transform.TransformDirection(Vector3.forward);
        Vector3 moveDirection = myTargetPoint.TransformDirection(Vector3.forward);
        //Vector3 delta = targetPosition - transform.position;
        Vector3 delta = targetPosition - myTargetPoint.position;
        float magnitude = delta.magnitude;

        //if (delta.magnitude > minimalDistance)
        //if ((delta.magnitude > minimalDistance) && (!isShooting) && (!isHit) && (!isDead))
              
      
        if ((magnitude > minimalDistance) && (!isShooting) && (!isHit) && (isAlive))
        {
            velocity = moveDirection.normalized * movingSpeed ;
            //animation.CrossFade("run");
            characterAnimationState = AnimateState.run;
            
        }
        else
        {
            velocity = Vector3.zero;
            //animation.CrossFade("idle");
            
            if ((!isShooting) && (!isHit) && (!isDead))
            //if ((!isAttaking) && (!isHit) && (!isDead))
            {
                characterAnimationState = AnimateState.idle;
            }
        }

        //test-b
        if ((magnitude > minimalDistance))
        {

            isWalkingToLastPosition = true;

        }
        else
        {
            isWalkingToLastPosition = false;
        }
        // test-e
        GetComponent<Rigidbody>().velocity = velocity;
    }

    protected void SurfaceDetection()
    {        
        float tolerance = 0.05f;
        RaycastHit hit;
       
        //Debug.DrawRay(transform.position, -Vector3.up ,Color.red);
        //Debug.DrawRay(myTargetPoint.position, -Vector3.up * targetPointHeight , Color.red);
        //if (Physics.Raycast(transform.position, -Vector3.up, out hit, fromSurfaceToCenter, visibleLayers))
        //if (Physics.Raycast(transform.position, -Vector3.up, out hit, fromSurfaceToCenter, visibleLayers))
        if (Physics.Raycast(myTargetPoint.position, Vector3.down, out hit, targetPointHeight + 2.5f, visibleLayers))
        {
            if (hit.distance < targetPointHeight - tolerance)
            {
                //Debug.Log("U hit.distance: " + hit.distance.ToString());
                //Debug.Log("gameObject.transform.position.y: " + gameObject.transform.position.y);
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + (Vector3.up * gravityFactor );
                //rigidbody.velocity = rigidbody.velocity + (Vector3.up * gravityFactor );
            }
            else if (hit.distance > targetPointHeight + tolerance)
            {
                //Debug.Log("D hit.distance: " + hit.distance.ToString());
                //Debug.Log("gameObject.transform.position.y: " + gameObject.transform.position.y);
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + (Vector3.down * gravityFactor );
                //rigidbody.velocity = rigidbody.velocity + (Vector3.down * gravityFactor );
            }
            else
            {
                
                if (!isAlive ||(waypointModus== true && waypoints.Count < 2))
                {
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }
        else
        {            
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + (Vector3.down * gravityFactor );
            //rigidbody.velocity = rigidbody.velocity + (Vector3.down * gravityFactor );
        }      
    }

    void FindTarget()
    {
        if (otherTargetPoint != null)
        {
            RaycastHit hit;
            Ray ray = new Ray();
            //ray.origin = transform.position;
            ray.origin = myTargetPoint.position;
            //ray.direction = target.position - transform.position;
            //ray.direction = target.position - myTargetPoint.position;
            ray.direction = otherTargetPoint.position - myTargetPoint.position;

            isfollowingTarget = false;
            isAttaking = false;
            //Debug.DrawLine(myTargetPoint.position, otherTargetPoint.position,Color.magenta);

            if (Physics.Raycast(ray, out hit, viewDistance, visibleLayers))
            {
                //entweder ist das Object selber mein Zielobject oder aber das Parent von dem getroffenen Object (z.B. bei Collider-Gameobejcts)
                //if (hit.collider.gameObject.transform == target || (hit.collider.gameObject.transform.parent.gameObject.transform == target))          
                if (hit.collider.gameObject.transform == target)
                {
                    //if (isInFieldOfView( target.position))
                    if (IsInFieldOfView(otherTargetPoint.position))
                    {

                        isfollowingTarget = true;
                        //lastVisiblePosition = target.position;
                        lastVisiblePosition = otherTargetPoint.position;
                        waypointModus = false; //dynamic WP
                        if (waypoints.Count > 0)
                        {
                            ClearWaypoints(); //dynamic WP
                        }
                        if (hit.distance <= attakingDistance)
                        {
                            isAttaking = true;
                        }
                    }
                }
                else
                {
                    //Debug.Log("wrong target: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                //Debug.Log("no Physics.Raycast detect");
            }

            if (isfollowingTarget)
            {
                minimalDistance = minimalDistanceStandard;
            }
            else
            {
                minimalDistance = 0.2f;
            }
        }
        else
        {
            //dann ist er wohl tot oder hat noch nie existiert
            //deswegen wechseln wir wieder in den Waypointmodus
            waypointModus = true;
        }
    }

    bool IsInFieldOfView(Vector3 destination)
    {
        //Vector3 relative = transform.InverseTransformPoint(target);
        Vector3 relative = myTargetPoint.InverseTransformPoint(destination);
        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        float halfFieldOfView = fieldOfView / 2;
        if ((angle <= halfFieldOfView) && angle >= (-1) * halfFieldOfView)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void StopMoving()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        characterAnimationState = AnimateState.idle;
    }

    protected void WaypointWalk()
    {
        if (waypoints.Count > currentWaypoint)
        {
           
            Vector3 targetPosition = waypoints[currentWaypoint].position;
            Vector3 velocity;
            //Vector3 moveDirection = transform.TransformDirection(Vector3.forward);
            Vector3 moveDirection = myTargetPoint.TransformDirection(Vector3.forward);
            //Vector3 delta = targetPosition - transform.position;
            Vector3 delta = targetPosition - myTargetPoint.position;

            velocity = moveDirection.normalized * movingSpeed ;
            if (delta.magnitude > 1)
            {
                characterAnimationState = AnimateState.run;
            }
            else
            {

                if (currentWaypoint < (waypoints.Count - 1))
                {
                    currentWaypoint++;
                }
                else
                {
                    currentWaypoint = 0;
                }

                //Wait a random time between 0 and patrolRandomDelay (in seconds)
                if (waitingMaxTime > 0)
                {
                    StartCoroutine(Wait(Random.Range(0, waitingMaxTime)));
                }
            }

            if (isWaiting || isHit || isDead)
            {
                characterAnimationState = AnimateState.idle;
                velocity = Vector3.zero;
            }

            GetComponent<Rigidbody>().velocity = velocity;
           
        }
        else
        {
            ClearWaypoints();
        }
    }

    //dynamic WP
    void ClearWaypoints()
    {
        foreach (Transform tf in waypoints)
        {
            if (tf != null)
            {
                Destroy(tf.gameObject);
            }
        }

        waypoints.Clear();
        currentWaypoint = 0;
    }

    void SetDynamicWaypoints()
    {
        float height = myTargetPoint.position.y;

        Vector3 leftPos = Vector3.zero;
        Vector3 leftPos1 = transform.TransformPoint(Vector3.left * waypointDistance);
        //Vector3 leftPos1 = myTargetPoint.TransformPoint(Vector3.left * waypointDistance);         
        leftPos1.y = height;
        float leftPosBarrierDistance1 = 0;
        //if (IsSomethingBetween(transform.position, leftPos1, out leftPosBarrierDistance1))
        if (IsSomethingBetween(myTargetPoint.position, leftPos1, out leftPosBarrierDistance1))
        {

            Vector3 leftPos2 = transform.TransformPoint(Vector3.left * waypointDistance);
            //Vector3 leftPos2 = myTargetPoint.TransformPoint(Vector3.left * waypointDistance);
            leftPos2.y = height;
            GameObject leftWp2 = new GameObject();
            leftWp2.transform.position = leftPos2;
            leftPos2 = leftWp2.transform.TransformPoint(Vector3.forward * 1);
            float leftPosBarrierDistance2 = 0;

            //if (IsSomethingBetween(transform.position, leftPos2, out leftPosBarrierDistance2))
            if (IsSomethingBetween(myTargetPoint.position, leftPos2, out leftPosBarrierDistance2))
            {

                Vector3 leftPos3 = transform.TransformPoint(Vector3.left * waypointDistance);
                //Vector3 leftPos3 = myTargetPoint.TransformPoint(Vector3.left * waypointDistance);
                leftPos3.y = height;
                GameObject leftWp3 = new GameObject();
                leftWp3.transform.position = leftPos3;
                leftPos3 = leftWp3.transform.TransformPoint(Vector3.back * 1);
                float leftPosBarrierDistance3 = 0;

                //if (IsSomethingBetween(transform.position, leftPos3, out leftPosBarrierDistance3))
                if (IsSomethingBetween(myTargetPoint.position, leftPos3, out leftPosBarrierDistance3))
                {
                    //wenn alle 3 WPs von etwas versperrt werden
                    // dann waehle ich eins von den aeusseren
                    if (leftPosBarrierDistance2 > leftPosBarrierDistance3)
                    {
                        leftPos = leftPos2;
                    }
                    else
                    {
                        leftPos = leftPos3;
                    }
                }
                Destroy(leftWp3);

            }
            else
            {
                leftPos = leftPos2;
            }
            Destroy(leftWp2);

        }
        else
        {
            leftPos = leftPos1;
        }
        float barrierDistance = 0;
        Vector3 surfaceCheck;
        surfaceCheck = leftPos;
        surfaceCheck.y -= 10;

        
        if (IsSomethingBetween(leftPos, surfaceCheck, out barrierDistance))
        {
            //nur wenn ein Boden gefunden wird, wird auch ein WP erzeugt            
            leftPos.y -= (barrierDistance - targetPointHeight);
            CreateWaypoint(leftPos, "wpL");
        }
        else
        {
            CreateWaypoint(myTargetPoint.position, "wpL");
        }


        //Vector3 rightPos;
        //Vector3 rightPos1 = transform.TransformPoint(Vector3.right * defaultWaypointDistance);
        //CreateWaypoint(rightPos, "wpR");      
        Vector3 rightPos = Vector3.zero;
        Vector3 rightPos1 = transform.TransformPoint(Vector3.right * waypointDistance);
        //Vector3 rightPos1 = myTargetPoint.TransformPoint(Vector3.right * waypointDistance);
        rightPos1.y = height;
        float rightPosBarrierDistance1 = 0;
        //if (IsSomethingBetween(transform.position, rightPos1, out rightPosBarrierDistance1))
        if (IsSomethingBetween(myTargetPoint.position, rightPos1, out rightPosBarrierDistance1))
        {
            Vector3 rightPos2 = transform.TransformPoint(Vector3.right * waypointDistance);
            //Vector3 rightPos2 = myTargetPoint.TransformPoint(Vector3.right * waypointDistance);
            rightPos2.y = height;
            GameObject rightWp2 = new GameObject();
            rightWp2.transform.position = rightPos2;
            rightPos2 = rightWp2.transform.TransformPoint(Vector3.forward * 1);
            float rightPosBarrierDistance2 = 0;

            //if (IsSomethingBetween(transform.position, rightPos2, out rightPosBarrierDistance2))
            if (IsSomethingBetween(myTargetPoint.position, rightPos2, out rightPosBarrierDistance2))
            {

                Vector3 rightPos3 = transform.TransformPoint(Vector3.right * waypointDistance);
                //Vector3 rightPos3 = myTargetPoint.TransformPoint(Vector3.right * waypointDistance); 
                rightPos3.y = height;
                GameObject rightWp3 = new GameObject();
                rightWp3.transform.position = rightPos3;
                rightPos3 = rightWp3.transform.TransformPoint(Vector3.back * 1);
                float rightPosBarrierDistance3 = 0;

                //if (IsSomethingBetween(transform.position, rightPos3, out rightPosBarrierDistance3))
                if (IsSomethingBetween(myTargetPoint.position, rightPos3, out rightPosBarrierDistance3))
                {
                    //wenn alle 3 WPs von etwas versperrt werden
                    // dann waehle ich eins von den aeusseren
                    if (rightPosBarrierDistance2 > rightPosBarrierDistance3)
                    {
                        rightPos = rightPos2;
                    }
                    else
                    {
                        rightPos = rightPos3;
                    }
                }
                Destroy(rightWp3);

            }
            else
            {
                rightPos = rightPos2;
            }
            Destroy(rightWp2);
        }
        else
        {
            rightPos = rightPos1;
        }

        surfaceCheck = rightPos;
        surfaceCheck.y -= 10;
        
        if (IsSomethingBetween(rightPos, surfaceCheck, out barrierDistance))
        {
            //nur wenn ein Boden gefunden wird, wird auch ein WP erzeugt            
        rightPos.y -= (barrierDistance - targetPointHeight);
        CreateWaypoint(rightPos, "wpR");
        }
        else
        {
            CreateWaypoint(myTargetPoint.position, "wpR");
        }
    }

    void CreateWaypoint(Vector3 position, string name)
    {
        //GameObject myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject newWp = new GameObject();
        newWp.name = name;
        newWp.transform.position = position;
        //myCube.renderer.enabled = false;
        //Destroy(myCube.collider);
        waypoints.Add(newWp.transform);
    }

    //dynamic WP ende

    public void AddLife(float life)
    {
        health += life;
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        fieldOfView = 360;
        if (health <= 0)
        {
            StartCoroutine(CharacterDie());
        }
        else
        {
            StartCoroutine(CharacterHit());
            //CharacterHit();
        }
    }

    IEnumerator CharacterHit()
    {
        if (hitAnimation != "")
        {
            isHit = true;
            if (crossFadeAnimations)
            {
                GetComponent<Animation>().CrossFade(hitAnimation);
            }
            else
            {
                GetComponent<Animation>().Play(hitAnimation);
            }
            if (playSound)
            {
                hitSound.Play();
            }
            yield return new WaitForSeconds(0.4f);
            isHit = false;
        }

        if (onHit != null)
        {
            AIEventArgs e = new AIEventArgs ();
            e.name = gameObject.name;
            e.health = health;
            e.position = gameObject.transform.position;
            e.rotation = gameObject.transform.rotation;     
            e.tag = gameObject.tag;
            onHit(e);
        }
    }

    IEnumerator CharacterDie()
    {
        if (!isDead)
        {
            isDead = true;
            isAlive = false;
            //Debug.Log("Play die");
            if (dieAnimation != "")
            {                

                if (crossFadeAnimations)
                {
                    GetComponent<Animation>().CrossFade(dieAnimation);
                }
                else
                {
                    GetComponent<Animation>().Play(dieAnimation);
                }              

            }

            if (playSound)
            {
                dieSound.Play();
            }

            yield return new WaitForSeconds(0.4f);
          
            if (deathDelayTime == 0)
            {
                Destroy(this.gameObject);
            }
            else if (deathDelayTime > 0)
            {
                Destroy(this.gameObject, deathDelayTime);
            }

            if (onIsDying != null)
            {                    
                AIEventArgs e = new AIEventArgs();
                e.name = gameObject.name;
                e.health = health;
                e.position = gameObject.transform.position;
                e.rotation = gameObject.transform.rotation;        
                e.tag = gameObject.tag;
                onIsDying(e);
            }
        }
    }

    void OnDestroy () {
        if (onIsDead != null && isAlive == false)
        {            
            AIEventArgs e = new AIEventArgs();
            e.name = gameObject.name;
            e.health = health;
            e.position = gameObject.transform.position;
            e.rotation = gameObject.transform.rotation;    
            e.tag = gameObject.tag;
            onIsDead(e);
        }
    }
    
    IEnumerator Wait(float seconds)
    {
        isWaiting = true;
        yield return new WaitForSeconds(seconds);
        isWaiting = false;
    }

    void CheckBarredWay(Vector3 destination)
    {
        RaycastHit hit;
        //Vector3 delta = destination - transform.position;
        Vector3 delta = destination - myTargetPoint.position;
        //if (Physics.Raycast(transform.position, delta, out hit, delta.magnitude))
        //if (Physics.Linecast(transform.position, destination, out hit, visibleLayers))
        if (Physics.Linecast(myTargetPoint.position, destination, out hit, visibleLayers))
        {
            //Wert 1 koennte auch ueber Variable individualisierbar gemacht werden
            if (hit.distance <= 1)
            {
                //Wert 1 koennte auch ueber Variable individualisierbar gemacht werden
                //if (delta.sqrMagnitude > (1 * 1))
                if (delta.sqrMagnitude > 1)
                {
                    //erstmal anhalten
                    StopMoving();
                    //Alle Waypoints loeschen(falls es welche gibt), um dann neue zu erzeugen
                    ClearWaypoints();
                    //Waypoint-Modus aktivieren - die Waypoints selber werden beim naechsten Waypoint-Durchlauf erzeugt.
                    waypointModus = true;
                    //Erzeugt werden die
                }
            }
        }
    }

    bool IsSomethingBetween(Vector3 start, Vector3 destination, out float barrierDistance)
    {
        barrierDistance = -1;
        RaycastHit hit;
        //Vector3 delta = destination - transform.position;
        Vector3 delta = destination - myTargetPoint.position;
        bool result = false;
        //if (Physics.Raycast(transform.position, delta, out hit, delta.magnitude))
        if (Physics.Linecast(start, destination, out hit, visibleLayers))
        {
            barrierDistance = hit.distance;
            result = true;
        }

        return result;
    }

    bool EqualRotations(Quaternion Rotation1, Quaternion Rotation2, float angleTolerance)
    {
        bool isEqual = false;

        if (Quaternion.Angle(Rotation1, Rotation2) < angleTolerance)
        {
            isEqual = true;
        }

        return isEqual;
    }

    void DoIdleStart()
    {
        ClearWaypoints();
        //Vector3 startPos = transform.position;       
        Vector3 startPos = myTargetPoint.position;
        CreateWaypoint(startPos, "wpStart");
    }

    void ObstacleAvoidance()
    {
        Vector3 oaMoveDirection = Vector3.zero;
        float height = myTargetPoint.position.y;
        
        
        Vector3 leftPos1 = transform.TransformPoint((Vector3.left + Vector3.forward) * obstacleAvoidanceDistance);                             
        leftPos1.y = height;
        
        float leftPosBarrierDistance1 = 0;
        //Debug.DrawLine(myTargetPoint.position, leftPos1, Color.blue);
        if (IsSomethingBetween(myTargetPoint.position, leftPos1, out leftPosBarrierDistance1))
        {
            oaMoveDirection = myTargetPoint.TransformDirection(Vector3.right);               
        }
                   
        Vector3 rightPos1 = transform.TransformPoint((Vector3.right + Vector3.forward )* obstacleAvoidanceDistance);
        rightPos1.y = height;

        float rightPosBarrierDistance1 = 0;
        //Debug.DrawLine(myTargetPoint.position, rightPos1, Color.blue);
        if (IsSomethingBetween(myTargetPoint.position, rightPos1, out rightPosBarrierDistance1))
        {
            if (oaMoveDirection == Vector3.zero || leftPosBarrierDistance1 > rightPosBarrierDistance1)
            {
                oaMoveDirection = myTargetPoint.TransformDirection(Vector3.left);                    
            }
        }

        if (oaMoveDirection != Vector3.zero)
        {               
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + oaMoveDirection.normalized * (movingSpeed) ;
        }
        
    }
  
}

public class AIEventArgs 
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public float health;          
    public string tag;
}




