namespace CodeBreaker_MonoGame
{
    public enum LangOption { PL = 1 };
    public enum LangKey { GameTitle, StartGameKey, ExitGameKey,
        CodeLength, IsLimitedAttempts, NumberAttempts, IsTimeLimitation, TimeLimit,
        PlayingSound, MusicVolume, LanguageSelected, CreditsStart, VersionInfo };
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
