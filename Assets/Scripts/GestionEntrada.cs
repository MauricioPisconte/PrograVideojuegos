using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionEntrada : MonoBehaviour
{
    [SerializeField] private GameObject[] tokens;
    /*[SerializeField] private GameObject cajas;
    [SerializeField] private GameObject jefe;
    [SerializeField] private GameObject activacion;*/
    [SerializeField] private GameObject[] activables;
    private static int cantidadTokensRestantes;
    /*private static GameObject activacionC;
    private static GameObject bloqueo;
    private static GameObject jefeStatic;*/
    private static GameObject[] activablesC;

    // Start is called before the first frame update
    void Start()
    {
        cantidadTokensRestantes = tokens.Length;
        activablesC = new GameObject[3];
        for (int i = 0; i <3; i++)
        {
            activablesC[i] = activables[i];
        }

        activablesC[2].SetActive(false);
    }

    public static void Revision()
    {
        cantidadTokensRestantes--;
        if(cantidadTokensRestantes <= 0)
        {
            activablesC[1].SetActive(false);
        }
    }

    public static void ActivarJefe()
    {
        activablesC[0].SetActive(false);
        activablesC[1].SetActive(true);
        activablesC[2].SetActive(true);
    }
}
