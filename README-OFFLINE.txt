FATE'S PASSAGE — PWA OFFLINE SUPPORT FILES

This package contains the PWA support files and a patch note for the two existing game HTML files.

Upload the package contents into the GitHub Pages repository root, preserving the icons/ folder:
- index.html
- sw.js
- manifest.webmanifest
- icons/fates-passage-180.png
- icons/fates-passage-512.png

Keep the existing game files:
- CharacterCreation.html
- Fatespassage.html

Apply PWA-INSTALL-PATCH.txt to the <head> of both existing game HTML files.

The service worker attempts to cache both main HTML files, optional local screens, the embedded studio-video-containing character-creation page, Three.js, and the project's Google Fonts. Full offline use must be tested on the target iPhone after the first online visit.
