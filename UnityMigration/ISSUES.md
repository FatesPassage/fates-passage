# Fate's Passage Unity migration — active issues

Completed milestones are archived below. Active unresolved issues are numbered from 1.

## Issue 1 — Exact character creation parity (top priority)
- [Committed; Unity verification pending] Source-backed JSON catalog of all ten original specializations and ten star signs from `character-creation.html` blob `c3a10c34ec5289897ed974ecede9f5e1df1eee6f`.
- [Committed; Unity verification pending] Multi-step Unity character creation prototype and `Fate's Passage > Build Character Creation Scene` menu.
- [Open] Recreate original character-creation visuals, portraits, class icons, animations, transitions, sign-sealing behavior, appearance assets, and Naturalist five-companion choice.
- [Open] Exact browser handoff, persistent character state, source-derived validations, and complete step parity.
- [Open] Unity compilation and Play Mode tests for the new scene.

## Issue 2 — Original input parity
- Audit original `fates-passage.html` keyboard and mouse controls, including Q, E, simultaneous key combinations, pointer interactions and camera behavior.
- Audit touch/mobile controls and implement matching touch targets, joystick, gestures and adaptive layout.
- Validate against the browser game; current WASD/Space prototype is not parity.

## Issue 3 — Source art and audio extraction
- Extract embedded media from the original large HTML sources, hash and classify, then import native Unity assets.
- Do not replace original assets with generic art when originals exist.

## Issue 4 — Browser save migration
- Implement and test complete `fatesPassage.save.v1` importer and preservation of separate inventory/corpse records.
- Existing sheet handoff importer is partial foundation, not full save compatibility.

## Issue 5 — Gameplay/combat parity
- Replace prototype movement, skills, cooldowns, combat, enemies, companions and progression with verified browser rules.
- Build deterministic tests and playtest with keyboard, mouse and mobile.

## Verified milestones (not active issues)
- User confirmed Unity prototype arena loads and character movement works.
- Prior blocking C# syntax and duplicate assembly issues were repaired.
- A temporary combat arena and HUD exist; they are not the original game.

## Current batch — November 19, 2026
- Character creation is now the first development priority.
- New source-backed catalog: `Assets/Resources/FatesPassageCharacterCreation.json`.
- New runtime: `Assets/Scripts/Core/CharacterCreationPrototype.cs`.
- New editor menu: `Assets/Editor/CharacterCreationSceneBuilder.cs`.
- Interface update: a centered, safe-area-aware creation panel with stage progression, scrollable path and sign choices, and a review screen.
- **No claim of visual parity or successful Unity compilation yet.** The prototype is functional scaffolding and still lacks original character art and companion choices.
- Keep main untouched; commit on `unity-migration`.
