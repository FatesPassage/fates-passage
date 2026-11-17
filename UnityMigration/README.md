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
