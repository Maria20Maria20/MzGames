# MzGames

A Unity 3D simulation where **M** animals each chase their own **food** target across an **N×N** grid at movement speed **V**, with boid-style separation, autosave, and a resumable session — built on a DI-driven architecture (state machine + [VContainer](https://github.com/hadashiA/VContainer) + Addressables + JSON save/load).

## Tech stack

- **Engine:** Unity `6000.0.42f1` (Unity 6), Universal Render Pipeline
- **DI:** [VContainer](https://github.com/hadashiA/VContainer) `1.18.0`
- **Content delivery:** Unity Addressables `2.3.16`
- **Serialization:** Newtonsoft Json for Unity `3.2.2`

## Running the project

1. Clone the repository and open it in **Unity Hub** (*Add project from disk*).
2. Let Unity finish importing and resolving packages on first open (Addressables/VContainer).
3. **Open the `Bootstrap` scene:** `Assets/MzGames/Scenes/Bootstrap.unity`.
   This scene hosts the DI composition root (`RootLifetimeScope`) and is the **only scene meant to be opened and played directly**. The `Meta` (menu) and `TestTask` (simulation) scenes are Addressable content, loaded dynamically at runtime by the app's state machine — opening them on their own will not run the game correctly.
4. Press **Play**. Boot-up initializes core services, warms up Addressables, and takes you to the main menu automatically.

## Save data

Save files are written to `Application.persistentDataPath`:

- `simulation.json` — last autosaved simulation snapshot (config, animal and food positions/velocities)
- `menu-settings.json` — last-used menu configuration

Application flow (`GameStateMachine`):
`BootstrapState → LoadProgressState → MenuState → LoadSimulationState → GameLoopState`
