namespace OrderService.Messaging.Model
{
    public record CustomerDeletedModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}