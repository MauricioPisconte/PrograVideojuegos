using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour, IVida
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private GameObject Tonel;
    [SerializeField] private Image LClick;
    [SerializeField] private Image RClick;

    [SerializeField] private Image barraVida;

    private Rigidbody2D mRb;
    private Vector3 mDirection = Vector3.zero;
    private Animator mAnimator;
    private PlayerInput mPlayerInput;
    private Transform hitBox;
    

    public float VidaInicial;
    public float VidaActual { get; set; }

    private void Start()
    {
        VidaActual = VidaInicial;
        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mPlayerInput = GetComponent<PlayerInput>();

        hitBox = transform.Find("HitBox");

        ConversationManager.Instance.OnConversationStop += OnConversationStopDelegate;
        InvokeRepeating("IncrementarVidas", 10f, 10f);
    }

    private void OnConversationStopDelegate()
    {
        mPlayerInput.SwitchCurrentActionMap("Player");
    }

    private void Update()
    {
        if (mDirection != Vector3.zero)
        {
            mAnimator.SetBool("IsMoving", true);
            mAnimator.SetFloat("Horizontal", mDirection.x);
            mAnimator.SetFloat("Vertical", mDirection.y);
        }else
        {
            // Quieto
            mAnimator.SetBool("IsMoving", false);
        }

        barraVida.fillAmount = VidaActual / VidaInicial;
    }

    private void IncrementarVidas()
    {
        QuitarVida(-1);
    }

    private void FixedUpdate()
    {
        mRb.MovePosition(
            transform.position + (mDirection * speed * Time.fixedDeltaTime)
        );
    }

    public void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>().normalized;
    }

    public void OnNext(InputValue value)
    {
        if (value.isPressed)
        {
            ConversationManager.Instance.NextConversation();
        }
    }

    public void OnCancel(InputValue value)
    {
        if (value.isPressed)
        {
            ConversationManager.Instance.StopConversation();
        }
    }

    public void OnAttack(InputValue value)
    {
        StopCoroutine(ColorBlancoL());
        if (value.isPressed)
        {
            StartCoroutine(ColorBlancoL());
            mAnimator.SetTrigger("Attack");
            hitBox.gameObject.SetActive(true);
        }
    }

    public void OnTonel(InputValue value)
    {
        StopCoroutine(ColorBlancoR());
        if (value.isPressed)
        {
            StartCoroutine(ColorBlancoR());
            Instantiate(Tonel, transform.position, Quaternion.identity);
        }
        
    }

    IEnumerator ColorBlancoL()
    {
        LClick.color = Color.yellow;
        yield return new WaitForSeconds(0.3f);
        LClick.color = Color.white;
    }

    IEnumerator ColorBlancoR()
    {
        RClick.color = Color.yellow;
        yield return new WaitForSeconds(0.3f);
        RClick.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Conversation conversation;
        if (other.transform.TryGetComponent<Conversation>(out conversation))
        {
            mPlayerInput.SwitchCurrentActionMap("Conversation");
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    public void DisableHitBox()
    {
        hitBox.gameObject.SetActive(false);
    }

    public virtual void QuitarVida(int cantidad)
    {
        VidaActual -= cantidad;
        if (VidaActual <= 0)
        {
            VidaActual = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (VidaActual >= VidaInicial)
        {
            VidaActual = VidaInicial;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("DisparoEnemigo"))
        {
            Debug.Log("Recibe daño");
            QuitarVida(1);
        }
        else if(col.gameObject.CompareTag("Token"))
        {
            GestionEntrada.Revision();
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Activate"))
        {
            GestionEntrada.ActivarJefe();
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.CompareTag("VidaExtra"))
        {
            QuitarVida(-1);
            Destroy(col.gameObject);
        }
        else if(col.gameObject.CompareTag("Charco"))
        {
            QuitarVida(2);
        }
    }
}
