-- �������� ��������� ��� ��������� ������ � ������� SpaceObject
ALTER PROCEDURE GenerateSpaceObjects
    @count INT = 1000  -- ���������� ��������, ������� ����� �������
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @i INT = 1;
    DECLARE @randomYear INT;
    DECLARE @randomCategoryId INT;
    DECLARE @zodiacSign NVARCHAR(50);
    
    -- ������ ������������ ���������
    DECLARE @Zodiac TABLE (Name NVARCHAR(50));
    INSERT INTO @Zodiac (Name) VALUES 
        ('����'), ('�����'), ('��������'), ('���'), ('���'), ('����'), 
        ('����'), ('��������'), ('�������'), ('�������'), ('�������'), ('����');
    
    WHILE @i <= @count
    BEGIN
        -- ��������� ��������� ������
        SET @randomYear = FLOOR(RAND() * (2024 - 1650 + 1)) + 1650; -- ��������� ��� � ��������� �� 1650 �� 2024
        
        -- �������� ��������� ������������ ��������� �� �������
        SELECT TOP 1 @zodiacSign = Name FROM @Zodiac ORDER BY NEWID();
        
        -- ��������� SpaceObject
        INSERT INTO SpaceObjects (Name, DiscoveryYear, Location)
        VALUES (CONCAT('������ ', @i), @randomYear, @zodiacSign);
        
        -- � 90% ������������ ��������� ��������� ��� �������
        IF RAND() <= 0.9
        BEGIN
            -- �������� ��������� ���������� ��������� ��� ������� (1-3)
            DECLARE @numCategories INT = FLOOR(RAND() * 3) + 1;
            
            -- ��������� ��������� ��������� ��� �������� �������
            WHILE @numCategories > 0
            BEGIN
                -- �������� ��������� ��������� �� ������� Category
                SELECT TOP 1 @randomCategoryId = Id FROM Categories ORDER BY NEWID();
                
                -- ���������, �� ��������� �� ��� ��� ��������� ��� �������� SpaceObject
                IF NOT EXISTS (
                    SELECT 1 FROM CategorySpaceObject WHERE SpaceObjectsId = @i AND CategoriesId = @randomCategoryId
                )
                BEGIN
                    -- ����� ������� � ���������� (����� ��������� �������, ���� ������������ many-to-many)
                    INSERT INTO CategorySpaceObject (SpaceObjectsId, CategoriesId)
                    VALUES (@i, @randomCategoryId);
                END
                
                SET @numCategories = @numCategories - 1;
            END
        END
        
        SET @i = @i + 1;
    END
END;