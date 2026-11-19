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

## Source-backed character sheet extraction — 2026-11-19

The exact `character-sheet.html` Git blob was recovered and inspected. Its initial `D` character record identifies Corin Thistledown as Level 1, gender "Other", Woodsman sign, Verdantshade, Shadarill, Naturalist specialization; 105/105 HP, 95/95 MP, 95/95 Energy; Strength 10, Agility 15, Intellect 15, Toughness 10; 5 Resonance and 0 unspent points. It lists ten resistances at 10 with zero bonus, three active abilities (Root Snare, Nature’s Embrace, Forest Companion), and starter equipment including Verdant Tunic, Verdant Trousers and Gnarled Oak Staff.

The Unity branch now contains a typed `CharacterSheetData` schema, a `CharacterSheetFactory.CreateCorinStarter()` source-backed fixture, validation, tests, and a versioned save service. These are a faithful extraction of the initial sheet fields listed above, not yet a complete conversion of every character-creation choice or live game-state rule.

The game runtime's `fatesPassage.save.v1` snapshot separately persists world position, orientation, inside/outside state, attributes/resources objects, companion state, quest/boss flags, skill cooldowns, hotbar state, dummy health, target and UI progression fragments. `fatesPassage.sheet.v1` transfers HP/MP/Energy pairs, resonance, level-up flag and selected skill charges to the sheet page. Those save structures need a dedicated compatibility importer before save migration can be considered complete.

## Browser sheet handoff importer

The source handoff is explicitly `{hp:[current,max], mp:[current,max], en:[current,max], res:number, lv2:0|1, ch:[...]}` under `fatesPassage.sheet.v1`. The native `BrowserSheetHandoffImporter` now imports and validates the resource pairs and resonance. For `lv2:1` it applies the source-observed level 2 state: level becomes 2, XP resets to 0, Toughness gains 3, Intellect gains 2, and Moonhowl Helm is added. The `ch` skill-charge array is intentionally not applied yet because the browser uses nullable entries and a safe null-preserving parser is still required.
