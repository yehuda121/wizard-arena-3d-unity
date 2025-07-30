# Wizard Arena 3D

## Project Summary
Wizard Arena 3D is a third-person 3D shooting game where the player battles waves of enemies using magical powers until reaching the final boss. The game features stage progression, an advanced shooting system, a defensive shield mechanism, menus, dynamic background music, and a real-time HUD. It is designed for single-player mode, with a typical playthrough lasting between 10 to 20 minutes.

## Gameplay Mechanics
- **Tank Movement**: The player moves forward and rotates using arrow keys, resembling tank-style controls.  
- **Spell Shooting via Object Pooling**: Instead of instantiating projectiles, an object pool is used for efficiency.  
- **Defensive Mode (Shield)**: When the player activates the shield, all movement and shooting are disabled. Enemy projectiles are fully blocked, except boss attacks (reduced damage).  
- **First-Person Aiming Mode**: Pressing down arrow moves the camera forward and displays a crosshair for accurate targeting.  
- **Kill Boost System**: After defeating 4 enemies, the player enters a 30-second power-up period where spells deal more damage.

## Controls
- Arrow Up: Move forward  
- Arrow Left / Right: Rotate left / right  
- Arrow Down: Switch to aiming mode (first-person view with crosshair)  
- Space: Shoot magic projectile  
- S: Activate shield  
  - blocks enemy projectiles, disables movement and shooting but not rotation.  
  - Against the boss the damage is reduced but not nullified: only 5% damage is taken instead of 35%.  
- P: Open Cheat Menu (available only in Development Build or Unity Editor)  
- K: Play death animation (visual only – player remains alive and functional)  
- ESC: Open quit confirmation popup ("Are you sure you want to quit the game?")

## Main Scripts Overview
The following scripts implement the core systems of Wizard Arena 3D. Each script handles a specific responsibility in gameplay, UI, or logic.
- `Assets/Scripts/Player/PlayerMovement.cs`  
  Handles tank-style movement and rotation. Includes wall collision detection and triggers the death animation via key press.
- `Assets/Scripts/Player/PlayerShooting.cs`  
  Manages magic shooting, power-up activation every 4 kills, and shooting cooldown. Also controls shield activation and its interaction with movement and animations.
- `Assets/Scripts/Player/SC_WizardAnimator.cs`  
  Controls the player character's animations for walking, shooting, shielding, and dying.
- `Assets/Scripts/Enemy/SC_EnemyController.cs`  
  Implements enemy navigation toward the player, spacing between enemies, auto-firing behavior based on difficulty, and rotation with animation control.
- `Assets/Scripts/Enemy/SC_EnemyHealthSystem.cs`  
  Tracks enemy health, handles death logic, and updates the player's score and power-up triggers.
- `Assets/Scripts/Enemy/SC_EnemyAnimator.cs`  
  Plays attack, death, and movement animations for enemies.
- `Assets/Scripts/BossEnemy/SC_BossEnemy.cs`  
  Controls boss behavior including rotation toward the player, delayed projectile shooting, and integration with boss animations.
- `Assets/Scripts/GameManager/SC_GameManager.cs`  
  Handles stage progression, difficulty changes, boss spawning, and global pause and restart logic.
- `Assets/Scripts/GameManager/SC_InGameMenu.cs`  
  Controls the in-game menu panel, including pause, continue, and restart.
- `Assets/Scripts/GameManager/SC_OpeningManager.cs`  
  Manages the intro video, skip logic, and transition to the game with optional music control.
- `Assets/Scripts/GameManager/VolumeManager.cs`  
  Applies music and SFX volume changes using Unity's AudioMixer and saves preferences with PlayerPrefs.

## Game Flow – Step by Step
### Opening Scene
An opening video is displayed with the option to skip by clicking the "Skip" button.  
Afterward, the game transitions to the main menu, which includes a "Credits" button to display the project credits, a "Play" button to start the game.

### Gameplay Stages
- The player is placed in an arena.  
- Enemies spawn in waves from 3 different spawns.  
- **Stage progression**: every 10 enemies defeated advances the player to a new stage.  
In each stage:  
- The average enemy spawn rate increases according to the difficulty level.  
- The average enemy firing rate increases according to the difficulty level.  
- The background music changes every level.  
- Every 4 enemies defeated grants a 30-second boost during which the player's projectiles deal increased damage.

### Boss Stage
- A single, very powerful enemy appears.  
- Fires a projectile every 4 seconds.  
- A regular hit reduces the player's health by 35%.  
- While the shield is active, damage is reduced to 5%.  
- Hitting a regular enemy deals 25% of their health, while hitting the boss deals only 10% of its health.

### End Conditions
- **Victory**: Defeating the boss, a Victory text is displayed for a few seconds, followed by a return to the main menu.  
- **Defeat**: The player dies (health reaches 0). A Game Over screen is displayed for a few seconds, followed by a return to the main menu with the option to start over.

## Core Features
- Tank-style player movement  
- Magic shooting system with Object Pooling  
- Dynamic aiming mode and camera control  
- Real-time HUD  
- Full menu system  
- Stage-based dynamic music  
- Enemy auto-fire system  
- Advanced shield system  
- Audio control using Unity's AudioMixer  
- Cheat Menu (Development only):  
  - Stage selection (skip to any stage)  
  - Continue from current game state  
  - Refill Health (restore player health to 100%)

## Quit Confirmation Popup
- When pressing the Escape key during gameplay or in the opening menu, a confirmation dialog appears:  
  `Are you sure you want to quit the game?`

- The player can choose:  
  - Yes – The game closes immediately.  
  - No – The popup is dismissed and gameplay continues.

## Enemy AI Behavior
The enemies use a manually implemented basic AI system.  
They detect the player's position, move toward them while maintaining a minimum distance, and automatically shoot at varying intervals based on difficulty.  
Additionally, they include local avoidance logic to reduce crowding between enemies.  
Although the implementation does not rely on Unity's built-in NavMesh or state machines, it effectively demonstrates core AI behaviors such as tracking, pursuit, shooting, and collision-aware movement.

## Credits
The full project credits can be viewed from the main menu by clicking the "Credits" button.  
The credits include:
- Solo Developer: Yehuda Shmulevitz  
- Music: Opening and stage tracks – credited to artists such as Kevin MacLeod, Darren Curtis, Adrian von Ziegler, and others  
- Characters: Mixamo – used for the player, enemies, and boss models  
- All other code, systems, and scenes were built entirely from scratch by the developer.

## Project Management
Throughout development, the project was fully managed using Git and GitHub.  
All game code, scenes, assets, and documentation were version-controlled and backed up online.  
Although developed individually, GitHub was used to maintain a clean, organized, and trackable development workflow.

**GitHub Repository**:  
https://github.com/yehuda121/wizard-arena-3d-unity

## Installation and Running
The game is built using Unity and provided in two versions:

1. Development Build (for testing and debugging with cheat menu)  
2. Production Build (final play version)

To run the game:
- Open the final build folder (`WizardArena3D\Builds\Dev` or `WizardArena3D\Builds\Release`)  
- Launch the main executable file (e.g., `WizardArena3D.exe`)

**System Requirements:**
- OS: Windows 10 or later  
- RAM: 2GB minimum  
- Graphics: OpenGL/DirectX compatible GPU

## YouTube link to watch the game Overview  
https://youtu.be/msOr3By3SdQ
