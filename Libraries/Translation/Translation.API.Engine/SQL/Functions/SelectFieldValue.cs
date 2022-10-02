namespace Translation.API.Engine.SQL.Functions;
public sealed class SelectFieldValue
{
	public const string Sql = $@"
CREATE OR ALTER FUNCTION dbo.fncSelectFieldValue (
@Value NVARCHAR(MAX), 
@Culture VARCHAR(5) = 'en-US', 
@SiteCulture NVARCHAR(5) = NULL,
@FallbackCulture NVARCHAR(5) = 'en-US',
@IgnoreFallbacks BIT = 0)
RETURNS NVARCHAR(MAX)
WITH EXECUTE AS CALLER
AS
BEGIN
    DECLARE @ReturnValue NVARCHAR(MAX);
	DECLARE @JsonValue TABLE (Culture VARCHAR(5), Value NVARCHAR(MAX));

	INSERT INTO @JsonValue (Culture, Value)
	SELECT Culture, Value FROM OPENJSON(@Value) WITH (
   [Culture] VARCHAR(5) '$.Culture',
   [Value] NVARCHAR(MAX) '$.Value'
) j

IF (@Culture IS NULL)
BEGIN

SET @Culture = ISNULL(ISNULL(@SiteCulture, @FallbackCulture), 'en-US');
END


SELECT @ReturnValue = Value FROM @JsonValue Where Culture = @Culture;

IF (@ReturnValue IS NOT NULL OR @IgnoreFallbacks = 1)
BEGIN
 RETURN @ReturnValue;
END

-- If the Fallback = Culture value, and SiteCulture is the same or NULL.
IF ((@FallbackCulture = @Culture AND (@FallbackCulture = @SiteCulture OR @SiteCulture IS NULL)) OR (@SiteCulture IS NULL AND @FallbackCulture IS NULL))
BEGIN
 SELECT TOP 1 @ReturnValue = Value FROM @JsonValue;
 RETURN @ReturnValue;
END

-- If the Culture value was not retrieved and the SiteCulture differs.
IF EXISTS(SELECT TOP 1 1 FROM @JsonValue Where Culture = @SiteCulture)
BEGIN
 SELECT @ReturnValue = Value FROM @JsonValue Where Culture = @SiteCulture;
 RETURN @ReturnValue;
END

IF (@SiteCulture = @FallbackCulture)

BEGIN
 SELECT TOP 1 @ReturnValue = Value FROM @JsonValue;
 RETURN @ReturnValue;
END

IF EXISTS(SELECT TOP 1 1 FROM @JsonValue Where Culture = @FallbackCulture)
BEGIN
 SELECT @ReturnValue = Value FROM @JsonValue Where Culture = @FallbackCulture;
 RETURN @ReturnValue;
END

SELECT TOP 1 @ReturnValue = Value FROM @JsonValue;
RETURN @ReturnValue;

END;
GO

--Example code
--SELECT dbo.fncSelectFieldValue(N'[
--{{""Culture"":""en-US"",""Value"":""0""}},
--{{""Culture"":""de-DE"",""Value"":""another value""}},
--{{""Culture"":""fr-FR"",""Value"":""1""}},
--{{""Culture"":""en-GB""}}
--]',
--'en-GB',
--DEFAULT,
--DEFAULT,
--DEFAULT) [Value];
";
}