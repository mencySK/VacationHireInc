# VacationHireInc using API.

API doc:

1. /Vehicle/GetVehicles - to get a list of vehicles

Ex: https://localhost:xxxxx/Vehicle/GetVehicles

2. /Order/NewOrder - to create a new order
Parameters: 1) VehicleID - GUID - required (vehicle you wish to hire)
            2) StartDate - DateTime - required (when will the booking start)
            3) EndDate - DateTime - required (when will the booking end)
            4) CustomerName - string - required
            5) CustomerPhoneNumber - string - required
            
Ex: https://localhost:xxxxx/Order/NewOrder?VehicleID=385175d5-48f1-4653-8513-c4c6acaecd76&StartDate=2021-11-28 13:00:00&EndDate=2021-11-30 12:00:00&CustomerName=Marian&CustomerPhoneNumber=07914618716

3. /Order/CancelOrder - to cancel a booked order 
Parameters: 1) VehicleID - GUID - required (will be used to identify the booking/order and cancel it)
            2) StartDate - DateTime - required (will be used to identify the booking/order and cancel it)
            3) EndDate - DateTime - required (will be used to identify the booking/order and cancel it)

Ex: https://localhost:xxxxx/Order/CancelOrder?VehicleID=385175d5-48f1-4653-8513-c4c6acaecd76&StartDate=2021-11-28 13:00:00&EndDate=2021-11-30 12:00:00

4. /Order/FinishOrder - To finish an order/booking
Parameters: 1) VehicleID - GUID - required
            2) StartDate - DateTime - required
            3) EndDate - DateTime - required
            4) Damage - string
            5)GasolineFilled - bool (false if left empty)

Ex: https://localhost:xxxxx/Order/FinishOrder?VehicleID=A11C2FBB-682C-47AA-A88D-5624BC635F25&StartDate=2021-10-25 12:00:00&EndDate=2021-10-27 12:00:00&Damage=Aripa dreapta zgariata&GasolineFilled=true

5. /Order/GetOrders - to get a list of orders/bookings

Ex: https://localhost:xxxxx/Order/GetOrders


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
