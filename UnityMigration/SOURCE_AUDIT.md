# Fate's Passage source audit — 2026-11-18

Source: `FatesPassage/fates-passage` (`main`). Development: `unity-migration`.

## Confirmed source recovery
The GitHub file endpoint returned empty text for several very large HTML files. Fetching their exact Git blob SHA instead successfully recovered the full text:

| File | Approx. source size | Observations |
| --- | ---: | --- |
| character-creation.html | 16.2 MB | Browser localStorage, embedded images and audio, canvas, JavaScript |
| fates-passage.html | 16.5 MB | Three.js/WebGL references, browser localStorage, embedded images |
| arkaedon-map.html | 3.7 MB | Three.js references, embedded media |
| character-sheet.html | 5.7 MB | Three.js references, many embedded images |
| spell-tome.html | 2.7 MB | Three.js references, embedded images |
| melding-forge.html | smaller | Responsive HTML/CSS stage and external Google Fonts |

`index.html` redirects to character creation. `sw.js` defines the offline cache list. `manifest.json` defines the browser PWA.

## Engineering implications
- This is not a simple HTML wrapper migration: Three.js scene construction, browser storage, DOM UI and embedded media require native Unity replacements.
- Embedded base64 art should be extracted, hashed and catalogued before import; avoid embedding multi-megabyte data URIs in C#.
- Browser save keys and schemas must be enumerated before implementing compatibility or import.
- Current C# character fields, progression rules and placeholder visuals are **provisional scaffolding**, not verified exact parity.
- Unity C# source and NUnit tests have been committed but have **not** been compiled, run or visually reviewed in the Unity Editor.

## Work remaining
1. Extract gameplay code and asset inventory from recovered blobs.
2. Identify and document actual character classes, skills, world and combat mechanics.
3. Replace provisional data model with verified schema.
4. Build usable scenes and UI, import assets, then compile/test in Unity.
5. Compare Unity behavior and visuals to the browser game.
