namespace BOA.EntityGeneration.CustomSQLExporting.ContextManagement
{
    class NamingMap : EntityGeneration.NamingMap
    {
        #region Public Properties
        public string CamelCasedCustomSqlName    => Resolve(nameof(CamelCasedCustomSqlName));
        public string CamelCasedResultName       => Resolve(nameof(CamelCasedResultName));
        public string EntityNamespace            => Resolve(nameof(EntityNamespace));
        public string EntityProjectDirectory     => Resolve(nameof(EntityProjectDirectory));
        public string ProfileName                => Resolve(nameof(ProfileName));
        public string RepositoryNamespace        => Resolve(nameof(RepositoryNamespace));
        public string RepositoryProjectDirectory => Resolve(nameof(RepositoryProjectDirectory));
        public string SchemaName                 => Resolve(nameof(SchemaName));
        public string SlnDirectoryPath           => Resolve(nameof(SlnDirectoryPath));



        public string InputClassName => Resolve(nameof(InputClassName));
        public string ResultClassName => Resolve(nameof(ResultClassName));
        public string RepositoryClassName => Resolve(nameof(RepositoryClassName));




        #endregion
    }
}