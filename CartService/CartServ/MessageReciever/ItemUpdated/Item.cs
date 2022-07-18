namespace CartServ.MessageReciever.ItemUpdated;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public uint Amount { get; set; }

}
