namespace CodeBreaker_MonoGame
{
    public enum LangOption { PL = 1 };
    public enum LangKey { GameTitle, StartGameKey, ExitGameKey,
        CodeLength, isLimitedAttempts, NumberAttempts, TimeLimitation, TimeLimit,
        PlayingSound, VolumeMusic, LanguageSelected };
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
                            returnText = "Lamacz kodu - MonoGame";
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
                            returnText = "Wcisnij [Enter] aby rozpoczac gre";
                            break;
                        default:
                            returnText = "Press [Enter] to Start Game";
                            break;
                    }
                    break;
                case LangKey.ExitGameKey:
                    break;
                case LangKey.CodeLength:
                    break;
                case LangKey.isLimitedAttempts:
                    break;
                case LangKey.NumberAttempts:
                    break;
                case LangKey.TimeLimitation:
                    break;
                case LangKey.TimeLimit:
                    break;
                case LangKey.PlayingSound:
                    break;
                case LangKey.VolumeMusic:
                    break;
                case LangKey.LanguageSelected:
                    switch (langOption)
                    {
                        case LangOption.PL:
                            returnText = "Jezyk: Polski";
                            break;
                        default:
                            returnText = "Language: English";
                            break;
                    }
                    break;
                default:
                    break;
            }
            return returnText;
        }
    }
}
