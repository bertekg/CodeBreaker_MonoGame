namespace CodeBreaker_MonoGame.Class
{
    public enum LangOption { PL = 1 };
    public enum LangKey
    {
        GameTitle, StartGameKey, ModifiersKey, GameInstuctionKey, ExitGameKey,
        CodeLength, IsLimitedAttempts, NumberAttempts, IsTimeLimitation, TimeLimit,
        PlayingSound, MusicVolume, SoundsVolume, LanguageSelected, CreditsStart, VersionInfo,
        HelpLeftRight, HelpUpDown, HelpSpace, HelpEsc, HelpSingleDigitOnce,
        HelpColorsOption, HelpColorRed, HelpColorBlue, HelpColorGreen,
        DebugMarkerIndex, DebugMarkerPos, DebugCurrentCode,
        GameRemainingAttempts, GameNumberOfAttempts, GameRemainingTime,
        FinishWin, FinishLost, FinishRemainingTime, FinishPlayingTime,
        FinishRemainingAttempts, FinishNumberOfAttempts, FinishCorrectCode,
        FinishPlayAgain, GoBackMenu,
        GameInstuction, ControlsInGame, GameOptionKey, GameOption,
        GameModifiers, DebuggingModeEnabled, Start, Modifiers, Help, Option, Quit, Back, PlayAgain, MainMenu,
        TrueSingleLetter, FalseSingleLetter, DigitRange, HistoryCount
    };
    internal class Lang
    {
        private LangOption langOption;
        public Lang(byte langID)
        {
            langOption = (LangOption)langID;
        }
        public void ChangeLangID(byte langID)
        {
            langOption = (LangOption)langID;
        }
        public void DecSelection()
        {
            sbyte langID = (sbyte)langOption;
            langID--;
            if (langID < 0)
            {
                langID = 1;
            }
            ChangeLangID((byte)langID);
        }
        public void IncSelection()
        {
            byte langID = (byte)langOption;
            langID++;
            if (langID > 1)
            {
                langID = 0;
            }
            ChangeLangID(langID);
        }
        public byte GetLangID()
        {
            return (byte)langOption;
        }
        public string GetLangText(LangKey langKey)
        {
            string returnText = string.Empty;
            switch (langKey)
            {
                case LangKey.GameTitle:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Łamacz kodów - MonoGame";
                            break;
                        default:
                            returnText = "Code Breaker - MonoGame";
                            break;
                    }
                    break;
                case LangKey.StartGameKey:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Wciśnij [Enter lub Start] aby rozpocząc grę";
                            break;
                        default:
                            returnText = "Press [Enter or Start] to Start Game";
                            break;
                    }
                    break;
                case LangKey.ModifiersKey:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Wciśnij [M lub Y] aby ustawić modyfikatory gry";
                            break;
                        default:
                            returnText = "Press [M or Y] to set Game Modifiers";
                            break;
                    }
                    break;
                case LangKey.GameInstuctionKey:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Wciśnij [H lub RightShoulder] aby pokazać instrukcję gry";
                            break;
                        default:
                            returnText = "Press [H or RightShoulder] for Game Instructions";
                            break;
                    }
                    break;
                case LangKey.GameOptionKey:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Wciśnij [O lub LeftShoulder] aby pokazać opcje gry";
                            break;
                        default:
                            returnText = "Press [O or LeftShoulder] for Game Option";
                            break;
                    }
                    break;
                case LangKey.ExitGameKey:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Naciśnij [Escape lub Back] aby wyjść z gry";
                            break;
                        default:
                            returnText = "Press [Escape or Back] to Exit";
                            break;
                    }
                    break;
                case LangKey.CodeLength:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Długość kodu: ";
                            break;
                        default:
                            returnText = "Code length: ";
                            break;
                    }
                    break;
                case LangKey.IsLimitedAttempts:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Ograniczona liczba prób: ";
                            break;
                        default:
                            returnText = "Limited number of attempts: ";
                            break;
                    }
                    break;
                case LangKey.NumberAttempts:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Ilość prób: ";
                            break;
                        default:
                            returnText = "Number of attempts: ";
                            break;
                    }
                    break;
                case LangKey.IsTimeLimitation:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Ograniczenie czasowe: ";
                            break;
                        default:
                            returnText = "Time limitation: ";
                            break;
                    }
                    break;
                case LangKey.TimeLimit:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Limit czasu [s]: ";
                            break;
                        default:
                            returnText = "Time limit [s]: ";
                            break;
                    }
                    break;
                case LangKey.PlayingSound:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Muzyka i efekty dźwiękowe: ";
                            break;
                        default:
                            returnText = "Music and sound effects: ";
                            break;
                    }
                    break;
                case LangKey.MusicVolume:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Głośność muzyki: ";
                            break;
                        default:
                            returnText = "Music volume: ";
                            break;
                    }
                    break;
                case LangKey.SoundsVolume:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Głośność dzwięków: ";
                            break;
                        default:
                            returnText = "Sounds volume: ";
                            break;
                    }
                    break;
                case LangKey.LanguageSelected:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Język: Polski";
                            break;
                        default:
                            returnText = "Language: English";
                            break;
                    }
                    break;
                case LangKey.CreditsStart:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Stworzone przez: ";
                            break;
                        default:
                            returnText = "Developed by: ";
                            break;
                    }
                    break;
                case LangKey.VersionInfo:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Wersja: ";
                            break;
                        default:
                            returnText = "Version: ";
                            break;
                    }
                    break;
                case LangKey.HelpLeftRight:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[Lewo lub A],[Prawo lub D] - Przesuń kursor w kodzie.";
                            break;
                        default:
                            returnText = "[Left or A],[Right or D] - Move the cursor in the code.";
                            break;
                    }
                    break;
                case LangKey.HelpUpDown:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[Góra lub W],[Dół lub S] -  Zmiana wartości wskazanej cyfry.";
                            break;
                        default:
                            returnText = "[Up or W],[Down or S] - Change the value of the indicated digit.";
                            break;
                    }
                    break;
                case LangKey.HelpSpace:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[Spacja lub Przycisk A] - Sprawdź aktualny kod.";
                            break;
                        default:
                            returnText = "[Space or Game Pad A] - Check current code.";
                            break;
                    }
                    break;
                case LangKey.HelpEsc:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[Escape lub Back] - Powrót do menu głównego.";
                            break;
                        default:
                            returnText = "[Escape or Back] - Return to main menu.";
                            break;
                    }
                    break;
                case LangKey.HelpSingleDigitOnce:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Pojedyncza cyfra może wystąpić w kodzie tylko raz!";
                            break;
                        default:
                            returnText = "A single digit can occur only once in the code!";
                            break;
                    }
                    break;
                case LangKey.HelpColorsOption:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Znaczenia kolorów cyfr:";
                            break;
                        default:
                            returnText = "Color meanings of the digits:";
                            break;
                    }
                    break;
                case LangKey.HelpColorRed:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[CZERWONA] - Nie występuje w kodzie.";
                            break;
                        default:
                            returnText = "[RED] - Not present in the code.";
                            break;
                    }
                    break;
                case LangKey.HelpColorBlue:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[NIEBIESKA] - Pojawia się w kodzie w innej pozycji.";
                            break;
                        default:
                            returnText = "[BLUE] - Appears in the code in a different position.";
                            break;
                    }
                    break;
                case LangKey.HelpColorGreen:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[ZIELONA] - Jest na prawidłowej pozycji.";
                            break;
                        default:
                            returnText = "[GREEN] - Is in the correct position.";
                            break;
                    }
                    break;
                case LangKey.DebugMarkerIndex:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Marker, indeks: ";
                            break;
                        default:
                            returnText = "Marker, index: ";
                            break;
                    }
                    break;
                case LangKey.DebugMarkerPos:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = ", pozycja: ";
                            break;
                        default:
                            returnText = ", position: ";
                            break;
                    }
                    break;
                case LangKey.DebugCurrentCode:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Aktualny kod: ";
                            break;
                        default:
                            returnText = "Current code: ";
                            break;
                    }
                    break;
                case LangKey.GameRemainingAttempts:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Pozostałe próby:\n";
                            break;
                        default:
                            returnText = "Remaining attempts:\n";
                            break;
                    }
                    break;
                case LangKey.GameNumberOfAttempts:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Ilość prób:\n";
                            break;
                        default:
                            returnText = "Number of attempts:\n";
                            break;
                    }
                    break;
                case LangKey.GameRemainingTime:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Pozostały czas:\n{0:N1}";
                            break;
                        default:
                            returnText = "Remaining time:\n{0:N1}";
                            break;
                    }
                    break;
                case LangKey.FinishWin:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "!!! WYGRAŁEŚ !!!";
                            break;
                        default:
                            returnText = " !!! You WIN !!!";
                            break;
                    }
                    break;
                case LangKey.FinishLost:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "!!! PRZEGRAŁEŚ !!!";
                            break;
                        default:
                            returnText = " !!! You LOST !!!";
                            break;
                    }
                    break;
                case LangKey.FinishRemainingTime:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Pozostały czas: {0:N3} [s]";
                            break;
                        default:
                            returnText = "Remaining time: {0:N3} [s]";
                            break;
                    }
                    break;
                case LangKey.FinishPlayingTime:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Czas gry: {0:N3} [s]";
                            break;
                        default:
                            returnText = "Playing time: {0:N3} [s]";
                            break;
                    }
                    break;
                case LangKey.FinishRemainingAttempts:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Pozostało prób: ";
                            break;
                        default:
                            returnText = "Remaining attempts: ";
                            break;
                    }
                    break;
                case LangKey.FinishNumberOfAttempts:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Liczba prób: ";
                            break;
                        default:
                            returnText = "Number of attempts: ";
                            break;
                    }
                    break;
                case LangKey.FinishCorrectCode:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Poprawny kod: ";
                            break;
                        default:
                            returnText = "Correct code: ";
                            break;
                    }
                    break;
                case LangKey.FinishPlayAgain:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Naciśnij [Enter lub Start] aby zagrać ponownie";
                            break;
                        default:
                            returnText = "Press [Enter or Start] to Start Game again";
                            break;
                    }
                    break;
                case LangKey.GoBackMenu:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Naciśnij [Escape lub Back] aby wrócić do głównego menu";
                            break;
                        default:
                            returnText = "Press [Escape or Back] to go back Main Menu";
                            break;
                    }
                    break;
                case LangKey.GameInstuction:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Instrukcja gry";
                            break;
                        default:
                            returnText = "Game instruction";
                            break;
                    }
                    break;
                case LangKey.ControlsInGame:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Sterowanie podczas gry:";
                            break;
                        default:
                            returnText = "Controls during the game:";
                            break;
                    }
                    break;
                case LangKey.GameOption:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Opcje gry";
                            break;
                        default:
                            returnText = "Game Option";
                            break;
                    }
                    break;
                case LangKey.GameModifiers:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Modyfikatory gry";
                            break;
                        default:
                            returnText = "Game Modifiers";
                            break;
                    }
                    break;
                case LangKey.DebuggingModeEnabled:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Tryb debugowania włączony".ToUpper();
                            break;
                        default:
                            returnText = "Debugging mode enabled".ToUpper();
                            break;
                    }
                    break;
                case LangKey.Start:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "START";
                            break;
                        default:
                            returnText = "START";
                            break;
                    }
                    break;
                case LangKey.Modifiers:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "MODYFIKATORY";
                            break;
                        default:
                            returnText = "MODIFIERS";
                            break;
                    }
                    break;
                case LangKey.Help:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "POMOC";
                            break;
                        default:
                            returnText = "HELP";
                            break;
                    }
                    break;
                case LangKey.Option:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "OPCJE";
                            break;
                        default:
                            returnText = "OPTIONS";
                            break;
                    }
                    break;
                case LangKey.Quit:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "WYJŚCIE";
                            break;
                        default:
                            returnText = "EXIT";
                            break;
                    }
                    break;
                case LangKey.Back:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "WRÓĆ";
                            break;
                        default:
                            returnText = "BACK";
                            break;
                    }
                    break;
                case LangKey.PlayAgain:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "GRAJ PONOWNIE";
                            break;
                        default:
                            returnText = "PLAY AGAIN";
                            break;
                    }
                    break;
                case LangKey.MainMenu:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "MENU GŁÓWNE";
                            break;
                        default:
                            returnText = "MAIN MENU";
                            break;
                    }
                    break;
                case LangKey.TrueSingleLetter:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "P";
                            break;
                        default:
                            returnText = "T";
                            break;
                    }
                    break;
                case LangKey.FalseSingleLetter:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "F";
                            break;
                        default:
                            returnText = "F";
                            break;
                    }
                    break;
                case LangKey.DigitRange:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Zakres cyfr";
                            break;
                        default:
                            returnText = "Digit Range";
                            break;
                    }
                    break;
                case LangKey.HistoryCount:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Ilość odgadniętych kodów: ";
                            break;
                        default:
                            returnText = "Number of codes guessed: ";
                            break;
                    }
                    break;
                default:
                    break;
            }
            return returnText;
        }
        public string GetBoolInLang(bool value)
        {
            string answer;
            if (value)
            {
                switch (langOption)
                {
                    case LangOption.PL:
                        answer = "Tak";
                        break;
                    default:
                        answer = "Yes";
                        break;
                }
            }
            else
            {
                switch (langOption)
                {
                    case LangOption.PL:
                        answer = "Nie";
                        break;
                    default:
                        answer = "No";
                        break;
                }
            }
            return answer;
        }
    }
}
