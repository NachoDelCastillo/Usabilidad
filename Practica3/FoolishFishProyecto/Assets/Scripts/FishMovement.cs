using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;
using Random = UnityEngine.Random;

public class FishMovement : MonoBehaviour
{
    [SerializeField]
    bool DEBUGGING;

    [SerializeField]
    Timer timer;

    bool hasJumped = false;

    // Components
    Rigidbody2D rb;
    Animator anim;
    [SerializeField]
    ParticleSystem bubblesSystem;
    [SerializeField]
    ParticleSystem SplashSystem;

    [SerializeField]
    Transform waterGFX;

    // Movement
    float walkVelocity = 2;
    float input_hor;
    bool WalkInputInactivity; // Se pone a true despues de X segundos de inactividad
    float input_hor_notClicked_time = 1;
    float input_hor_notClicked_timer = 0;
    LineRenderer lr;

    // Jump
    float JumpForce = 13;
    [SerializeField]
    Transform mouseTarget;
    [SerializeField]
    Transform rotatingPivot;

    MousePosition3D mousePosition;

    [HideInInspector]
    public bool onAir;
    bool onJumpingAnimation; // Esta a true si se esta ejecutando la animacion de salto

    // Ground checker
    [SerializeField] Transform groundCheck_tr;
    [SerializeField] Transform groundCheckLeft_tr;
    [SerializeField] Transform groundCheckRight_tr;
    [SerializeField] LayerMask groundLayer;
    bool onGround;
    float check_radius = .3f;
    bool onGround_Remember; // El estado de la variable onGround el frame anterior
    
    // Wall checker
    [SerializeField] Transform leftWallCheck_tr;
    [SerializeField] Transform rightWallCheck_tr;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask bounceLayer;
    bool onLeftWall;
    bool onRightWall;
    bool onLeftWall_Remember; // El estado de la variable onWall el frame anterior
    bool onRightWall_Remember; // El estado de la variable onWall el frame anterior

    float xVelocityRemember;

    bool canJump = false; 

    #region Input Setup

    PlayerControls inputActions;
    Vector2 movementInput;

    bool jump_performed; // Jump pressed this frame
    bool jump_canceled; // Jump released this frame
    bool jump_hold;

    [SerializeField]
    GameObject arrowPrefab;
    GameObject arrow;

    bool arrow_performed;
    bool bubbles_performed;

    [SerializeField]
    GameObject dustSystem;
    Vector3 jumpLastMousePos= new Vector3();

    PlayerMoveEvent playerMoveEvent;

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();

