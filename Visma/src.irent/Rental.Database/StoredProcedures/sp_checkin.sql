CREATE PROCEDURE [dbo].[sp_checkin]
	@customer int, @item int, @throw bit, @result int output
AS
set @result = 0;
begin
	begin try
		insert into T_REGISTRATION_QUEUE values(@customer, sysdatetime(), @item);
		set @result = 1;
	end try
	begin catch
		set @result = -12;
		if @throw = 1
			throw;
	end catch
end;
	
RETURN 0
