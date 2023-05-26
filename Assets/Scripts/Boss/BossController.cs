using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour, IVida
{
    #region Public Properties
    public float WakeDistance = 5f;
    public float Speed = 2f;
    //public bool soltar;

    #endregion

    #region Components
    public Transform Player;
    public GameObject Charcos;
    public GameObject EnemigosPref;
    public SpriteRenderer spriteRenderer { private set; get; }
    public Rigidbody2D rb { private set; get; }
    public Animator animator { private set; get; }

    public bool AttackingEnd { set; get; } = false;
    public Transform hitBox { private set; get; }

    public float VidaInicial;
    public float VidaActual { get; set; }

    [SerializeField] private Image barraVida;

    #endregion

    #region Private Properties
    private FSM<BossController> mFSM;

    #endregion

    private void Start()
    {
        //soltar = false;
        VidaActual = VidaInicial;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hitBox = transform.Find("HitBox");
        Player = GameObject.Find("Player").transform;

        // Creo la maquina de estado finita
        mFSM = new FSM<BossController>(new Boss.BossIdleState(this));
        mFSM.Begin();  // prendo la mquina de estados
        InvokeRepeating("SoltarCharcos", 5f, 2f);
        InvokeRepeating("InstanciarEnemigos",7f, 4f);
    }

    void Update()
    {
        barraVida.fillAmount = VidaActual / VidaInicial;
    }

    private void FixedUpdate()
    {
        mFSM.Tick(Time.fixedDeltaTime);
    }

    public void SetAttackingEnd()
    {
        AttackingEnd = true;
    }

    private void SoltarCharcos()
    {
        //soltar = true;
        Instantiate(Charcos, transform.position, Quaternion.identity);
    }

    private void InstanciarEnemigos()
    {
        Instantiate(EnemigosPref, transform.position, Quaternion.identity);
    }

    public void InstanciarEnemigosFrente()
    {
        Instantiate(EnemigosPref, transform.position, Quaternion.identity);
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
        if (col.gameObject.CompareTag("AtaquePlayer"))
        {
            Debug.Log("Enemigo Abatido");
            QuitarVida(1);
        }
    }
}
