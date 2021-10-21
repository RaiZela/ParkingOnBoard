using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ParkingOnBoard.MODELS
{
    class Program
    { 
        static void Main(string[] args)
        {
            bool run;
            do
            {
                PrintMenuOptions();
                run = ReadCommand();
            }
            while (run);
        }
        public static void PrintMenuOptions()
        {
            Console.WriteLine("Choose an option: ");
            Console.WriteLine(" [1] Add a state ");
            Console.WriteLine(" [2] Print State List ");
            Console.WriteLine(" [3] Close a state ");
            Console.WriteLine(" [4] Validate a state ");
            Console.WriteLine(" [5] Delete a state ");
            Console.WriteLine(" [6] Add a city ");
            Console.WriteLine(" [7] Print City List ");
            Console.WriteLine(" [8] Close a city ");
            Console.WriteLine(" [9] Validate a city ");
            Console.WriteLine(" [10] Delete a city ");
            Console.WriteLine(" [11] Add a street ");
            Console.WriteLine(" [12] Print street list ");
            Console.WriteLine(" [13] Close a street ");
            Console.WriteLine(" [14] Validate a street ");
            Console.WriteLine(" [15] Delete a street ");
            Console.WriteLine(" [16] Add a parking slot to a street");
            Console.WriteLine(" [17] Remove a parking slot from a street");
            Console.WriteLine(" [18] Close a parking slot ");
            Console.WriteLine(" [19] Validate a parking slot ");
            Console.WriteLine(" [20] Parking");
            Console.WriteLine(" [21] Unparking");
            Console.WriteLine(" [22] Mark slot as invalid");
            Console.WriteLine(" [23] City Statistics");
            Console.WriteLine(" [24] Street Statistics");
            Console.WriteLine(" Exit ");
        }

        public static bool ReadCommand()
        {
            bool run = true;
            string selected = Console.ReadLine();
            int streetID = 0;
            int slotPositionNr = 0;
            int stateID = 0;
            int cityID = 0;


            if (selected.ToLower() == "exit")
            {
                run = false;
                return run;
            }
            switch (selected)
            {
                //ADD A STATE
                case "1":
                    Service.AddState();
                    break;
                //PRINT THE LIST OF STATES
                case "2":
                    Service.PrintStateList();
                    break;
                //CLOSE A STATE
                case "3":
                    Service.PrintValidatedStateList();
                    stateID = Service.GetExistingStateID();
                    Service.CloseState(stateID);
                    break;
                //VALIDATE A STATE
                case "4":
                    Service.PrintClosedStateList();
                    stateID = Service.GetExistingStateID();
                    Service.ValidateState(stateID);
                    break;
                //DELETE A STATE AND ALL ITS CITIES
                case "5":
                    Service.PrintStateList();
                    stateID = Service.GetExistingStateID();
                    Service.DeleteState(stateID);
                    break;
                //ADD A NEW CITY
                case "6":
                    Service.AddCity();
                    break;
                //PRINT THE LIST OF CITIES
                case "7":
                    Service.PrintCityList();
                    break;
                //CLOSE A CITY
                case "8":
                    Service.PrintValidCityList();
                    cityID = Service.GetExistingCityID();
                    Service.CloseCity(cityID);
                    break;
                //VALIDATE A CITY
                case "9":
                    Service.PrintClosedCityList();
                    cityID = Service.GetExistingCityID();
                    Service.ValidateCity(cityID);
                    break;
                //DELETE CITY AND ITS STREETS
                case "10":
                    Service.PrintCityList();
                    cityID = Service.GetExistingCityID();
                    Service.DeleteCity(cityID);
                    break;
                //ADD A STREET
                case "11":
                    Service.AddStreet();
                    break;
                //PRINT STREET LIST
                case "12":
                    Service.PrintStreetList();
                    break;
                //CLOSE A STREET
                case "13":
                    Service.PrintValidStreetList();
                    streetID = Service.GetExistingStreetID();
                    Service.CloseStreet(streetID);
                    break;
                //VALIDATE A STREET
                case "14":
                    Service.PrintClosedStreetList();
                    streetID = Service.GetExistingStreetID();
                    Service.ValidateStreet(streetID);
                    break;
                //DELETE A STREET
                case "15":
                    Service.PrintStreetList();
                    streetID = Service.GetExistingStreetID();
                    Service.DeleteStreet(streetID);
                    break;
                //ADD PARKING SLOT TO A STREET
                case "16":
                    Service.PrintStreetList();
                    streetID = Service.GetExistingStreetID();
                    Service.AddParkingSlot(streetID);
                    break;
                //REMOVE A PARKING SLOT 
                case "17":
                    Service.PrintStreetList();
                    streetID = Service.GetExistingStreetID();
                    Service.PrintSlotsOfAStreet(streetID);
                    slotPositionNr = Service.GetSlotPosition(streetID);
                    Service.RemoveParkingSlot(slotPositionNr, streetID);
                    break;
                //CLOSE A PARKING SLOT
                case "18":
                    Service.PrintStreetList();
                    streetID = Service.GetExistingStreetID();
                    Service.PrintValidSlotsOfAStreet(streetID);
                    slotPositionNr = Service.GetSlotPosition(streetID);
                    Service.CloseParkingSlot(slotPositionNr, streetID);
                    break;
                //VALIDATE A PARKING SLOT
                case "19":
                    Service.PrintStreetList();
                    streetID = Service.GetExistingStreetID();
                    Service.PrintClosedSlotsOfAStreet(streetID);
                    slotPositionNr = Service.GetSlotPosition(streetID);
                    Service.ValidateParkingSlot(slotPositionNr, streetID);
                    break;
                //PARKING OPTION
                case "20":
                    Service.Parking();
                    break;
                //UNPARKING OPTION
                case "21":
                    Service.Unparking();
                    break;
                //TURN SLOT AS INVALID
                case "22":
                    Service.TurnSlotInvalid();
                    break;
                //CITY STATISTICS
                case "23":
                    Service.PrintCityList();
                    cityID = Service.GetExistingCityID();
                    Service.CityStatistics(cityID);
                    break;
                //STREET STATISTICS
                case "24":
                    Service.PrintStreetList();
                    streetID = Service.GetExistingStreetID();
                    Service.StreetStatistics(streetID);
                    break;
                default:
                    Console.WriteLine("PLEASE ENTER A VALUE FROM OPTIONS ABOVE");
                    break;
            }
            return run;
        }
    }
}






