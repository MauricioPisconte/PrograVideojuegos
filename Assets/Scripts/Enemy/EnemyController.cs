using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IVida
{
    #region Public Properties
    public float WakeDistance = 5f;
    public float Speed = 2f;
    public float AttackDistance = 1f;

    #endregion

    #region Components
    public Transform Player;
    public SpriteRenderer spriteRenderer {private set; get;}
    public Rigidbody2D rb { private set; get; }
    public Animator animator { private set; get; }
    
    public bool AttackingEnd { set; get; } = false;
    public Transform hitBox { private set; get; }

    public float VidaInicial;
    public float VidaActual { get; set; }

    #endregion

    #region Private Properties
    private FSM<EnemyController> mFSM;

    #endregion

    private void Start()
    {
        VidaActual = VidaInicial;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hitBox = transform.Find("HitBox");
        Player = GameObject.Find("Player").transform;

        // Creo la maquina de estado finita
        mFSM = new FSM<EnemyController>(new Enemy.IdleState(this));
        mFSM.Begin();  // prendo la mquina de estados
    }


    private void FixedUpdate()
    {
        mFSM.Tick(Time.fixedDeltaTime);
    }

    public void SetAttackingEnd()
    {
        AttackingEnd = true;
    }

    public virtual void QuitarVida(int cantidad)
    {
        VidaActual -= cantidad;
        if (VidaActual <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("AtaquePlayer"))
        {
            Debug.Log("Enemigo Abatido");
            QuitarVida(2);
        }
    }
}
