CREATE PROCEDURE [dbo].[sp_checkout] 
	@customer int, @item int, @throw bit, @result int output, @created datetime2 output
AS
set @result = 0;
set @created = null;
begin
	-- extend here
	begin try
		select @created = CREATED from T_REGISTRATION_QUEUE where ITEM_ID = @item and CUSTOMER_ID = @customer;

		if @created is null 
		  RAISERROR (-12,-1,-1, 'item not found'); -- extend here
		else
			delete from T_REGISTRATION_QUEUE where ITEM_ID = @item and CUSTOMER_ID = @customer;

		set @result = 1;
	end try
	begin catch
		set @result = -12; -- extend here
		if @throw = 1
			throw;
	end catch
end;
	
RETURN 0