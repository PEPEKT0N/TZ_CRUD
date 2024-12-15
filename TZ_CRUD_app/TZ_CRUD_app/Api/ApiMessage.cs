using TZ_CRUD_app.Model;

namespace TZ_CRUD_app.Api
{
    // record-ы сообщений API

    // StringMessage - строковое сообщение
    public record StringMessage(string Message);
    // ErrorMessage - сообщение с ошибкой
    public record ErrorMessage(string Type, string Message);
    // SpaceObjectItemMessage - сообщение с данными о космических объектах в списке космических объектов
    public record SpaceObjectListItemMessage(int Id, string Name, ObjectType Type, int DiscoveryYear, string Location);
    // CategoryItemMessage - сообщение с данными о категории космичекских объектов
    public record CategoryItemMessage(int Id, string Name);
}
