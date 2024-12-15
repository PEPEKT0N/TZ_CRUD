﻿namespace TZ_CRUD_app.Model
{
    // SpaceObject - класс, описывающий сущность космического объекта
    public class SpaceObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;        // название  
        public ObjectType Type { get; set; }                    // тип космического объекта
        public int DiscoveryYear { get; set; }                  // год открытия
        public string Location { get; set; } = string.Empty;    // расположение (созвездие / галактика)
    }
}
