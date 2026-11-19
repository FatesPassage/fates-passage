# Fate's Passage Unity migration — active issue register

Completed work is intentionally kept out of this list. Active issue numbering restarts at 1.

## Open issues

### Issue 1 — Browser save compatibility importer
- Extract the complete `fatesPassage.save.v1` and `fatesPassage.sheet.v1` schemas from the original game.
- Map world position, orientation, quests, companion, boss state, skills/cooldowns, hotbars, resources, attributes and inventory into versioned Unity save data.
- Build a safe JSON import preview and preserve the original browser save untouched.
- Add round-trip and malformed-save tests.

### Issue 2 — Full character-creation parity
- Recover the exact selectable ancestry, specialization, appearance, sign and starting-choice data from `character-creation.html`.
- Replace generic free-text ancestry/specialization selection with source-backed definitions.
- Recreate the multi-step selection and preview UI in native Unity UI.

### Issue 3 — Original art and 3D asset inventory
- Extract embedded image/audio payloads from the source HTML using hashes and a manifest.
- Classify models, textures, portraits, icons, audio and effects; identify which assets are images versus procedural Three.js geometry.
- Import assets with appropriate Unity formats, compression, mipmaps and platform settings.

### Issue 4 — Actual gameplay rules and combat parity
- Recover the original combat, NPC, skill, cooldown, companion, damage, enemy and boss rules.
- Replace provisional Unity combat formulas and placeholder attacks only after source verification.
- Add deterministic tests for each verified mechanic.

### Issue 5 — Native Input System and controller support
- Replace temporary Legacy Input Manager calls with Unity Input System actions.
- Support keyboard/mouse and gamepad; add remapping and interaction prompts.
- Preserve the currently working temporary controls until the replacement is tested.

## Completed milestones (not active issues)
- Unity Editor opens the local project after resolving duplicate assembly definition and missing preprocessor terminator.
- Combat prototype scene generated locally.
- User confirmed keyboard movement works in Play Mode.
- GitHub migration branch contains the current prototype scaffold and source-backed Corin starter-sheet extraction.

## Verification gates
Do not mark an issue complete merely because code was committed. Require Unity compilation, relevant automated tests, Play Mode validation, and source parity review as appropriate. Keep the main branch unchanged; migration work stays on `unity-migration` until reviewed.
