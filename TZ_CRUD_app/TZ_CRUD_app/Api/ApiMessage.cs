using TZ_CRUD_app.Model;

namespace TZ_CRUD_app.Api
{
    // record-ы сообщений API

    // StringMessage - строковое сообщение
    public record StringMessage(string Message);
    // ErrorMessage - сообщение с ошибкой
    public record ErrorMessage(string Type, string Message);
    // SpaceObjectMessage - сообщение с полными данными о космическом объекте
    public record SpaceObjectMessage (
        int Id,
        string Name,
        int? DiscoveryYear,
        string Location,
        List<CategoryMessage> Categories
    );    
    // SpaceObjectItemMessage - сообщение с данными о космических объектах в списке космических объектов
    public record SpaceObjectListItemMessage(int Id, string Name, int? DiscoveryYear, string Location);
    // AddSpaceObjectMessage - данные, необходимые для добавления космического объекта в БД
    public record AddSpaceObjectMessage(
        string Name,
        int? discoveryYear,
        string Location
    );

    // CategoryMessage - сообщение с данными о категории космичекских объектов
    public record CategoryMessage(int Id, string Name);
    
}
