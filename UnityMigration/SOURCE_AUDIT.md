# Source audit — 2026-11-18

Source of truth: `FatesPassage/fates-passage` on `main`. Migration branch: `unity-migration`.

## Verified via GitHub file reads
- `index.html`: browser entry point redirects to `character-creation.html`.
- `manifest.json`: installable PWA, portrait orientation, JPEG icon.
- `sw.js`: offline caching list references character creation, main game, Arkaedon map, character sheet, spell tome, and melding forge.
- `melding-forge.html`: contains an HTML/CSS interface using viewport-responsive layout, Google Fonts, and stage art.
- `character-creation.html`, `fates-passage.html`, `arkaedon-map.html`, `character-sheet.html`, `spell-tome.html`: GitHub file reads of lines 1–35 (or 1–90) returned empty text with nonempty blob SHAs. **This does not prove these files are empty.** Full content remains unverified.

## Risks / unresolved
- Actual gameplay mechanics, Three.js dependencies, source assets, and persistence keys have **not** been recovered.
- No automatic conversion of HTML/JavaScript to Unity C# is assumed.
- CharacterProfile fields are temporary scaffolding, not a validated conversion of the browser schema.
- No Unity compilation, scene execution, or performance profiling has occurred.

## Next
1. Obtain complete source files through GitHub content API or repository archive, without altering main.
2. Map gameplay and asset dependencies before porting mechanics.
3. Build equivalent systems incrementally and validate against the original game.
