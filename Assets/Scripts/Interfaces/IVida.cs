using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVida
{
    float VidaActual { get; set; }

    static bool isAlive { get; set; }
    void QuitarVida(int cantidad);
}
