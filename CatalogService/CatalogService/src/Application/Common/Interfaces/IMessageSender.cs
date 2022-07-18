namespace CatalogService.Application.Common.Interfaces;
public interface IMessageSender
{
    Task SendAsync<T>(T message);
}
