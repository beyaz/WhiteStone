﻿using System.Collections.Generic;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    public static class Extensions
    {
        #region Public Methods
        public static string GetNameofEntityNamespace(this IDataContext context)
        {
            var config    = context.Get(Data.Config);
            var profileId = context.Get(CustomSqlExporter.ProfileId);

            return config.CustomSQLEntityNamespaceFormat.Replace("{ProfileId}", profileId);
        }

        public static string GetNameofRepositoryNamespace(this IDataContext context)
        {
            var config    = context.Get(Data.Config);
            var profileId = context.Get(CustomSqlExporter.ProfileId);

            return config.CustomSQLRepositoryNamespaceFormat.Replace("{ProfileId}", profileId);
        }

        public static IReadOnlyList<string> GetProfileNames(this IDataContext context)
        {
            var profileIdList = new List<string>();
            var database      = context.Get(Data.Database);
            var config        = context.Get(Data.Config);

            database.CommandText = config.SQL_GetProfileIdList;
            var reader = database.ExecuteReader();
            while (reader.Read())
            {
                profileIdList.Add(reader["ProfileId"].ToString());
            }

            reader.Close();

            profileIdList.Add("*");

            return profileIdList;
        }
        #endregion
    }
}