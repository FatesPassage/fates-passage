# Fate's Passage — Unity migration

This directory is the **new native Unity implementation**, maintained separately from the original browser game. The web game remains untouched.

## Current status
- Bootstrap script, serializable character profile, JSON save/load foundation, and initial EditMode test added.
- These scripts are **not yet compiled or play-tested in Unity**.
- Character fields are provisional until the browser character-creation data model is fully audited.
- No claim is made that existing browser saves or visual assets have been migrated.

## Suggested Unity setup
1. Install Unity Hub and a compatible Unity 6 LTS editor on macOS.
2. Create a new **Universal 3D (URP)** project named `FatesPassageUnity`.
3. Copy the contents of this directory's `Assets` into the Unity project's `Assets` directory.
4. Add the eventual `CharacterCreation` scene to Build Settings before calling `StartNewGame`.

## Migration sequence
1. Inventory source pages, scripts, images, audio, data and browser storage.
2. Extract game rules into engine-independent data definitions.
3. Implement character creation and persistence.
4. Implement world, movement, combat, UI, inventory, skills and progression.
5. Port art and effects; profile performance and run automated and device tests.

## Safety
Do not delete or overwrite the original HTML/PWA implementation. Work on the `unity-migration` branch and merge only after verification.

## Incremental performance foundation (November 18, 2026)
- `Assets/Scripts/Core/FixedTickClock.cs`: bounded 60 Hz simulation event source, with configurable tick rate and catch-up limit.
- `Assets/Scripts/Core/MovementMotor.cs`: CharacterController-based motion/gravity adapter (provisional; not yet matched to browser movement).
- `Assets/Scripts/Core/FramePerformanceMonitor.cs`: opt-in FPS and average frame-time overlay.
- UI polish planned: compact, toggleable diagnostics HUD that does not obscure the gameplay interface.

### Wiring caution
Call MovementMotor.Simulate from exactly one update loop. If using FixedTickClock, subscribe to Tick and supply movement input through an adapter. CharacterController movement is not equivalent to Rigidbody physics; validate original controls and collision before treating this as a replacement.

### Verification status
The new scripts were committed but have **not** been compiled or play-tested in Unity. No performance improvement is claimed until baseline/profiler comparisons are collected.

## Native combat foundation — November 18, 2026
- Portable `CharacterVitals` and `CombatMath` provide provisional resource/damage rules with EditMode tests.
- `Combatant` connects damage to health, optional hit flash, and `Damaged`/`Defeated` events.
- `CombatFeedback` temporarily tints compatible URP materials without replacing their shared material.
- `CharacterVitalsHud` displays compact health/mana/stamina bars; `PerformanceHud` toggles with F3.
- `PlayerInputAdapter` currently requires the legacy Unity Input Manager enabled (or Both) and uses Horizontal/Vertical axes.
- The native UI is **temporary**; original character HUD, art, gameplay rules, and source assets have not yet been ported.

### Validation checklist
- [ ] Open Unity project and compile scripts
- [ ] Run EditMode tests through Unity Test Runner
- [ ] Create a test scene with CharacterController, MovementMotor, PlayerInputAdapter and a camera
- [ ] Add CharacterVitalsBehaviour, Combatant, CombatFeedback, CharacterVitalsHud and verify hit feedback and resource UI
- [ ] Verify performance and gameplay against the original HTML implementation

## Combat prototype scene generator
From Unity Editor, choose **Fate's Passage > Build Combat Test Scene**. This creates `Assets/Scenes/CombatPrototype.unity` with a capsule player, a training dummy, a camera, lighting, an arena floor and prototype HUD. The scene is not an imported original-game level.

### Temporary controls
- WASD / arrow keys: move via legacy Horizontal/Vertical input axes
- Space: attack the training dummy within 3 Unity units
- F3: toggle the compact FPS overlay

Unity **Active Input Handling** must be set to **Input Manager (Old)** or **Both** for these temporary controls. The scene uses default primitive meshes, not original game assets. Test it in Play Mode before claiming it is playable.

### Current prototype limitations
- Enemy is stationary; no AI, animations, targeting UI, or loot.
- Abilities and combat values are provisional, not validated against browser rules.
- Scene generation and tests have not run in a Unity Editor here.
- The visual HUD is a diagnostic stand-in, not a replacement for the original interface.
- Do not merge to main until Unity compile, test runner, gameplay and visual parity checks pass.

## Visual prototype upgrade
The editor scene builder now generates a stylized dark-fantasy arena with reusable saved materials (obsidian stone, aged bronze, jade player, ember training dummy), perimeter architecture, pillars, inlaid floor lines, warm directional lighting, cool point fill, and restrained distance fog. Camera-relative movement is wired to the generated camera transform.

To apply the updated builder to an existing local Unity project, replace `Assets/Editor/PrototypeSceneBuilder.cs` with the latest branch copy, let Unity recompile, then run **Fate's Passage > Build Combat Test Scene** again. This regenerates `Assets/Scenes/CombatPrototype.unity`; save or back up any local edits to that scene first.

The visual upgrade remains a prototype style pass using Unity primitives and generated materials. Original Fate's Passage models, textures, environments and effects still need to be audited and ported. Re-run in Play Mode and check the Console before considering it verified.
