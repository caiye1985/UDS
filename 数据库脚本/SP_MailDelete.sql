USE [uds]
GO
/****** Object:  StoredProcedure [dbo].[SP_MailDelete]    Script Date: 08/01/2013 01:32:42 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO




-------------------------------------------------------------------------------
-- SP_MailDelete
--MailIDStr 格式示例: MailID=12 or MailID=34 or MailID=56
-------------------------------------------------------------------------------
ALTER PROCEDURE  [dbo].[SP_MailDelete]
(
    @MailIDStr NVarchar(4000),
    @DeleteType int =1
    
)
AS
    DECLARE @Sql NVarchar(4000)
IF @DeleteType=0
BEGIN
   
     SET @Sql =  "UPDATE TabMailList SET MailFolderType=3   WHERE "+ @MailIDStr
    EXEC (@Sql)	
END
ELSE IF @DeleteType=40
BEGIN
	set @Sql = "DELETE FROM TabExtMailList where "+@MailIDStr
	EXEC(@Sql)
END
ELSE
BEGIN
   
    SET @Sql=" DELETE FROM TabMailList  WHERE "+@MailIDStr
    EXEC (@Sql)
END



