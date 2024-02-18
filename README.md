# Submarine-Adventures
 Imagine you are a submarine captain in search of treasure against a crowd of hungry sharks.

# Game Rules:
Movement is controlled using the W, A, S, D keys. The player can drop a mine, which will explode after 3 seconds by pressing the Space key. The player needs to find the treasure chest and avoid becoming a victim of sharks. At the beginning of the game, the player has 3 health points. The player can die from a mine or an enemy. After three deaths, the game is considered lost. There is a bonus on the game field that, when picked up, increases the player's available lives by one. The game is considered won upon reaching the treasure chest.

# Enemy movement
 The project used ml-agents package for Unity, as well as scripts for teaching enemies to move using machine learning. However, during the training process, the bots never learned to attack the player, moving along the shortest path. The script for training agents is located in the folder Assets\Scripts\AgentEnemyBehaviour.cs. The ML training game field prefab and ML enemy prefab is located in the folder Assets\Prefabs.
 
 As a result, the project implemented the A* algorithm to find the shortest path to the player.

 Due to the game setting, it was decided to use two types of movement: arcade mode and simulator mode. Arcade mode uses changing the speed value to move, while simulator mode uses physical movement.

# Animations
 Animations for schools of fish were created using a DOTWeen package.
