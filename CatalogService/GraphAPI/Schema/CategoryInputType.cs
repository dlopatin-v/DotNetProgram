using GraphQL.Types;

namespace GraphAPI.Schema
{
    public class CategoryInputType : InputObjectGraphType
    {
        public CategoryInputType()
        {
            Name = "CategoryInput";

            Field<IntGraphType>("id");

            Field<NonNullGraphType<StringGraphType>>("name");

            Field<StringGraphType>("image");
        }
    }
}
