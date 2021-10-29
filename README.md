UPDATE 2: My azure subscription has been resumed so now the hosted version can be used.

UPDATE: Azure has disabled my subscription without notice and you won't be able to access the host.
I sent a support request and i am waiting for their reply.

However i pushed a new version using RAW connection string to DB so you will be able to test on localhost.

# VacationHireInc using API.

API doc:

1. /Vehicle/GetVehicles - to get a list of vehicles

Ex: https://vacationhireinc.azurewebsites.net/Vehicle/GetVehicles

2. /Order/NewOrder - to create a new order
Parameters: 1) VehicleID - GUID - required (vehicle you wish to hire)
            2) StartDate - DateTime - required (when will the booking start)
            3) EndDate - DateTime - required (when will the booking end)
            4) CustomerName - string - required
            5) CustomerPhoneNumber - string - required
            
Ex: https://vacationhireinc.azurewebsites.net/Order/NewOrder?VehicleID=385175d5-48f1-4653-8513-c4c6acaecd76&StartDate=2021-11-28 13:00:00&EndDate=2021-11-30 12:00:00&CustomerName=Marian&CustomerPhoneNumber=07914618716

3. /Order/CancelOrder - to cancel a booked order 
Parameters: 1) VehicleID - GUID - required (will be used to identify the booking/order and cancel it)
            2) StartDate - DateTime - required (will be used to identify the booking/order and cancel it)
            3) EndDate - DateTime - required (will be used to identify the booking/order and cancel it)

Ex: https://vacationhireinc.azurewebsites.net/Order/CancelOrder?VehicleID=385175d5-48f1-4653-8513-c4c6acaecd76&StartDate=2021-11-28 13:00:00&EndDate=2021-11-30 12:00:00

4. /Order/FinishOrder - To finish an order/booking
Parameters: 1) VehicleID - GUID - required
            2) StartDate - DateTime - required
            3) EndDate - DateTime - required
            4) Damage - string
            5)GasolineFilled - bool (false if left empty)

Ex: https://vacationhireinc.azurewebsites.net/Order/FinishOrder?VehicleID=A11C2FBB-682C-47AA-A88D-5624BC635F25&StartDate=2021-10-25 12:00:00&EndDate=2021-10-27 12:00:00&Damage=Aripa dreapta zgariata&GasolineFilled=true

In order to test FinishOrder, the order must be Processing, the status would be updated to Processing by the function if the booking has started so i manually updated an order for you to test:
Please replace the above parameters with:
VehicleID:  A11C2FBB-682C-47AA-A88D-5624BC635F25
StartDate: 2021-10-28 13:00:00.000
EndDate: 2021-10-30 12:00:00.000

5. /Order/GetOrders - to get a list of orders/bookings

Ex: https://vacationhireinc.azurewebsites.net/Order/GetOrders


Function "UpdateOrdersStatus"
Would run every minute looking for orders that just started and set their status as Processing and also looking for orders that just finished and set their status to Finished.
-would then create an endpoint to only add damage or state out that the tank is not filled up.

Enums:
    public enum OrderStatus
    {
        Booked = 1,
        Processing = 2,
        Finished = 3,
        Cancelled = 4
    }
    
    public enum VehicleType
    {
        Truck = 1,
        Minivan = 2,
        Sedan = 3
    }
    
    Given the way the database and objects were created, it would be easy to extend to boats, hotels, etc. as the VehicleID in the Order table is not required and there could be a RoomID too. 

I did not make use of currencylayer.com API as i did not need it but i would have created a JSON object matching the currencylayer API response and deserialize it.
