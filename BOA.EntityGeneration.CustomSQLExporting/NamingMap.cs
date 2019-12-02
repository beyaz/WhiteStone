namespace BOA.EntityGeneration.CustomSQLExporting
{
    class NamingMap:BOA.EntityGeneration.NamingMap
    {
        public string SchemaName              => Resolve(nameof(SchemaName));
        public string ProfileName => Resolve(nameof(ProfileName));
        public string EntityNamespace         => Resolve(nameof(EntityNamespace));
        public string RepositoryNamespace => Resolve(nameof(RepositoryNamespace));
        public string CamelCasedCustomSqlName => Resolve(nameof(CamelCasedCustomSqlName));
        public string CamelCasedResultName    => Resolve(nameof(CamelCasedResultName));

        public string SlnDirectoryPath => Resolve(nameof(SlnDirectoryPath));
    }
}