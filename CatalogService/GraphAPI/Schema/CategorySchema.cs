using SchemaQL = GraphQL.Types.Schema;

namespace GraphAPI.Schema
{
    public class CategorySchema : SchemaQL
    {
        public CategorySchema(Queries query, Mutations mutation, IServiceProvider provider) : base(provider)
        {
            Query = query;
            Mutation = mutation;
        }
    }
}