            inputActions.PlayerActions.Jump.performed += Jump_canceled;
            inputActions.PlayerActions.Jump.canceled += Jump_performed;
        }

        inputActions.Enable();
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if (!canJump)
            return;
        jumpLastMousePos = mouseTarget.position;
        jump_performed = true;
        jump_hold = true;
    }

    private void Jump_canceled(InputAction.CallbackContext obj)
    {
        if (!canJump)
            return;
       
        jump_canceled = true;
        jump_hold = false;
    }

    public void CanJumpNow()
    {
        canJump = true;
    }

    // Resetear variables
    private void LateUpdate()
    {
        if (!canJump)
            return;

        jump_performed = false;
        jump_canceled = false;
    }

    private void OnDisable()
    { inputActions.Disable(); }

    #endregion

    void Awake()
    {
        if (DEBUGGING)
            canJump = true;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        lr = GetComponentInChildren<LineRenderer>();

        mousePosition = mouseTarget.GetComponent<MousePosition3D>();

        arrow_performed = false;
        bubbles_performed = false;


        if (PlayerPrefs.GetInt("TutorialDone", 0) == 1)
        {
            transform.position = startingPosition.position;

            canJump = true;
        }

        else
        {
            canJump = false;

            PlayerPrefs.SetInt("TutorialDone", 1);
        }

        //Vector3 desiredForwardVector = Vector3.back;
        //Vector3 desiredRightVector = Vector3.up;


        ////Vector3 upVector = Vector3.Cross(desiredForwardVector, desiredRightVector);
        ////rotatingPivot.transform.rotation = Quaternion.LookRotation(forward)



        //Vector3 rightProjected = Vector3.ProjectOnPlane(desiredForwardVector, desiredRightVector).normalized; // makes sure the vectors are perpendicular. You can skip this if you are already sure they are.
        //rotatingPivot.transform.rotation = Quaternion.LookRotation(desiredForwardVector, rightProjected); // sets the forward vector to desiredForwardVector and the up vector to rightProjected. Now we need to rotate it so the right vector is at rightProjected
        //rotatingPivot.transform.rotation *= Quaternion.AngleAxis(-90f/*this might need to be positive*/, rotatingPivot.transform.forward);
    }

    void Update()
    {
        if (PauseMenu_PK.paused) return;
        if(inputActions.PlayerActions.ShowTrajectory.IsInProgress() && canJump) CreateTrayectory();
        InputHandler();

        if (onAir)
        {
            lr.enabled = false;
            if(arrow!=null) DestroyArrow();
            OnAir();
        }

        if (GameplayManager.Instance.IsUnderWater(transform.position.y))
        {
            //Debug.Log("UNDERWATER");
            if (!bubbles_performed)
            {
                bubblesSystem.Play();
                bubbles_performed = true;
            }

            thisFrameUnderWater = true;
        }

        else
        {
            // Debug.Log("NOT UNDERWATER");
            //bubblesSystem.gameObject.SetActive(false);
            bubblesSystem.Stop();
            bubbles_performed=false;
            thisFrameUnderWater = false;
        }

        //Debug.Log("wafedwgrhtejy");
        //Debug.Log("thisFrameUnderWater = " + thisFrameUnderWater);

        if (thisFrameUnderWater && !lastFrameUnderwater)
        {
            if (transform.position.y < 2 + GameplayManager.Instance.FirstWaterLevelPosY())
            {
                Debug.Log("RETURN TO STARTING POINT");
                rb.velocity = Vector3.zero;
                xVelocityRemember = 0f;
                float jumpDuration = 1.5f;
                transform.DOJump(startingPosition.position, - 5, 1, jumpDuration);
                rotatingPivot.DORotate(new Vector3(0, 0, 720 + 180), 
                    jumpDuration + .3f, RotateMode.FastBeyond360);
                rb.isKinematic = true;
                Invoke("JumpFinished", jumpDuration + .2f);

                anim.SetTrigger("Fail");
            }
        }

        lastFrameUnderwater = thisFrameUnderWater;
    }
    public void CreateTrayectory()
    {
        if (onAir) { return; }
        Vector2 velocity = (Vector2)Calculador.calcular(transform,mouseTarget.position);

        if (velocity == Vector2.zero)
        {
            lr.positionCount = 0;
            mousePosition.ShowX();
            return;
        };
        mousePosition.HideX();

        lr.enabled = true;

        Vector2[] trajectory = Plot(rb, (Vector2)transform.position, velocity, 1000);

        List<Vector3> pos = new List<Vector3>();

        for (int i = 0; i < trajectory.Length; i++)
        {
            if (!GameplayManager.Instance.IsUnderWater(trajectory[i].y)) break;
            else
                pos.Add(trajectory[i]);
        }
        if (pos.Count == 0) { return; }
        int tam = pos.Count - 1;
        lr.positionCount = tam;
        Vector3[] positions = new Vector3[tam];

        for (int i = 0; i < tam; i++)
            positions[i] = pos[i];

        lr.SetPositions(positions);

        if (!arrow_performed)
        {
            arrow = Instantiate<GameObject>(arrowPrefab, pos[tam], Quaternion.identity);
            arrow_performed = true;
        }
    }
    public void DestroyArrow()
    {
        if (arrow_performed)
        {
            Destroy(arrow.gameObject);
            arrow_performed = false;
        }
    }
    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = -Physics2D.gravity * rigidbody.mass * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }

    void JumpFinished()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
    }

    bool thisFrameUnderWater = true;
    bool lastFrameUnderwater = true;

    [SerializeField]
    Transform startingPosition;

    void InputHandler()
    {
        #region Movement
        // Si se detecta que no se esta pulsando ninguna tecla
        if (movementInput.x > -.1 && movementInput.x < .1)
        {
            // Sumar tiempo al timer
            input_hor_notClicked_timer += Time.deltaTime;

            // Si el timer llega a X segundos, informarlo con el bool input_hor_notClickedInXSec
            if (input_hor_notClicked_timer > input_hor_notClicked_time)
                WalkInputInactivity = true;
        }
        // Resetear el timer
        else
        {
            input_hor_notClicked_timer = 0;
            WalkInputInactivity = false;
        }

        anim.SetBool("WalkInputInactivity", WalkInputInactivity);

        #endregion


        #region Jump

        // Si se puede saltar y se ha presionado saltar
        if (jump_performed && !onAir && (onGround || onLeftWall || onRightWall) && !onJumpingAnimation)
        {
            onJumpingAnimation = true;

            // Parar del todo el cuerpo
            rb.velocity = Vector3.zero;

            anim.SetTrigger("Jump");

            JumpStartEvent trackerEvent = new JumpStartEvent(1);
            Tracker.Instance.TrackEvent(trackerEvent);
        }

        #endregion
    }

    #region Jump

    // Se llama cuando se activa el trigger en la animaci�n de salto del pez
    public void JumpAnimationTrigger() {
        onJumpingAnimation = false;
        Jump();
    }

    public void Jump()
    {
        Vector3 vel = Calculador.calcular(transform, jumpLastMousePos);
        if (vel == Vector3.zero)
        {
            anim.SetTrigger("Land");
            return;
        }

        AudioManager_PK.GetInstance().Play("Jump", UnityEngine.Random.Range(0.9f, 1.1f));

        rb.velocity = vel;

        xVelocityRemember = rb.velocity.x;

        Instantiate<GameObject>(dustSystem, transform.position, Quaternion.identity);
        onAir = true;

        if (!hasJumped)
        {
            timer.StartTimer();
            hasJumped = true;
        }

        // Resetear la variable que guarda la velocidad horizontal/vertical cuando esta andando
        currentXvel = 0;
        currentYvel = 0;
    }

    Vector2 currentDirOnAir;
    void OnAir()
    {
        // Logica
        currentXvel = 0;
        currentYvel = 0;

        // Comprobar si se sale del agua;
        if (GameplayManager.Instance.IsUnderWater(transform.position.y))
        {
            if (rb.gravityScale != -1)
            {
                if (rb.gravityScale == 1)
                {
                    AudioManager_PK.instance.Play("Splash", Random.Range(0.8f, 1.1f));
                    splashPart = Instantiate(SplashSystem, new Vector3(transform.position.x, GameplayManager.Instance.GetWaterLevel() + 0.1f), Quaternion.identity, waterGFX);
                    SplashSystem.Play();
                }
                rb.gravityScale = -1;
            }
        }
        else
        {
            if (rb.gravityScale != 1)
            {
                if (rb.gravityScale == -1)
                {
                    AudioManager_PK.instance.Play("Splash", Random.Range(0.8f, 1.1f));
                    splashPart = Instantiate(SplashSystem, new Vector3(transform.position.x, GameplayManager.Instance.GetWaterLevel() + 0.1f), Quaternion.identity, waterGFX);
                    SplashSystem.Play();
                }
                rb.gravityScale = 1;
                GameplayManager.Instance.ResetWaterLevel();
            }
        }

        // Rotaciones graficas
        Vector2 dir = rb.velocity;
        dir.Normalize();
        currentDirOnAir = Vector2.Lerp(currentDirOnAir, dir, Time.deltaTime);
        rotatingPivot.up = dir;
        Transform child = rotatingPivot.GetChild(0);
        child.localRotation = Quaternion.Euler(child.localRotation.eulerAngles.x, 180, child.localRotation.eulerAngles.z);
    }

    #endregion


    void FixedUpdate()
    {
        Move();

        CheckGroundAndWalls();
    }

    private Vector3 m_Velocity = Vector3.zero;
    float smoothTime = .1f;

    // Velocidad actual al andar, ya sea por el suelo (x) o por la pared (y)
    float currentXvel = 0;
    float currentYvel = 0;
    private ParticleSystem splashPart;

    private void Move()
    {
        // Si se esta saltando, no permitir moverse
        if (onAir || onJumpingAnimation)
            return;


        // Velocidad deseada este frame
        Vector3 targetVelocity = Vector3.zero;

        // La direccion depende de si el pescado esta en el suelo, en la pared izquiera o derecha
        if (onGround)
        {
            var collidersLeft = Physics2D.OverlapCircleAll(leftWallCheck_tr.position, check_radius, bounceLayer);
            var collidersRight = Physics2D.OverlapCircleAll(rightWallCheck_tr.position, check_radius, bounceLayer);
            if ((movementInput.x > 0 && collidersRight.Length > 0) ||
                (movementInput.x < 0 && collidersLeft.Length > 0))
            {
                targetVelocity = Vector2.zero;
                currentXvel = Mathf.Lerp(currentXvel, targetVelocity.x, Time.deltaTime * 10);
                playerMoveEvent = new PlayerMoveEvent(1);
                Tracker.Instance.TrackEvent(playerMoveEvent);
            }
            else
            {
                targetVelocity = new Vector2(movementInput.x * walkVelocity, 0);
                currentXvel = Mathf.Lerp(currentXvel, targetVelocity.x, Time.deltaTime * 2);
            }

            // And then smoothing it out and applying it to the character
            rb.velocity = new Vector3(currentXvel, 0);

            // Cambiar animacion
            anim.SetFloat("WalkVelocity", rb.velocity.x / walkVelocity);
            anim.SetFloat("WalkMagnitude", Mathf.Abs(rb.velocity.x / walkVelocity));
        }
        else
        {
            if (onLeftWall)
            {
                targetVelocity = new Vector2(0, movementInput.y * walkVelocity);
                playerMoveEvent = new PlayerMoveEvent(1);
                Tracker.Instance.TrackEvent(playerMoveEvent);
            }

            else if (onRightWall)
            {
                targetVelocity = new Vector2(0, movementInput.y * walkVelocity);
                playerMoveEvent = new PlayerMoveEvent(1);
                Tracker.Instance.TrackEvent(playerMoveEvent);
            }

            currentYvel = Mathf.Lerp(currentYvel, targetVelocity.y, Time.deltaTime * 2);
            rb.velocity = new Vector3(0, currentYvel);

            if (onLeftWall)
            {
                // Cambiar animacion
                anim.SetFloat("WalkVelocity", rb.velocity.y / walkVelocity);
                anim.SetFloat("WalkMagnitude", Mathf.Abs(rb.velocity.y / walkVelocity));
            }
            else if (onRightWall)
            {
                // Cambiar animacion
                anim.SetFloat("WalkVelocity", -rb.velocity.y / walkVelocity);
                anim.SetFloat("WalkMagnitude", Mathf.Abs(rb.velocity.y / walkVelocity));
            }
        }

        //Debug.Log("targetVelocity = " + targetVelocity);
    }

    private void CheckGroundAndWalls()
    {
        #region Ground

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck_tr.position, check_radius, groundLayer);

        if (colliders.Length == 0)
            onGround = false;
        else
            onGround = true;

        if (!onGround_Remember && onGround)
            AudioManager_PK.instance.Play("Fall", UnityEngine.Random.Range(.3f, .6f));

        // Si andas hacia un precipicio, caerte
        if (onGround_Remember && !onGround && !onAir)
            onAir = true;

        if ((onGround && !onGround_Remember))
            Land();

        onGround_Remember = onGround;

        #endregion


        #region Wall

        #region LeftWall
        colliders = Physics2D.OverlapCircleAll(leftWallCheck_tr.position, check_radius, wallLayer);

        if (colliders.Length == 0)
            onLeftWall = false;
        else
            onLeftWall = true;


        // Si andas hacia un precipicio, caerte
        if (onLeftWall_Remember && !onLeftWall && !onAir)
            onAir = true;

        if ((onLeftWall && !onLeftWall_Remember))
            Land();

        onLeftWall_Remember = onLeftWall;


        #endregion

        #region RightWall
        colliders = Physics2D.OverlapCircleAll(rightWallCheck_tr.position, check_radius, wallLayer);

        if (colliders.Length == 0)
            onRightWall = false;
        else
            onRightWall = true;


        // Si andas hacia un precipicio, caerte
        if (onRightWall_Remember && !onRightWall && !onAir)
            onAir = true;

        if ((onRightWall && !onRightWall_Remember))
            Land();

        onRightWall_Remember = onRightWall;

        //Bounce
        if (Mathf.Abs(rb.velocity.x - xVelocityRemember) > 0.5f && onAir)
        {
            rb.velocity = new Vector2(-xVelocityRemember * 0.9f, rb.velocity.y * 0.9f);
        }

        xVelocityRemember = rb.velocity.x;

        #endregion

        #endregion

        bool onSurface = onGround || onLeftWall || onRightWall;
        anim.SetBool("OnGround", onSurface);
    }

    void Land()
    {
        JumpEndEvent trackerEvent = new JumpEndEvent(1);
        Tracker.Instance.TrackEvent(trackerEvent);


        GameplayManager.Instance.RecalculateWaterLevel(transform.position.y);

        AudioManager_PK.instance.Play("Land", Random.Range(0.9f, 1.1f));

        onAir = false;
        anim.SetTrigger("Land");

        rotatingPivot.DOKill();

        // Eliminar gravedad (solo se aplica gravedad cuando el pez esta en el aire)
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;

        // Rotaciones graficas

        if (onGround)
        {
            rotatingPivot.up = Vector2.down;
            Transform child = rotatingPivot.GetChild(0);
            child.localRotation = Quaternion.Euler(child.localRotation.eulerAngles.x, 0, child.localRotation.eulerAngles.z);
        }
        else if (onLeftWall)
        {
            rotatingPivot.up = Vector2.right;
            Transform child = rotatingPivot.GetChild(0);
            child.localRotation = Quaternion.Euler(child.localRotation.eulerAngles.x, 180, child.localRotation.eulerAngles.z);
        }
        else if (onRightWall)
        {
            rotatingPivot.up = Vector2.left;
            Transform child = rotatingPivot.GetChild(0);
            child.localRotation = Quaternion.Euler(child.localRotation.eulerAngles.x, 180, child.localRotation.eulerAngles.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager_PK.GetInstance().Play("Slap", UnityEngine.Random.Range(0.9f, 1.1f));
    }

    public bool IsOnAir()
    {
        return onAir;
    }
}
