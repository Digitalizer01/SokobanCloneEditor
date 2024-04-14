using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public GameObject prefab; // El prefab que quieres instanciar.
    public int numberOfObjects = 3; // La cantidad de objetos que quieres instanciar.

    private void Start()
    {
        // Obtiene el ancho del sprite del prefab.
        float spriteWidth = prefab.GetComponent<SpriteRenderer>().bounds.size.x;

        // Calcula el ancho total de los objetos basado en el número de objetos y el ancho del sprite.
        float totalWidth = numberOfObjects * spriteWidth;

        // Calcula el punto inicial donde se instanciarán los objetos.
        Vector3 startPosition = transform.position - new Vector3(totalWidth / 2f - spriteWidth / 2f, 0f, 0f);

        // Itera sobre la cantidad de objetos y los instancia uno al lado del otro.
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calcula la posición del objeto actual en la iteración.
            Vector3 position = startPosition + new Vector3(spriteWidth * i, 0f, 0f);

            // Instancia el objeto y le asigna la posición.
            GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
        }
    }

}
