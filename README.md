# Editor de Niveles Sokoban

Vídeo: https://youtu.be/v95VmtJh-xE

Este es un editor de niveles para el juego Sokoban, donde puedes crear y editar niveles mediante una interfaz gráfica intuitiva.

## Requisitos

- Unity 2022.3.20f1 o superior.

## Funcionamiento

El editor te permite crear y editar niveles de Sokoban utilizando una matriz bidimensional. Puedes seleccionar diferentes sprites para representar distintos elementos del juego, como jugador, cajas, objetivos y espacios vacíos.
Los niveles se guardan en C:/SokobanLevels/
Recuerda que debes crear los 10 niveles del juego para poder tener una experiencia completa.

### Controles

- Haz clic en un sprite de la paleta para seleccionarlo como el sprite actual.
- Haz clic en una celda de la matriz para colocar el sprite actual en esa ubicación.
- Utiliza los botones "Open CSV" y "Save CSV" para cargar y guardar niveles en formato CSV.
- Utiliza el campo de texto "Selector de Nivel" para especificar el nombre del nivel que deseas abrir o guardar.

### Comprobación de Jugadores y Objetivos

Antes de guardar un nivel, el editor realiza una comprobación para garantizar que haya exactamente un jugador y que la cantidad de objetivos coincida con la cantidad de cajas. Si hay algún error en la configuración del nivel, se mostrará un mensaje de error y no se guardará el nivel.

¡Disfruta creando tus propios desafíos de Sokoban!

