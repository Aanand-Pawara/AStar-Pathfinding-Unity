

## Procedural Generation using Perlin Noise and Dynamic A* Grid Pathfinding

### Overview

This project showcases a procedural terrain generation system using Perlin noise and a dynamic pathfinding system using the A* algorithm. The combination of these techniques allows for the creation of a dynamic and interactive world where terrain is generated on-the-fly and pathfinding adapts to changes in the environment.

### Features

- **Procedural Terrain Generation:**
  - Utilizes Perlin noise to generate realistic terrain features such as grass, dirt, sand, elevated terrain, and water.
  - Adjustable parameters for terrain generation, including noise scale, thresholds for different terrain types, and seed for randomness.

- **Dynamic A* Pathfinding:**
  - Implements the A* algorithm for finding the shortest path between points on a grid.
  - Dynamically updates paths when obstacles are added or removed, ensuring real-time responsiveness.
  - Supports toggling nodes between walkable and non-walkable states to simulate dynamic environment changes.

- **Editor Integration:**
  - Custom editor scripts for ease of use and visualization within the Unity Editor.
  - Interactive buttons and sliders for generating the terrain and adjusting simulation speed.
  - Property drawers for ScriptableObjects to manage prefab references conveniently.

### Components

- **WorldPrefabs ScriptableObject:**
  - Holds references to various terrain prefabs (grass, dirt, elevated dirt, water, sand).
  - Easily expandable and editable within the Unity Editor.

- **Generator:**
  - Main script responsible for terrain generation and pathfinding initialization.
  - Generates a grid-based world with various terrain types based on Perlin noise.
  - Handles pathfinding grid creation and updates.

- **TileInfo:**
  - Component attached to each terrain tile to store tile-specific information.
  - Includes methods for toggling walkable state and interacting with pathfinding nodes.

- **Pathfinding:**
  - Implements the A* algorithm for efficient pathfinding.
  - Dynamically recalculates paths as the environment changes.

### Getting Started

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/MelloMNDRiN/BlackMARCH.git
   ```

2. **Open in Unity:**
   - Open the cloned project in Unity.

3. **Set Up Prefabs:**
   - Assign the appropriate prefabs to the `WorldPrefabs` ScriptableObject in the Unity Editor.

4. **Generate Terrain:**
   - Use the custom editor window to generate the terrain and visualize the grid.

5. **Run and Interact:**
   - Play the scene and interact with the terrain. Use the provided buttons to toggle tile states and observe dynamic pathfinding in action.

### Usage

- **Terrain Generation:**
  - Adjust parameters like grid size, noise scale, and thresholds in the Generator script.
  - Click the "Generate Grid" button in the custom editor window to create the terrain.

- **Dynamic Pathfinding:**
  - Click on terrain tiles during runtime to toggle their walkable state.
  - Observe how the pathfinding system adapts to changes in the terrain.

### Contributions

Contributions are welcome! If you have ideas for improvements or new features, feel free to fork the repository and submit a pull request.

### License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

### Acknowledgements

- Inspired by various tutorials and resources on procedural generation and pathfinding algorithms.
- Utilizes Unity's powerful tools and features to create an interactive and dynamic world.

---
