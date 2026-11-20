# Fate's Passage Unity migration — active issue register

Completed work is intentionally kept out of this list. Active issue numbering restarts at 1.

## Open issues

### Issue 1 — Browser save compatibility importer
- [Source audit complete] Recover exact `fatesPassage.save.v1` field inventory, restore behavior, and separate `fatesPassage.inv.v1` / `fatesPassage.corpses.v1` keys from the original `fates-passage.html` source.
- [Implementation complete; Unity verification pending] Finish `fatesPassage.sheet.v1` handoff including nullable skill charges, null-preserving slot behavior, and source-observed level-two bonuses.
- Build a safe, previewable importer for the full world snapshot, including position/orientation, companion transforms, primitive state maps, progression flags, boss state, cooldowns, hot-over-time records, dummies, target and auto-attack state.
- Validate nested arrays and preserve unknown primitive fields; treat `lvT` / `xpW` as UI hints rather than authoritative progression.
- Add round-trip, malformed-save, compatibility and no-data-loss tests; never modify the original browser save.

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
- [Committed] Add a shared input facade for Input System keyboard (WASD/arrows), gamepad stick/D-pad, Space/primary-button attack, and F3 diagnostics.
- [Committed] Add the Unity.InputSystem assembly reference and remove remaining prototype Legacy Input Manager-only polling.
- [Open verification] Recompile locally; verify keyboard, mouse focus, gamepad movement/attack, and behavior after switching Active Input Handling. Remapping and contextual prompts remain future work.

## Completed milestones (not active issues)
- Unity Editor opens the local project after resolving duplicate assembly definition and missing preprocessor terminator.
- Combat prototype scene generated locally.
- User confirmed keyboard movement works in Play Mode.
- GitHub migration branch contains the current prototype scaffold and source-backed Corin starter-sheet extraction.

## Verification gates
Do not mark an issue complete merely because code was committed. Require Unity compilation, relevant automated tests, Play Mode validation, and source parity review as appropriate. Keep the main branch unchanged; migration work stays on `unity-migration` until reviewed.

**Progress:** The separate `fatesPassage.sheet.v1` handoff is now partially imported (resource pairs, resonance, and verified level-2 sheet bonuses). Nullable skill-charge import and the full world save remain open.


## Latest development batch — November 19, 2026
- The input implementation is committed but not yet user-verified in Unity; do not close Issue 5 until compile and Play Mode checks pass.
- Prototype target HUD received a cleaner centered target plate, live health bar, and defeated state.
- Performance diagnostics now use the shared input facade and retain the compact optional F3 panel.
- The previous keyboard movement test predates this input refactor; re-test before relying on it.


## Latest batch — November 19, 2026
- Committed null-preserving parsing for the source handoff's `ch` array. Null charge entries preserve existing template values; explicit charges are validated against each skill's maximum.
- Added EditMode cases for non-null charge import, null-slot preservation, invalid charge values, reordered JSON properties and escaped quoted metadata.
- Reopened the original browser runtime and documented exact world-save fields and restore semantics in `SOURCE_AUDIT.md`.
- Interface polish: the lower-left vitals panel now anchors inside `Screen.safeArea` and scales down on narrow screens.
- All new code and tests are committed to `unity-migration`; local Unity compile and test-runner verification are still required.

## Compile recovery batch — November 19, 2026
- Fixed malformed escaped newlines in `PrototypeInputReader.cs` on the migration branch; F3 diagnostic toggle remains available through the shared input facade.
- Removed the redundant `Assets/Tests/EditMode/FatesPassage.EditModeTests.asmdef` to avoid duplicate assembly names; retained the `Assets/FatesPassage/Tests/EditMode` definition.
- Updated `PerformanceHud` to use shared input and a responsive, safe-area-aware top-left overlay with resize-aware typography.
- Local Unity compilation, EditMode tests, and Play Mode verification **not yet confirmed** after these commits.
- User action: refresh the `unity-migration` branch locally and send the first remaining Console errors, if any.
- The GitHub issue tracker has no open issue tickets; this Markdown file remains the active migration register.

## Play Mode milestone and combat visibility — November 19, 2026
- User confirmed the Unity combat arena loads and the player moves in Play Mode. Prior compile and scene-opening blockers are removed from the active troubleshooting queue.
- Added a safe-area-aware, proportional training-target health plate with clearer defeat feedback.
- Exposed a successful-hit counter in the temporary attack controller and wired it into the generated scene HUD to make attack verification observable.
- Scene builder updated; rerun **Fate's Passage > Build Combat Test Scene** after backing up local scene edits to see the new HUD.
- Open verification: Space attack reduces dummy HP, hit counter increases, and the dummy can be defeated; gamepad and F3 still require validation.
- These code changes are committed but not compiled or play-tested in the user's local Unity installation.

## Control-parity batch — November 19, 2026
- Reopened the original `fates-passage.html` Git blob and documented browser keyboard/mouse/mobile controls in `CONTROL_PARITY.md`.
- Implemented provisional desktop mouse orbit/wheel zoom and mobile left movement/right look touch zones in Unity prototype.
- **Important parity gap:** Space remains temporary prototype attack, but original browser Space means jump. F interaction, 1–3 abilities, mobile action buttons and pinch zoom remain open.
- Local Unity compilation, gesture conflict tests, mobile build and UI verification are still pending. No claim of full parity.

## Original controls parity — November 19, 2026
- User explicitly requires original HTML keyboard, mouse, and mobile controls, including Q/E and simultaneous-key combinations.
- Source check: `fates-passage.html` contains a control mapping `q:'rl',e:'rr'` (rotate left/right) in one context, and separate gameplay keydown/keyup handling that records lowercase keys. Do not assume this map is the entire gameplay control scheme.
- Preserve combined states (e.g. movement + Q/E, diagonals), continuous key-down/key-up semantics, mouse-look/interaction where verified, and touch/multitouch gestures after source inspection.
- The current Unity `PrototypeInputReader` supports WASD/arrows, gamepad, attack and diagnostics only. **Q/E, mouse and touch parity are not implemented or verified.**
- Next: recover the exact gameplay controls and touch handler logic from the original source, define a context-sensitive input action specification, implement Unity adapters, and test combined inputs on desktop and mobile.

## Source-backed controls parity batch — November 19, 2026
- Inspected the original `fates-passage.html` Git blob: game loop reads W/A/S/D and arrows for movement; Q adds `dt*1.6` to yaw target and E subtracts it. Inputs are independent, so simultaneous movement + turning is supported. Original also binds F interaction, Space jump, 1–3 skill casts, wheel zoom, and mobile pointer gestures.
- Implemented Q/E yaw in the Unity camera through a shared input facade, including modern Input System and legacy fallback; rotation runs independently of WASD.
- Added compact in-game control guidance to the target HUD.
- **Not yet verified:** exact camera drag semantics, touch UI parity, Space jump versus provisional Space attack, F interaction, skill keys, and full mobile control affordances. Do not claim full parity until these are implemented and tested.
- **Local verification needed:** refresh three updated C# files; test W+Q, W+E, W+A+Q, Q+E cancellation, and camera movement while running.
