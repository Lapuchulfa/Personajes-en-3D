using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    private bool touched = false;

    private void OnTriggerEnter(Collider other)
    {
        // evitar multiples colisiones
        if (touched)
            return;

        // misma logica que la moneda
        if (!IsPlayerCollider(other))
            return;

        touched = true;

        // reiniciar nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    bool IsPlayerCollider(Collider other)
    {
        return other.CompareTag("Player") ||
               other.GetComponentInParent<PlayerController>() != null;
    }
}