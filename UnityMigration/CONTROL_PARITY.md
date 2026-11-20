# Fate's Passage control parity — original HTML as authority

Source: `fates-passage.html` on the repository's migration branch, inspected directly from Git blob `7f03a3245206d89a5b30fa6fece2997a98a1c00d` on 2026-11-19.

## Confirmed original gameplay controls
- Keyboard `W/A/S/D`: movement, `F`: action/interact, `Space`: jump, `1/2/3`: cast first three skills, `Escape`: close overlay; Space/Enter also advances an open dialogue.
- Browser mouse wheel: camera zoom.
- Browser gameplay uses pointer/touch events for view control; separate in-page map has its own drag/pan/zoom behavior and must **not** be confused with gameplay camera.
- Gameplay mobile touch: left-side virtual movement zone and right-side look zone, with on-screen Jump and Auto Attack actions. Touch drag on right affects yaw and pitch; pinch zoom is implemented in browser gameplay.
- Do not treat temporary prototype Space-to-attack as final control parity.

## Implemented in native prototype (not yet Unity-verified)
- Keyboard WASD/arrows and gamepad move.
- New left-half touch-drag movement, right-half touch-drag camera, desktop left-mouse orbit and wheel zoom.
- F3 diagnostics (prototype-only).

## Still open
1. Preserve original Space jump and F interaction; map 1/2/3 to source-backed skills when ability system is ready.
2. Add accessible mobile touch UI with original placement, visual feedback, jump/attack/interact and skill buttons.
3. Add mobile pinch zoom and test multitouch conflict/cancel behavior.
4. Audit exact gameplay mouse button semantics and camera sensitivity from the original gameplay section (do not substitute map-viewer pointer behavior).
5. Test Mac keyboard/mouse and actual mobile touch builds in Unity, including safe-area and UI-blocking behavior.
6. Move temporary prototype attack off Space when a verified alternative control is available.

These are source-parity targets, not claims of a complete playable port.
