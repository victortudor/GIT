CREATE PROCEDURE [dbo].[sp_get_available_by_customer]
	@customer int
AS
	-- extend here
	select i.ITEM_ID as item, i.ITEM_NAME as itemName, t.TYPE_ID as type, t.TYPE_NAME as typeName
	from T_RENTAL_ITEM i join T_RENTAL_ITEM_TYPE t on i.ITEM_TYPE_ID = t.TYPE_ID
	where i.ITEM_ID not in(select r.ITEM_ID from T_REGISTRATION_QUEUE r where r.CUSTOMER_ID = @customer);
RETURN 0
