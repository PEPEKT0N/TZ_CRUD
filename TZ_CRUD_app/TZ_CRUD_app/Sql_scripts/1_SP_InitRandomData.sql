-- Хранимая процедура для генерации данных в таблице SpaceObject
ALTER PROCEDURE GenerateSpaceObjects
    @count INT = 1000  -- Количество объектов, которые нужно создать
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @i INT = 1;
    DECLARE @randomYear INT;
    DECLARE @randomCategoryId INT;
    DECLARE @zodiacSign NVARCHAR(50);
    
    -- Массив зодиакальных созвездий
    DECLARE @Zodiac TABLE (Name NVARCHAR(50));
    INSERT INTO @Zodiac (Name) VALUES 
        ('Овен'), ('Телец'), ('Близнецы'), ('Рак'), ('Лев'), ('Дева'), 
        ('Весы'), ('Скорпион'), ('Стрелец'), ('Козерог'), ('Водолей'), ('Рыбы');
    
    WHILE @i <= @count
    BEGIN
        -- Генерация случайных данных
        SET @randomYear = FLOOR(RAND() * (2024 - 1650 + 1)) + 1650; -- Случайный год в диапазоне от 1650 до 2024
        
        -- Выбираем случайное зодиакальное созвездие из таблицы
        SELECT TOP 1 @zodiacSign = Name FROM @Zodiac ORDER BY NEWID();
        
        -- Вставляем SpaceObject
        INSERT INTO SpaceObjects (Name, DiscoveryYear, Location)
        VALUES (CONCAT('Объект ', @i), @randomYear, @zodiacSign);
        
        -- С 90% вероятностью добавляем категории для объекта
        IF RAND() <= 0.9
        BEGIN
            -- Получаем случайное количество категорий для объекта (1-3)
            DECLARE @numCategories INT = FLOOR(RAND() * 3) + 1;
            
            -- Вставляем случайные категории для текущего объекта
            WHILE @numCategories > 0
            BEGIN
                -- Выбираем случайную категорию из таблицы Category
                SELECT TOP 1 @randomCategoryId = Id FROM Categories ORDER BY NEWID();
                
                -- Проверяем, не назначена ли уже эта категория для текущего SpaceObject
                IF NOT EXISTS (
                    SELECT 1 FROM CategorySpaceObject WHERE SpaceObjectsId = @i AND CategoriesId = @randomCategoryId
                )
                BEGIN
                    -- Связь объекта с категорией (через связующую таблицу, если используется many-to-many)
                    INSERT INTO CategorySpaceObject (SpaceObjectsId, CategoriesId)
                    VALUES (@i, @randomCategoryId);
                END
                
                SET @numCategories = @numCategories - 1;
            END
        END
        
        SET @i = @i + 1;
    END
END;