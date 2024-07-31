using Telegram.Bot.Types.ReplyMarkups;

namespace telegramShpigonGameBot
{
    // This class stores localization for English language
    internal class BotMessagesEnglish
    {
        // Dictionary for storing text for messages
        public Dictionary<string, string> textMessages = new Dictionary<string, string>
        {
            {"start", "<b>Hello! I'm SpyBot,</b> here to help your fun group of friends play the spy game.\n\n<b>Press to start the game.</b>"},
            {"startAlready", ", the game has already started.\nTo end the current game, enter the command <b>/stop</b>."},
            {"stopNotStarted", ", the game has not started yet."},
            {"stopConfirmation", ", are you sure you want to end the game?"},
            {"stopAccept", ", you have ended the game."},
            {"stopDeny", ", you canceled the game ending. The game continues."},
            {"commands", "<b>Commands:</b>\n/start | /stop | /help | /commands"},
            {"help", "<b>Game Rules</b>\n\nThe game consists of 5 main rounds:\n- introduction round\n- associations round\n- questions round\n- descriptions round\n- actions round\nAfter each round, there will be a guessing round.\n\nThe spy's task is to avoid letting others realize that they are the spy and to figure out which location they are in. The other players task is to reveal the spy so that, during the voting at the end of the game, the spy receives the majority of the votes.\n\n<b>The game takes approximately 20 minutes</b>"},
            {"gameNotStarted", ", you have not started the game. To start, send the command <b>/start</b> or select it from the list of commands."},
            {"startgameAlready", ", the game has already started."},
            {"startgameAdvice", "Everyone who will participate in the game should press the <b>I’m playing!</b> button. Once everyone is ready to start the game, someone should press <b>Ready!</b>\n\n"},
            {"startgameAdvice2", "<b> players have joined the game</b>"},
            {"saveuserAlready", ", you are already participating."},
            {"saveuserNoMore", ", the maximum number of players is 10."},
            {"readyNobody", ", you cannot start the game until no one has joined."},
            {"readyStarted", ", everyone is ready and the game has started."},
            {"readyBlocked", ", you do not have a private chat with the bot or you have blocked it. <b>Start the bot in the private chat, this is necessary to start the game.</b>"},
            {"readyTopic", "<b>Game topic: </b>"},
            {"readyLocation", "<b>Location: </b>"},
            {"readyRoleCivilian", "<b>Your role for this game: </b>"},
            {"readyRoleSpy", "<b>Your role for this game:</b> Spy."},
            {"gamestarted", "<b>The game has started</b>\nEveryone should have received their roles and location information for the current game in a <b>private message from the bot.</b>"},
            {"countdownStarted", "The countdown has begun..."},
            {"roundEnd", "<b>Round completed</b>\nStarting the guessing round..."},
            {"roundFindSpy", "<b>Guessing round</b>\nNow players will express their thoughts on who and why might be the spy.\n\n<b>The round will last 1 minute. When you press, the countdown will begin.</b>"},
            {"roundFindSpyEnd", "<b>Guessing round completed</b>\nThe next round begins..."},
            {"roundFindSpyEndNextVoting", "<b>Guessing round completed</b>\nVoting begins..."},
            {"round1Advice", "<b>Round 1: Introduction</b>\nEach player takes turns introducing themselves with a <b>fictitious alias related to the location and your role.</b> Use non-obvious aliases so the spy doesn't guess the location; but don't use overly complex aliases so that others (not spies) can understand that you are not the spy.\n\n<b>The round will last 2 minutes. When you press, the countdown will begin.</b>"},
            {"round2Advice", "<b>Round 2: Associations</b>\nEach player takes turns naming 3 associations with the location. Remember, the spy is still among you...\n\n<b>The round will last 2 minutes. When you press, the countdown will begin.</b>"},
            {"round3Advice", "<b>Round 3: Questions</b>\nIn this round, players should ask and answer questions in a chain to help reveal the spy. Consequently, the person who asks the first question will receive the last question.\n\n<b>The round will last 3 minutes. When you press, the countdown will begin.</b>"},
            {"round4Advice", "<b>Round 4: Descriptions</b>\nEach player describes details of the location, but in a way that isn't too obvious.\n\n<b>The round will last 3 minutes. When you press, the countdown will begin.</b>"},
            {"round5Advice", "<b>Round 5: Actions</b>\nThis is the final round of the game. Players take turns performing an action that corresponds to their role in this location, but without making it too obvious, as the spy is still not definitively known... The spy should perform an action that matches their guesses about the location.\n\n<b>The round will last 2 minutes. When you press, the countdown will begin.</b>"},
            {"votingAdvice", "<b>Voting</b>\nEveryone should vote now. Choose from the list the person you believe is the spy."},
            {"votingChose", " chosen "},
            {"votingAlreadyChose", ", you have already voted."},
            {"votingAllVoted", "<b>Everyone has cast their vote...</b>\n"},
            {"votingAllVotedAdvice", "\n\nThe spy now has the option to reveal themselves if they choose. In this case, they simply need to name the location they believe is correct. If the spy names the location correctly, they win the game.\n\n<b>Does the spy reveal themselves?</b>"},
            {"votingSpyShowedUpEnd", "<b>Game Over</b>\nIf the spy names the location correctly, they have won...\n\n<b>The game is now over</b>"},
            {"votingSpyNotShowedUpEnd", "<b>Game Over</b>\nThe spy turned out to be "},
            {"votingSpyWon", "\n\n<b>The spy won</b>"},
            {"votingSpyLose", "\n\n<b>The spy lost</b>"},
        };

        // Dictionary for storing the inline keyboard markups
        public Dictionary<string, InlineKeyboardMarkup> inlineKeyboardMarkups = new Dictionary<string, InlineKeyboardMarkup>
        {
            {"keyboardInlineMarkupLobby",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("I'm playing!", "save_user")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Ready!", "ready")
                    }
                })
            },

            {"keyboardInlineMarkupStart",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Start game", "start_game")
                    }
                })
            },

            {"keyboardInlineMarkupStop",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Yes", "stop_accept"),
                        InlineKeyboardButton.WithCallbackData("No", "stop_deny")
                    }
                })
            },

            {"keyboardInlineMarkupStartRoundFindSpy",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Begin", "roundFindSpy_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound1",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Begin", "round1_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound2",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Begin", "round2_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound3",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Begin", "round3_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound4",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Begin", "round4_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupStartRound5",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Begin", "round5_countdown")
                    }
                })
            },

            {"keyboardInlineMarkupVotingSpyShowUp",new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Yes", "spyshowup_accept")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("No", "spyshowup_deny")
                    }
                })
            }
        };
    }
}
