using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditorManager : MonoBehaviour
{
    private int[,] _matrix;
    private int _currentSprite { get; set; }
    public GameObject SpawnOrigin;
    public GameObject EmptyObject;
    public int MatrixWidth = 20;
    public int MatrixHeight = 12;
    public GameObject CurrentObject;
    public TMP_Text TextLogCSV;
    public TMP_Text LevelSelectorText;
    public Button OpenCSV_Button;
    public Button SaveCSV_Button;
    [Serializable]
    public struct MatrixValues {
        public Sprite Sprite;
        public int Value;
    }
    public MatrixValues[] SpritesValues;

    void Start()
    {
        _currentSprite = 1;
        CurrentObject.GetComponent<SpriteRenderer>().sprite = EmptyObject.GetComponent<SpriteRenderer>().sprite;
        SaveCSV_Button.onClick.AddListener(saveCSV);
        OpenCSV_Button.onClick.AddListener(openCSV);

        InitializeMatrix(MatrixWidth, MatrixHeight); // Cambiado aquí
        SpawnObjectsMatrix();
    }

    public void InitializeMatrix(int width, int height)
    {
        _matrix = new int[width, height];
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                _matrix[i,j] = -1;
            }
        }
    }

    private void SpawnObjectsMatrix()
    {
        // Obtiene el ancho del sprite del prefab.
        float spriteWidth = EmptyObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        // Obtiene la altura del sprite del prefab.
        float spriteHeight = EmptyObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        // Calcula el punto inicial donde se instanciarán los objetos.
        Vector3 startPosition = SpawnOrigin.transform.position;

        // Itera sobre la cantidad de objetos y los instancia uno al lado del otro.
        for (int j = 0; j < _matrix.GetLength(1); j++) // Itera por las filas
        {
            for (int i = 0; i < _matrix.GetLength(0); i++) // Luego por las columnas
            {
                // Calcula la posición del objeto actual en la iteración.
                Vector3 position = startPosition + new Vector3(spriteWidth * i, -spriteHeight * j, 0f);

                // Instancia el objeto y le asigna la posición.
                GameObject newObject = Instantiate(EmptyObject, position, Quaternion.identity);
                newObject.tag = "EditorSprite";
                _matrix[i, j] = -1;

                // Modifica la escala del objeto para hacerlo de la mitad del tamaño original
                newObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

                StringBuilder builder = new StringBuilder();
                builder.Append(i);
                builder.Append("_");
                builder.Append(j);
                newObject.name = builder.ToString();
            }
        }
    }

    private void SpawnObjectsMatrixFromCSV()
    {
        // Obtiene el ancho del sprite del prefab.
        float spriteWidth = EmptyObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        // Obtiene la altura del sprite del prefab.
        float spriteHeight = EmptyObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        // Calcula el punto inicial donde se instanciarán los objetos.
        Vector3 startPosition = SpawnOrigin.transform.position;

        // Itera sobre la cantidad de objetos y los instancia uno al lado del otro.
        for (int i = 0; i < _matrix.GetLength(1); i++) // Itera por las filas
        {
            for (int j = 0; j < _matrix.GetLength(0); j++) // Luego por las columnas
            {
                // Calcula la posición del objeto actual en la iteración.
                Vector3 position = startPosition + new Vector3(spriteWidth * j, -spriteHeight * i, 0f);

                // Instancia el objeto y le asigna la posición.
                GameObject newObject = Instantiate(EmptyObject, position, Quaternion.identity);
                SpriteRenderer newObjectSprite = newObject.GetComponent<SpriteRenderer>();

                // Obtiene el valor de la matriz CSV en las coordenadas actuales (x, y)
                int value = _matrix[j, i]; // Ajustado el orden de las dimensiones

                // Asigna el sprite correspondiente al valor de la matriz CSV
                newObjectSprite.sprite = GetSpriteForValue(value);
                newObject.tag = "EditorSprite";

                // Modifica la escala del objeto para hacerlo de la mitad del tamaño original
                newObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

                StringBuilder builder = new StringBuilder();
                builder.Append(j); // Ajustado el orden de las coordenadas en el nombre del objeto
                builder.Append("_");
                builder.Append(i);
                newObject.name = builder.ToString();
            }
        }
    }


    public void ChangeCurrentSprite(GameObject select)
    {
        SpriteRenderer currentObjectSprite = CurrentObject.GetComponent<SpriteRenderer>();
        currentObjectSprite.sprite = select.GetComponent<SpriteRenderer>().sprite;
    }

    public void ChangeEditorSprite(GameObject select)
    {
        SpriteRenderer selectSprite = select.GetComponent<SpriteRenderer>();
        selectSprite.sprite = CurrentObject.GetComponent<SpriteRenderer>().sprite;
        int value = GetValueForSprite(CurrentObject.GetComponent<SpriteRenderer>().sprite);
        string[] position = select.name.Split('_');
        ChangeMatrix(Int32.Parse(position[0]), Int32.Parse(position[1]), value);
    }

    public void ChangeMatrix(int x, int y, int value)
    {
        _matrix[x, y] = value;
    }

    private int GetValueForSprite(Sprite sprite)
    {
        foreach (MatrixValues matrixValue in SpritesValues)
        {
            if (matrixValue.Sprite == sprite)
            {
                return matrixValue.Value;
            }
        }
        // Si el sprite no se encuentra en la lista, devuelve un valor predeterminado o lanza una excepción, según lo que prefieras.
        return 0; // Por ejemplo, devolviendo 0 si no se encuentra.
    }

    private Sprite GetSpriteForValue(int value)
    {
        foreach (MatrixValues matrixValue in SpritesValues)
        {
            if (matrixValue.Value == value)
            {
                return matrixValue.Sprite;
            }
        }
        return null;
    }

    private void openCSV()
    {
        // Ruta del archivo CSV que deseas abrir
        string directoryPath = "C:/SokobanLevels/";
        try
        {
            string filePath = Path.Combine(directoryPath, LevelSelectorText.text + ".csv");
            // Destruye todos los objetos con el tag "EditorSprite"
            GameObject[] editorSprites = GameObject.FindGameObjectsWithTag("EditorSprite");
            foreach (GameObject editorSprite in editorSprites)
            {
                GameObject.Destroy(editorSprite);
            }
            FillMatrix(filePath);
            SpawnObjectsMatrixFromCSV();
        }
        catch (Exception e)
        {
            // Si ocurre un error, muestra un mensaje de error en el log y en el campo TextLogCSV
            Debug.LogError("Error al abrir el archivo CSV: " + e.Message);
            TextLogCSV.text = "Error al abrir el archivo CSV: " + e.Message;
        }
    }

    private bool CheckPlayerAndTargets()
    {
        // Encuentra todos los objetos con el tag "EditorSprite"
        GameObject[] editorSprites = GameObject.FindGameObjectsWithTag("EditorSprite");

        // Contadores para jugadores, cajas y objetivos
        int playerCount = 0;
        int boxCount = 0;
        int targetCount = 0;

        // Itera sobre los objetos encontrados
        foreach (GameObject editorSprite in editorSprites)
        {
            // Obtiene las coordenadas actuales (x, y) del objeto
            string[] position = editorSprite.name.Split('_');
            int x = Int32.Parse(position[0]);
            int y = Int32.Parse(position[1]);

            // Obtiene el valor de la matriz en las coordenadas actuales (x, y)
            int value = _matrix[x, y];

            // Incrementa el contador correspondiente según el valor de la matriz
            switch (value)
            {
                case 3: // Jugador
                    playerCount++;
                    break;
                case 1: // Caja
                    boxCount++;
                    break;
                case 4: // Objetivo
                    targetCount++;
                    break;
                default:
                    break;
            }
        }

        bool check = true;
        // Comprueba si hay exactamente un jugador y si la cantidad de objetivos coincide con la cantidad de cajas
        // Si hay un error, muestra un mensaje de error en el log y en Debug
        string errorMessage = "Error al guardar el archivo CSV:";
        if (playerCount != 1)
        {
            errorMessage += " Debe haber exactamente un jugador en el nivel.";
            check = false;
        }
        if (boxCount != targetCount)
        {
            errorMessage += " La cantidad de objetivos no coincide con la cantidad de cajas.";
            check = false;
        }
        if (boxCount == 0 && targetCount == 0)
        {
            errorMessage += " No hay cajas ni objetivos.";
            check = false;
        }
        else
        {
            if (boxCount == 0)
            {
                errorMessage += " No hay cajas.";
            check = false;
            }
            if (targetCount == 0)
            {
                errorMessage += " No hay objetivos.";
                check = false;
            }
        }
        if(!check)
        {
            Debug.LogError(errorMessage);
            TextLogCSV.text = errorMessage;
        }

        return check;
    }


    private void saveCSV()
    {
        // Realiza la comprobación de jugadores y objetivos
    if (!CheckPlayerAndTargets())
    {
        return; // Si hay un error, sale de la función sin guardar el archivo CSV
    }
    
        StringBuilder csvContent = new StringBuilder();

        // Recorre la matriz y agrega cada valor al contenido CSV
        for (int y = 0; y < MatrixHeight; y++) // Itera sobre las filas
        {
            for (int x = 0; x < MatrixWidth; x++) // Itera sobre las columnas
            {
                // Agrega el valor a la cadena CSV
                csvContent.Append(_matrix[x, y]); // Aquí se corrige el orden de los índices

                // Agrega una coma si no es el último valor de la fila
                if (x < MatrixWidth - 1) // Usa MatrixWidth aquí
                {
                    csvContent.Append(",");
                }
            }

            // Agrega un salto de línea al final de cada fila
            csvContent.AppendLine();
        }

        // Guarda el contenido CSV en un archivo
        string directoryPath = "C:/SokobanLevels/";
        string filePath = Path.Combine(directoryPath, LevelSelectorText.text + ".csv");

        try
        {
            // Guarda el archivo CSV
            System.IO.File.WriteAllText(filePath, csvContent.ToString());
            Debug.Log("Archivo CSV guardado en: " + filePath);
            TextLogCSV.text = "Archivo CSV guardado correctamente en: " + filePath;
        }
        catch (Exception e)
        {
            // Si ocurre un error, muestra un mensaje de error en el log y en el campo TextLogCSV
            Debug.LogError("Error al guardar el archivo CSV: " + e.Message);
            TextLogCSV.text = "Error al guardar el archivo CSV: " + e.Message;
        }
    }

    public void FillMatrix(string filePath)
    {
        List<string[]> matrixData = new List<string[]>();

        using (StreamReader reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                string[] line = reader.ReadLine().Split(',');
                matrixData.Add(line);
            }
        }

        // Establece las dimensiones de la matriz
        MatrixHeight = matrixData.Count;
        MatrixWidth = matrixData[0].Length;

        // Inicializa la matriz con las dimensiones obtenidas
        _matrix = new int[MatrixWidth, MatrixHeight]; // Corregido el orden de las dimensiones

        // Llena la matriz con los datos del archivo CSV
        for (int y = 0; y < MatrixHeight; y++)
        {
            for (int x = 0; x < MatrixWidth; x++)
            {
                _matrix[x, y] = int.Parse(matrixData[y][x]); // Corregido el orden de los índices
            }
        }
    }
}
