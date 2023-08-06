# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.3.0] - 2023-08-06

### Changed

-   Input Visualizer now works with the latest version of `InputUtilities` and also supports Nintendo Switch styles.

## [1.2.5] - 2023-07-13

### Fixed

-   Fixed a bug caused by changing the `EventSystem` in a scene.

## [1.2.4] - 2023-05-15

### Added

-   Added an option to `ButtonPlus` to play highlight effects on selection.

## [1.2.3] - 2023-04-14

### Changed

-   `FPSDisplay` can now display the fps low. It also supports selecting which values to display.

## [1.2.2] - 2023-03-11

### Changed

-   `FPSDisplay` now uses displays the average FPS for the current scene separately.

## [1.2.1] - 2022-08-14

### Changed

-   `DropdownAutoScroller` now uses `ScrollViewAutoScroller` for the autoscroll.
-   `ScrollViewAutoScroller` now works better with different pivots, anchors etc.
-   Improved the default prefabs in the GameObject/DartCore menu & added separators between them.

### Fixed

-   `DropdownAutoScroller` was ignoring the `onlyWorkOnActivation` property.

## [1.2.0] - 2022-08-14

### Added

-   `ScrollViewAutoScroller` now has a propery called `useDepthBasedSearchForSelectables`, when enabled it will consider the `Selectable` children of the elements in the `ScrollView` if the element has no `Selectable` component.
-   Added an option to disable the loopable navigation to `ScrollViewAutoScroller`.

### Fixed

-   `ScrollViewAutoScroller` was ignoring the `useUnscaledTime` property.

## [1.1.3] - 2022-05-11

### Changed

-   Added an option to stop `VersionDisplayer`'s `DontDestroyOnLoad`.
-   Added on option to select a key to toggle the `VersionDisplayer`.

## [1.1.2] - 2022-02-14

### Changed

-   `TogglePlus`'s `onToggle` unity event is now public.
