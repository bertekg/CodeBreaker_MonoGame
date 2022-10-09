namespace CodeBreaker_MonoGame
{
    public enum LangOption { PL = 1 };
    public enum LangKey { GameTitle, StartGameKey, ExitGameKey,
        CodeLength, IsLimitedAttempts, NumberAttempts, IsTimeLimitation, TimeLimit,
        PlayingSound, MusicVolume, LanguageSelected, CreditsStart, VersionInfo,
        HelpLeftRight, HelpUpDown, HelpSpace, HelpEsc, HelpSingleDigitOnce,
        HelpColorsOption, HelpColorRed, HelpColorBlue, HelpColorGreen,
        DebugMarkerIndex, DebugMarkerPos, DebugCurrentCode,
        GameRemainingAttempts, GameNumberOfAttempts, GameRemainingTime,
        FinishWin, FinishLost, FinishRemainingTime, FinishPlayingTime,
        FinishRemainingAttempts, FinishNumberOfAttempts, FinishCorrectCode,
        FinishPlayAgain, FinishGoBackMenu};
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
                            returnText = "Wciśnij [Enter] aby rozpocząc grę";
                            break;
                        default:
                            returnText = "Press [Enter] to Start Game";
                            break;
                    }
                    break;
                case LangKey.ExitGameKey:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Naciśnij [Esc] aby wyjść z gry";
                            break;
                        default:
                            returnText = "Press [Esc] to Exit";
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
                            returnText = "[Lewo],[Prawo] - Przesuń kursor w kodzie.";
                            break;
                        default:
                            returnText = "[Left],[Right] - Move the cursor in the code.";
                            break;
                    }
                    break;
                case LangKey.HelpUpDown:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[Góra],[Dół] -  Zmiana wartości wskazanej cyfry.";
                            break;
                        default:
                            returnText = "[Up],[Down] - Change the value of the indicated digit.";
                            break;
                    }
                    break;
                case LangKey.HelpSpace:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[Spacja] - Sprawdź aktualny kod.";
                            break;
                        default:
                            returnText = "[Space] - Check current code.";
                            break;
                    }
                    break;
                case LangKey.HelpEsc:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "[Esc] - Powrót do menu głównego.";
                            break;
                        default:
                            returnText = "[Esc] - Return to main menu.";
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
                            returnText = "Pozostały czas {0:N3} [s]";
                            break;
                        default:
                            returnText = "Remaining time {0:N3} [s]";
                            break;
                    }
                    break;
                case LangKey.FinishPlayingTime:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Czas gry {0:N3} [s]";
                            break;
                        default:
                            returnText = "Playing time {0:N3} [s]";
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
                            returnText = "Naciśnij [Enter] aby zagrać ponownie";
                            break;
                        default:
                            returnText = "Press [Enter] to Start Game again";
                            break;
                    }
                    break;
                case LangKey.FinishGoBackMenu:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Naciśnij [Esc] wrócić do głównego menu";
                            break;
                        default:
                            returnText = "Press [Esc] to go back Main Menu";
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
            string answer = string.Empty;
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
