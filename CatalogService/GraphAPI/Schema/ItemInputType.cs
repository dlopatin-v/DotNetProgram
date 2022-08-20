using GraphQL.Types;

namespace GraphAPI.Schema
{
    public class ItemInputType : InputObjectGraphType
    {
        public ItemInputType()
        {
            Name = "ItemInput";
            Field<IntGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<StringGraphType>("description");
            Field<StringGraphType>("image");
            Field<IntGraphType>("categoryId");
            Field<DecimalGraphType>("price");
            Field<LongGraphType>("amount");
        }
    }
}
