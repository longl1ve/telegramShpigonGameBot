using Telegram.Bot.Types.ReplyMarkups;

namespace telegramShpigonGameBot
{
    // This class stores localization for Ukrainian language
    internal class BotMessagesUkrainian
    {
        // Dictionary for storing text for messages
        public Dictionary<string, string> textMessages = new Dictionary<string, string>
        {
            {"checkUsername", ", ви не маєте імені користувача. Додайте його у своєму профілі Telegram (він має починатися з @, наприклад @username), щоб користуватись ботом."},
            {"start", "<b>Привіт! Я - бот Шпигон,</b> який допоможе вашій веселій компанії друзів провести гру в шпигуна.\n\n<b>Натисніть, щоб почати гру.</b>"},
            {"startAlready", ", гру вже розпочато.\nЩоб завершити поточну гру введіть команду <b>/stop</b>."},
            {"stopNotStarted", ", гру ще не розпочато."},
            {"stopConfirmation", ", ви впевнені, що хочете завершити гру?"},
            {"stopAccept", ", ви завершили гру."},
            {"stopDeny", ", ви скасували завершення гри. Гра продовжується."},
            {"commands", "<b>Команди:</b>\n/start | /stop | /help | /commands"},
            {"help", "<b>Правила гри</b>\n\nГра складається з 5 основних раундів:\n- раунд псевдонімів\n- раунд асоціацій\n- раунд запитань\n- раунд описів\n- раунд дій\nПілся кожного раунду буде коло припущень.\n\nЗадача шпигуна - не дати іншим зрозуміти, що він шпигун, і встигнути зрозуміти на якій локації він знаходиться. Задача інших гравців - викрити шпигуна, щоб під час голосування в кінці гри більшість голосів отримав саме шпигун.\n\n<b>Гра займає приблизно 20 хвилин</b>"},
            {"gameNotStarted", ", ви не розпочали гру. Для початку надішліть команду <b>/start</b> або оберіть її зі списку команд."},
            {"startgameAlready", ", гру вже розпочато."},
            {"startgameAdvice", "Хай всі, хто буде брати участь у грі, натиснуть на кнопку <b>Я граю!</b> Коли всі будуть готові почати гру, хтось має натиснути <b>Готові!</b>\n\n"},
            {"startgameAdvice2", "<b> гравців приєдналися до гри</b>"},
            {"saveuserAlready", ", ви вже приймаєте участь."},
            {"saveuserNoMore", ", максимальна кількість гравців - 10."},
            {"readyNobody", ", ви не можете почати гру, поки ніхто не приєднався."},
            {"readyStarted", ", всі вже готові і гру розпочато."},
            {"readyBlocked", ", ви не маєте приватного чату з ботом або заблокували його. <b>Запустіть бота в приватному чаті, це необхідно для початку гри.</b>"},
            {"readyTopic", "<b>Тема гри: </b>"},
            {"readyLocation", "<b>Локація: </b>"},
            {"readyRoleCivilian", "<b>Ваша роль у цій грі: </b>"},
            {"readyRoleSpy", "<b>Ваша роль у цій грі:</b> Шпигун."},
            {"gamestarted", "<b>Гру розпочато</b>\nВсі мали отримати ролі і інформацію про локацію на поточну гру в <b>приватному повідомленні від бота.</b>"},
            {"countdownStarted", "Відлік почався..."},
            {"roundEnd", "<b>Раунд завершено</b>\nПочинається коло припущень..."},
            {"roundFindSpy", "<b>Коло припущень</b>\nЗараз гравці будуть висловлювати свої думки щодо того, хто і чому може бути шпигуном.\n\n<b>Коло триватиме 1 хвилину. Коли ви натиснете, почнеться зворотній відлік.</b>"},
            {"roundFindSpyEnd", "<b>Коло припущень завершено</b>\nПочинається наступний раунд..."},
            {"roundFindSpyEndNextVoting", "<b>Коло припущень завершено</b>\nПочинається голосування..."},
            {"round1Advice", "<b>Раунд 1: Псевдоніми</b>\nКожен з гравців по черзі має відрекомендуватися <b>вигаданим псевдонімом, який мав би відноситися до локації і вашої ролі.</b> Використовуйте неочевидні псевдоніми, щоб шпигун не здогадався, що це за локація; але не використовуйте надто складні псевдоніми, щоб інші (не шпигуни) мали змогу зрозуміти, що ви не є шпигуном.\n\n<b>Раунд триватиме 2 хвилини. Коли ви натиснете, почнеться зворотній відлік.</b>"},
            {"round2Advice", "<b>Раунд 2: Асоціації</b>\nКожен з гравців по черзі має назвати по 3 асоціації з локацією. Не забувайте, що шпигун все ще серед вас...\n\n<b>Раунд триватиме 2 хвилини. Коли ви натиснете, почнеться зворотній відлік.</b>"},
            {"round3Advice", "<b>Раунд 3: Запитання</b>\nВ цьому раунді гравці мають ланцюжком ставити запитання, які мали б допомогти викрити шпигуна, і давати на них відповіді. Відповідно, хто першим ставив запитання, той останнім отримає запитання.\n\n<b>Раунд триватиме 3 хвилини. Коли ви натиснете, почнеться зворотній відлік.</b>"},
            {"round4Advice", "<b>Раунд 4: Описи</b>\nКожен гравець описує деталі локації, але так, щоб не бути надто очевидним.\n\n<b>Раунд триватиме 3 хвилини. Коли ви натиснете, почнеться зворотній відлік.</b>"},
            {"round5Advice", "<b>Раунд 5: Дії</b>\nЦе останній раунд гри. Гравці по черзі мають зобразити дію, яка відповідає їх ролі на цій локації, але не дуже очевидну, бо шпигун все ще достеменно не відомий... Шпигун має зобразити ту дію, яка відповідає його здогадкам, щодо локації.\n\n<b>Раунд триватиме 2 хвилини. Коли ви натиснете, почнеться зворотній відлік.</b>"},
            {"votingAdvice", "<b>Голосування</b>\nЗараз всі мають проголосувати. Оберіть зі списку того, кого ви вважаєте шпигуном."},
            {"votingChose", " обрав(-ла) "},
            {"votingAlreadyChose", ", ви вже проголосували."},
            {"votingAllVoted", "<b>Всі віддали свій голос...</b>\n"},
            {"votingAllVotedAdvice", "\n\nЗараз шпигун має змогу розкритися, якщо воліє цього. В такому разі він має просто назвати локацію, яку він вважає правильною. Якщо шпигун називає локацію правильно, він переміг у цій грі.\n\n<b>Чи розкривається шпигун?</b>"},
            {"votingSpyShowedUpEnd", "<b>Кінець гри</b>\nЯкщо шпигун назве локацію правильно, він виграв...\n\n<b>Наразі гру завершено</b>"},
            {"votingSpyNotShowedUpEnd", "<b>Кінець гри</b>\nШпигуном виявився "},
            {"votingSpyWon", "\n\n<b>Шпигун переміг</b>"},
            {"votingSpyLose", "\n\n<b>Шпигун програв</b>"},
            {"notParticipating", ", ви не берете участь у грі."}
        };

        // Dictionary for storing the inline keyboard markups
        public Dictionary<string, InlineKeyboardMarkup> inlineKeyboardMarkups = new Dictionary<string, InlineKeyboardMarkup>
        {
            {"keyboardInlineMarkupLobby",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Я граю!", "save_user")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Готові!", "ready")
                    }
                })
            },

            {"keyboardInlineMarkupStart",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Почати гру", "start_game")
                    }
                })
            },

            {"keyboardInlineMarkupStop",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Так", "stop_accept"),
                        InlineKeyboardButton.WithCallbackData("Ні", "stop_deny")
                    }
                })
            }, 

            // All below need to be added to English dictionary
            {"keyboardInlineMarkupStartRoundFindSpy",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Почати", "roundFindSpy_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound1",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Почати", "round1_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound2",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Почати", "round2_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound3",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Почати", "round3_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound4",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Почати", "round4_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound5",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Почати", "round5_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupVotingSpyShowUp",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Так", "spyshowup_accept")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Ні", "spyshowup_deny")
                    }
                })
            }
        };
    }
}
