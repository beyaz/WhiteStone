using System;
using System.Collections.Generic;
using BOA.CodeGeneration.Generators;
using BOA.Common.Helpers;

namespace BOAPlugins.FormApplicationGenerator.Types
{
        public static class Extensions
    {
        #region Public Methods
        

        public static IReadOnlyCollection<BField_Kaldırılacak> GetAllFields(this IReadOnlyCollection<BCard> cards)
        {
            var allFields = new List<BField_Kaldırılacak>();

            foreach (var card in cards)
            {
                allFields.AddRange(card.Fields);
            }

            return allFields;
        }

        public static IReadOnlyCollection<BField_Kaldırılacak> GetAllFields(this IReadOnlyCollection<TabPage> tabs)
        {
            var allFields = new List<BField_Kaldırılacak>();

            foreach (var tab in tabs)
            {
                allFields.AddRange(tab.Cards.GetAllFields());
            }

            return allFields;
        }
        #endregion

        #region Methods
        internal static string GetSnapName(this BField_Kaldırılacak dataBFieldKaldırılacak)
        {
            return $"{dataBFieldKaldırılacak.ComponentType.ToString().RemoveFromStart("B").MakeLowerCaseFirstChar()}{dataBFieldKaldırılacak.Name}";
        }

       

        internal static bool HasSnapName(this BField_Kaldırılacak dataBFieldKaldırılacak)
        {
            return dataBFieldKaldırılacak.ComponentType == ComponentType.BAccountComponent ||
                   dataBFieldKaldırılacak.ComponentType == ComponentType.BParameterComponent;
        }


        internal static string ToCSharp(this DotNetType name)
        {
            if (name == DotNetType.Boolean)
            {
                return "bool?";
            }

            if (name == DotNetType.DateTime)
            {
                return "DateTime?";
            }

            if (name == DotNetType.Decimal)
            {
                return "decimal?";
            }

            if (name == DotNetType.Int32)
            {
                return "int?";
            }

            if (name == DotNetType.String)
            {
                return "string";
            }

            throw new ArgumentException(name.ToString());
        }

        static string MakeLowerCaseFirstChar(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }

            return ContractBodyGenerator.GetPropertyFieldName("", value);
        }
        #endregion
    }

}
