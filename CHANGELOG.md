# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
