# Currently in early stage development.

### This is the .NET Core Telegram bot for playing a SpyFall with friends inside of chats. Bot language can be English or Ukrainian.

You can play with stable version of my project with these two bots:
**@ShpigonGameEngBot (English)** and **@ShpigonGameBot (Ukrainian)** 

### Deploy instruction for Ubuntu:
1. Install .NET Core SDK on your machine. You can find instruction for this on Microsoft website. 
2. Choose or create directory on your machine where you will store this project and then `cd` to it.
3. Make a local clone of this repository on your machine with `git clone https://github.com/longl1ve/telegramShpigonGameBot`
4. Now `cd` to your local repository.
5. Publish a .NET app with `dotnet publish -c Release -o /path/to/your/local/repository --runtime linux-x64`
6. Create a Systemd File for .NET app with `nano /etc/systemd/system/bot.service` (bot will be deployed as a service).
7. Copy config from [bot.service](https://github.com/longl1ve/telegramShpigonGameBot/blob/main/bot.service) to the `bot.service` file on your machine and edit things such as bot API token and path. Save the changes and close the file.
8. Now reload daemon with `sudo systemctl daemon-reload`
9. Start bot service with `sudo systemctl start bot.service`
10. Enable bot service with `sudo systemctl enable bot.service`
11. Now you can check the status of your bot with `sudo systemctl status bot.service`
12. It should be all set up and running! Now your service will start on system boot every time.