# Sokoban Level Editor

This is a level editor for the game Sokoban, where you can create and edit levels using an intuitive graphical interface.

## Requirements

- Unity 2022.3.20f1 or higher.

## Functionality

The editor allows you to create and edit Sokoban levels using a two-dimensional matrix. You can select different sprites to represent various game elements, such as the player, boxes, targets, and empty spaces. Levels are saved in C:/SokobanLevels/. Remember to create all 10 levels of the game to have a complete experience.

### Controls

- Click on a sprite in the palette to select it as the current sprite.
- Click on a cell in the matrix to place the current sprite in that location.
- Use the "Open CSV" and "Save CSV" buttons to load and save levels in CSV format.
- Use the "Level Selector" text field to specify the name of the level you want to open or save.

### Player and Target Verification

Before saving a level, the editor performs a check to ensure that there is exactly one player and that the number of targets matches the number of boxes. If there is any error in the level configuration, an error message will be displayed and the level will not be saved.

Enjoy creating your own Sokoban challenges!
