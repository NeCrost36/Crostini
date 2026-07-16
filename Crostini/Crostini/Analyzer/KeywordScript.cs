//     ______                __  _       _ 
//    / ____/________  _____/ /_(_)___  (_)
//   / /   / ___/ __ \/ ___/ __/ / __ \/ / 
//  / /___/ /  / /_/ (__  ) /_/ / / / / /  
//  \____/_/   \____/____/\__/_/_/ /_/_/   
//        
//Copyright (C) NeCrost36
using System;
using System.Collections.Generic;
using System.Reflection;

//S0rry for I haven't added the analysis logic yet
namespace Crostini
{
    public class KeywordScript
    {
        private KeywordReflection _reflection;

        public KeywordScript()
        {
            _reflection = new KeywordReflection();
        }
        //Reflection all keywords
        public Dictionary<Keyword, Type> GetAllKeywordsWithTypes()
        {
            return _reflection.TypeMap;
        }

        public Type GetKeywordType(Keyword keyword)
        {
            return _reflection.GetTypeFor(keyword);
        }
        public Type GetKeywordType(string keywordName)
        {
            Type enumType = typeof(Keyword);
            FieldInfo field = enumType.GetField(keywordName);

            if (field != null)
            {
                Keyword keyword = (Keyword)field.GetValue(null);
                return _reflection.GetTypeFor(keyword);
            }

            return null;
        }
        public bool IsValidKeyword(string keywordName)
        {
            Type enumType = typeof(Keyword);
            FieldInfo field = enumType.GetField(keywordName);
            return field != null;
        }
        public List<string> GetAllKeywordNames()
        {
            Type enumType = typeof(Keyword);
            return new List<string>(Enum.GetNames(enumType));
        }
        public List<Keyword> GetAllKeywords()
        {
            Type enumType = typeof(Keyword);
            return new List<Keyword>(Enum.GetValues(enumType) as Keyword[]);
        }
    }
}
