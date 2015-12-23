namespace Infrastructure.Repository.SqlGenerator
{
    public interface IEntityMetadata<TEntity>  where TEntity : new()
    {
        System.Collections.Generic.IEnumerable<PropertyMetadata> BaseProperties { get; set; }
        PropertyMetadata IdentityProperty { get; set; }
        bool IsIdentity { get; }
        System.Collections.Generic.IEnumerable<PropertyMetadata> KeyProperties { get; set; }
        bool LogicalDelete { get; }
        object LogicalDeleteValue { get; set; }
        PropertyMetadata StatusProperty { get; set; }
        string TableName { get; set; }
    }
}
