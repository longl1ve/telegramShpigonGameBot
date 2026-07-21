# Telegram Spy Game Bot

A .NET 10 Telegram bot for playing a Spyfall-style party game in group chats. The bot supports English and Ukrainian game content.

## Requirements

- .NET 10 SDK
- A Telegram bot token from BotFather

## Configuration

The bot reads configuration from environment variables:

| Variable | Required | Values | Description |
| --- | --- | --- | --- |
| `SHPIGON_TOKEN` | Yes | Telegram bot token | Authenticates the bot with Telegram. |
| `SHPIGON_LANGUAGE` | No | `en`, `uk` | Selects the game language. Defaults to `en`. |

Never commit a real bot token. Use environment variables or your deployment platform's secret store.

## Run locally

```shell
dotnet restore telegramShpigonGameBot.sln
dotnet run --project Bot/telegramShpigonGameBot.csproj
```

## Build and test

```shell
dotnet build telegramShpigonGameBot.sln --configuration Release --no-restore
dotnet run --project Bot.Tests/telegramShpigonGameBot.Tests.csproj --no-restore
```

The regression runner verifies session behavior, voting rules, round definitions, localized resource structure, role-list isolation, and keyboard construction.

## Game content and localization

Language resources are stored in:

- `Bot/Localization/en.json`
- `Bot/Localization/uk.json`

Each UTF-8 JSON file contains messages, button labels, topics, locations, and roles. When editing or adding a language:

1. Keep message and button keys identical across languages.
2. Keep the same number of topics, locations, and roles.
3. Do not duplicate topic names, location names, or roles within a location.
4. Run the regression tests before committing.

Keyboard layouts and callback identifiers are defined once in `KeyboardFactory.cs`; localization files contain only their translated labels.

## Publish for Linux

```shell
dotnet publish Bot/telegramShpigonGameBot.csproj \
  --configuration Release \
  --runtime linux-x64 \
  --self-contained false \
  --output ./publish
```

The root `bot.service` file is a hardened systemd template. Install the published application under `/opt/telegram-shpigon-game-bot`, create the dedicated `telegram-bot` user/group, copy the service to `/etc/systemd/system/telegram-shpigon-game-bot.service`, and replace the token placeholder before starting it. Protect the installed service file because it contains the bot token.

```shell
sudo chmod 600 /etc/systemd/system/telegram-shpigon-game-bot.service
sudo systemctl daemon-reload
sudo systemctl enable --now telegram-shpigon-game-bot.service
sudo systemctl status telegram-shpigon-game-bot.service
```

## License

This project is licensed under the terms in [LICENSE](LICENSE).
