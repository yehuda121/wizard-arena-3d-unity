# Wizard Arena 3D

Third-person arena wizard shooter built in **Unity 2022.3.50f1 LTS**. Fight through staged enemy waves, unlock a temporary power boost, and defeat a boss in a compact single-player run (typically 10–20 minutes).

[![Gameplay Overview](https://img.youtube.com/vi/msOr3By3SdQ/0.jpg)](https://youtu.be/msOr3By3SdQ)

> **Repository:** [github.com/yehuda121/wizard-arena-3d-unity](https://github.com/yehuda121/wizard-arena-3d-unity)

---

## Overview

Wizard Arena 3D is a portfolio-grade Unity project showcasing gameplay systems, object pooling, difficulty scaling, touch/mobile input, and polished feedback loops—without relying on external gameplay packages.

You play as a wizard in a dungeon arena. Enemies spawn from multiple points, fire projectiles, and escalate in pressure across four difficulty stages until the boss appears.

---

## Gameplay Features

- **Tank-style movement** — forward motion + rotation (keyboard arrows or mobile buttons)
- **Magic shooting** with object-pooled projectiles and cooldown-based fire
- **Hold-to-aim mode** — camera shifts and crosshair appears for precision
- **Shield (hold)** — blocks normal enemy projectiles; boss attacks are reduced, not nullified
- **Kill boost** — every 4 kills grants ~30 seconds of increased projectile damage
- **Stage progression** — Easy → Medium → Hard → Boss (10 kills per stage threshold)
- **Dynamic music** — track changes per difficulty stage
- **Combat feedback** — hit VFX, damage flash, camera shake, and reused SFX
- **End screens** — Game Over / Victory with **Restart** and **Main Menu** (player choice, no forced auto-exit)
- **Mobile touch controls** — six on-screen buttons; auto-hidden on desktop unless enabled
- **Audio mixer** — separate music/SFX volume with PlayerPrefs persistence

---

## Controls

### Keyboard (desktop)

| Input | Action |
|--------|--------|
| ↑ | Move forward |
| ← / → | Rotate left / right |
| ↓ (hold) | Aim mode (camera + crosshair) |
| Space (hold) | Shoot magic projectile |
| S (hold) | Shield (blocks movement forward; blocks most enemy shots) |
| ESC | Quit confirmation popup |
| P | Cheat menu *(Editor / Development Build only)* |
| K | Play death animation *(debug visual only)* |

### Mobile touch (6 buttons)

| Button | Action |
|--------|--------|
| Forward | Move forward |
| Left / Right | Rotate |
| Aim (hold) | Aim mode |
| Shield (hold) | Shield |
| Shoot (hold) | Fire projectile |

Mobile overlay auto-shows on mobile platforms or viewports under 1024px wide. Toggle **Show Mobile Controls** in the in-game pause menu, or set PlayerPrefs key `ShowMobileControls` (`0` = off, `1` = on; absent = auto).

---

## Technical Highlights

- **Object pooling** for player/enemy projectiles and enemies (`PlayerProjectilePool`, `EnemyProjectilePool`, `SC_EnemyPool`)
- **Singleton-style services** for mobile input and combat feedback
- **Percent-based combat** — fixed hit counts per target type (no random damage variance)
- **Difficulty-driven spawner** — spawn interval and enemy fire delays scale by stage
- **PlayerPrefs** for difficulty, volume, mobile controls, and opening-video skip
- **Built-in render pipeline** — lightweight, no URP/HDRP dependency
- **English-only code comments** throughout gameplay scripts

---

## Architecture / Systems

```
OpeningScene          MainArena
    │                     │
    ▼                     ├── SC_GameManager (stages, boss spawn, pause/restart)
SC_OpeningManager       ├── SC_EnemySpawner + SC_EnemyPool
    │                     ├── Player (Movement, Shooting, Health, Animator)
    └── Load MainArena    ├── SC_MobileInputController + MobileControlsCanvas
                          ├── SC_CombatFeedback (VFX/SFX hooks)
                          ├── SC_EndScreenController (Game Over / Victory UI)
                          ├── SC_MusicManager / VolumeManager
                          └── UI (HUD, pause menu, crosshair, end screens)
```

| Folder | Responsibility |
|--------|----------------|
| `Assets/Scripts/Player/` | Movement, shooting, health, camera, animation |
| `Assets/Scripts/Enemy/` | AI, health, spawner, pooling, animators |
| `Assets/Scripts/BossEnemy/` | Boss behavior, health, victory flow |
| `Assets/Scripts/Projectiles/` | Player/enemy/boss projectile logic + pools |
| `Assets/Scripts/GameManager/` | Game flow, menus, music, volume, cheats |
| `Assets/Scripts/UI/` | HUD, mobile input, end screens, crosshair |
| `Assets/Scripts/Feedback/` | Combat VFX/SFX coordination |

---

## Opening Locally in Unity

1. Install **Unity Hub** with editor **2022.3.50f1** (or compatible 2022.3 LTS).
2. Clone the repository and open the project folder in Unity Hub.
3. Open **`Assets/Scenes/OpeningScene.unity`** for the menu/intro flow, or **`Assets/Scenes/MainArena.unity`** to jump straight into gameplay.
4. Press **Play**.

First launch may take a moment while Unity imports assets and compiles scripts.

---

## Building

### From Unity Editor

1. **File → Build Settings**
2. Add scenes: `OpeningScene` (index 0), `MainArena` (index 1)
3. Target **Windows** (or your platform)
4. **Build** or **Build And Run**

### Existing build folders (if present)

- `Builds/Dev` — development build (cheat menu via **P**)
- `Builds/Release` — production build

**Suggested minimum specs:** Windows 10+, 2 GB RAM, DirectX/OpenGL-capable GPU.

---

## Screenshots & Media

<!-- Add screenshots here when available -->
<!-- ![Main Arena Gameplay](Docs/screenshots/gameplay.png) -->
<!-- ![Boss Fight](Docs/screenshots/boss.png) -->
<!-- ![Mobile Controls](Docs/screenshots/mobile-controls.png) -->

**Gameplay video:** [YouTube overview](https://youtu.be/msOr3By3SdQ)

---

## Known Limitations

- Tank controls only (no strafe/backpedal)
- Enemy AI uses direct pursuit + separation, not NavMesh pathfinding
- Limited dedicated SFX library (some sounds reused with pitch variation)
- Enemy death/grounding polish deferred to a future pass
- HUD layout is functional; full visual redesign not yet applied
- Cheat menu available in Editor/Development builds only

---

## Future Improvements

- Enemy death animation grounding and Y-position fix
- Walk animation speed tuning vs movement speed
- Richer hit/block/damage SFX set
- HUD visual pass (icons, layout, stage transition banners)
- Scene lighting/fog/atmosphere pass
- Optional gamepad support

---

## Credits & Third-Party Assets

| Asset | Use |
|-------|-----|
| **Mixamo** | Player, enemy, and boss character models/animations |
| **Gridness Studios – Elementary Dungeon Pack Lite** | Arena environment |
| **Hovl Studio – Procedural Fire** | Projectile and hit VFX |
| **TextMesh Pro** | UI text |
| **Kevin MacLeod, Darren Curtis, Adrian von Ziegler, et al.** | Music (see in-game Credits) |

**Solo developer:** Yehuda Shmulevitz — code, systems, scenes, integration, and documentation.

Full credits are also available in-game via the **Credits** button on the opening menu.

---

## License & Portfolio Use

This project is intended as a portfolio demonstration of Unity gameplay programming. Third-party assets remain subject to their respective licenses. Contact the repository owner before redistributing asset packs or commercial use.

---

## Project Management

Developed with **Git** and **GitHub** for version control, issue tracking, and portfolio visibility.
