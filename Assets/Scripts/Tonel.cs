using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonel : MonoBehaviour
{
    public float attackRange;
    public LayerMask Entidad;
    public int damage;
    bool explotar = true;
    public Sprite Explosion;
    private SpriteRenderer spriteRenderer;
    public GameObject particleSystemPrefab;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Activar()
    {
        if (explotar)
        {
            Collider2D[] explosion = Physics2D.OverlapCircleAll(transform.position, attackRange, Entidad);
            explotar = false;
            foreach (Collider2D entity in explosion)
            {
                if (entity.gameObject.CompareTag("Enemigo"))
                {
                    IVida vidaEnemigo = entity.gameObject.GetComponent<IVida>();
                    vidaEnemigo.QuitarVida(damage);
                }
                if (entity.gameObject.CompareTag("Boss"))
                {
                    IVida vidaEnemigo = entity.gameObject.GetComponent<IVida>();
                    vidaEnemigo.QuitarVida(damage-2); ;
                }
                if (entity.gameObject.CompareTag("Tonel"))
                {
                    Debug.Log("cadena");
                    entity.GetComponent<Tonel>().Invoke("Activar", 0.5f);
                }

            }
            Destruir();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "AtaquePlayer") Activar();
    }

    void OnDrawGizmosSelected()
    {
        if (transform.position == null) return;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Destruir()
    {
        //Reproducir particulas
        spriteRenderer.sprite = Explosion;
        Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 1f);
    }

}
