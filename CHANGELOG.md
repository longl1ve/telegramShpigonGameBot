# Changelog

All notable changes to this project are documented here.

## Unreleased

### Changed

- Replaced global parallel dictionaries with typed, concurrent game sessions.
- Replaced blocking timers and fire-and-forget handlers with cancellable asynchronous workflows.
- Consolidated repeated round, countdown, keyboard, and voting behavior.
- Moved messages, button labels, topics, locations, and roles to validated UTF-8 JSON resources.
- Added runtime English and Ukrainian language selection.
- Reorganized source into application, domain, infrastructure, content, and localization folders.
- Enabled deterministic builds, recommended .NET analyzers, code-style enforcement, and warnings as errors.
- Upgraded the projects from .NET 8 to .NET 10 LTS.
- Normalized the C# root namespace to `TelegramShpigonGameBot`.
- Moved and hardened the systemd deployment template.

### Added

- Added dependency-free regression tests for domain and localization behavior.
- Added repository ignore configuration and a security policy.

## 0.2.0 - 2024-09-08

### Added

- Added round countdowns.

## 0.1.1 - 2024-08-05

### Fixed

- Fixed usernames containing underscores in voting callback data.
- Fixed spy-win calculation.

### Changed

- A tied highest vote now counts as a spy win.
- Restricted voting and game controls to participating players.
- Added handling for Telegram users without usernames.
- Updated localized messages.

## 0.1.0 - 2024-07-31

### Added

- Initial release.
