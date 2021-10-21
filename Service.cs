using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ParkingOnBoard.MODELS
{
    public static class Service
    {
        //ADDING A NEW STATE TO THE DATABASE
        public static void AddState()
        {
            using (var context = new DatabaseContext())
            {
                string stateName = GetStateName();
                var state = context.States
                    .Where(s => s.Name == stateName)
                    .FirstOrDefault();

                if (state == null)
                {
                    state = new State();
                    state.Name = stateName.ToUpper();
                    state.CreationDate = DateTime.Now;
                    context.Add(state);
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE STATE WAS ADDED!");
                }
                else
                {
                    Console.WriteLine("THE STATE ALREADY EXISTS IN THE DATABASE");
                }
            }
        }
        //GETTING AN ID OF AN EXISTING STATE FROM THE USER 
        public static int GetExistingStateID()
        {
            using (var context = new DatabaseContext())
            {
                bool validInput = false;
                int stateID;
                string stateId = null;
                while (!validInput)
                {
                    Console.WriteLine("ENTER STATE ID: ");
                    stateId = Console.ReadLine();
                    if (Int32.TryParse(stateId, out int num))
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("PLEASE ENTER A VALID NUMBER!");
                    }

                }
                stateID = Convert.ToInt32(stateId);

                var stateSearch = context.States
                           .Include(s => s.Cities)
                           .Where(s => s.ID == stateID && !s.IsDeleted)
                           .FirstOrDefault();

                if (stateSearch == null)
                {
                    while (stateSearch == null)
                    {
                        Console.WriteLine("INVALID VALUE! THERE IS NO STATE WITH THAT ID IN OUR DATABASE!");
                        validInput = false;
                        while (!validInput)
                        {
                            Console.WriteLine("ENTER STATE ID: ");
                            stateId = Console.ReadLine();
                            if (Int32.TryParse(stateId, out int num))
                            {
                                stateID = Convert.ToInt32(stateId);
                                validInput = true;
                            }

                            stateSearch = context.States
                               .Include(s => s.Cities)
                               .Where(s => s.ID == stateID)
                               .FirstOrDefault();
                        }
                    }
                }
                return stateID;
            }
        }
        //PRINTING THE LIST OF STATES IN THE DATABASE 
        public static void PrintStateList()
        {
            using (var context = new DatabaseContext())
            {
                var states = context.States
                    .Where(s => !s.IsDeleted)
                    .ToList();
                Console.WriteLine("CHOOSE A STATE FROM THE LIST BELOW: ");
                foreach (var state in states)
                {
                    Console.WriteLine(state.ID);
                    Console.WriteLine(state.Name);
                }
            }
        }
        //PRINT THE LIST OF STATES THAT ARE VALIDATED
        public static void PrintValidatedStateList()
        {
            using (var context = new DatabaseContext())
            {
                var states = context.States
                    .Where(s => s.IsAvailable && !s.IsDeleted)
                    .ToList();

                if (states == null)
                {
                    Console.WriteLine("THERE ARE NO STATES AVAILABLE!");
                }
                else
                {
                    Console.WriteLine("CHOOSE A STATE FROM THE LIST BELOW: ");
                    foreach (var state in states)
                    {
                        Console.WriteLine(state.ID);
                        Console.WriteLine(state.Name);
                    }
                }
            }
        }
        //PRINT THE LIST OF STATES THAT ARE CLOSED
        public static void PrintClosedStateList()
        {
            using (var context = new DatabaseContext())
            {
                var states = context.States
                    .Where(s => !s.IsAvailable && !s.IsDeleted)
                    .ToList();

                if (states == null)
                {
                    Console.WriteLine("THERE ARE NO CLOSED STATES!");
                }
                else
                {
                    Console.WriteLine("CHOOSE A STATE FROM THE LIST BELOW: ");
                    foreach (var state in states)
                    {
                        Console.WriteLine(state.ID);
                        Console.WriteLine(state.Name);
                    }
                }
            }
        }
        //GETTING A STATE NAME FROM THE USER
        public static string GetStateName()
        {
            Console.WriteLine("ENTER STATE NAME: ");
            string stateName = Console.ReadLine();
            return stateName.ToUpper();
        }
        //DELETE AN EXISTING STATE
        public static void DeleteState(int stateID)
        {
            using (var context = new DatabaseContext())
            {
                var state = context.States
                    .Include(c => c.Cities)
                    .ThenInclude(s => s.Streets)
                    .ThenInclude(p => p.ParkingSlots)
                    .Where(s => s.ID == stateID && !s.IsDeleted)
                    .FirstOrDefault();

                var cities = state.Cities
                    .Where(s => !s.IsDeleted)
                    .ToList();


                if (state == null)
                {
                    Console.WriteLine("THE STATE DOES NOT EXIST IN THE DATABASE!");
                }
                else if (!state.IsDeleted)
                {
                    if (cities != null)
                    {
                        foreach (var city in cities)
                        {
                            var streets = city.Streets
                                .Where(s => !s.IsDeleted)
                                .ToList();

                            if (streets != null)
                            {
                                foreach (var street in streets)
                                {
                                    var slots = street.ParkingSlots
                                        .Where(s => !s.IsDeleted)
                                        .ToList();

                                    if (slots != null)
                                    {
                                        foreach (var slot in slots)
                                        {
                                            slot.DeletionDate = DateTime.Now;
                                            slot.IsDeleted = true;
                                            slot.IsAvailable = false;
                                        }
                                    }
                                    street.DeletionDate = DateTime.Now;
                                    street.IsAvailable = false;
                                    street.IsDeleted = true;
                                }
                            }
                            city.DeletionDate = DateTime.Now;
                            city.IsAvailable = false;
                            city.IsDeleted = true;
                        }
                    }
                    state.IsDeleted = true;
                    state.IsAvailable = false;
                    state.DeletionDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE STATE IS NOW DELETED!");
                }
            }
        }
        //CLOSING A STATE
        public static void CloseState(int stateID)
        {
            using (var context = new DatabaseContext())
            {
                var state = context.States
                    .Where(s => s.ID == stateID && !s.IsDeleted)
                    .FirstOrDefault();

                if (state.IsAvailable)
                {
                    state.IsAvailable = false;
                    state.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE STATE IS NOW CLOSED!");
                }
                else
                {
                    Console.WriteLine("THE STATE IS ALREADY CLOSED!");
                }
            }
        }
        //VALIDATING A STATE
        public static void ValidateState(int stateID)
        {
            using (var context = new DatabaseContext())
            {
                var state = context.States
                    .Where(s => s.ID == stateID)
                    .FirstOrDefault();

                if (!state.IsAvailable && !state.IsDeleted)
                {
                    state.IsAvailable = true;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE STATE IS NOW VALIDATED!");
                }
                else if (state.IsAvailable)
                {
                    Console.WriteLine("THE STATE IS ALREADY VALIDATED!");
                }
                else if (state.IsDeleted)
                {
                    Console.WriteLine("THE STATE NO LONGER EXISTS IN OUR DATABASE");
                }
            }
        }
        //ADD A NEW CITY TO THE DATABASE
        public static void AddCity()
        {
            string cityName = GetCityName();
            Console.WriteLine("TO WHICH STATE DO YOU WISH TO ADD THIS CITY: ");
            PrintStateList();
            int stateID = GetExistingStateID();
            using (var context = new DatabaseContext())
            {
                var city = context.Cities
                    .Where(c => c.Name == cityName && !c.IsDeleted)
                    .FirstOrDefault();

                var stateToUpdate = context.States
                    .Where(s => s.ID == stateID && !s.IsDeleted)
                    .Include(c => c.Cities)
                    .FirstOrDefault();

                if (city != null)
                {
                    Console.WriteLine("THE CITY ALREADY EXISTS IN THE DATABASE!");
                }
                else if (city == null)
                {
                    city = new City();
                    city.Name = cityName.ToUpper();
                    city.CreationDate = DateTime.Now;
                    stateToUpdate.Cities.Add(city);
                    stateToUpdate.ModificationDate = DateTime.Now;
                    Console.WriteLine("THE CITY WAS ADDED");
                    SaveChangesWrapper(context);
                }
            }
        }
        //GET CITY NAME FROM THE USER
        public static string GetCityName()
        {
            Console.WriteLine("ENTER CITY NAME: ");
            string cityName = Console.ReadLine();
            return cityName.ToUpper();
        }
        //GET CITY ID FROM THE USER
        public static int GetExistingCityID()
        {
            using (var context = new DatabaseContext())
            {
                bool validInput = false;
                string cityIdString = null;
                while (!validInput)
                {
                    Console.WriteLine("ENTER CITY ID: ");
                    cityIdString = Console.ReadLine();
                    if (Int32.TryParse(cityIdString, out int num))
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("PLEASE ENTER A VALID NUMBER!");
                    }

                }

                int cityID = Convert.ToInt32(cityIdString);

                var city = context.Cities
                    .Where(c => c.ID == cityID && !c.IsDeleted)
                    .FirstOrDefault();

                if (city == null)
                {
                    while (city == null)
                    {
                        Console.WriteLine("INVALID VALUE! THERE IS NO CITY WITH THAT ID IN OUR DATABASE!");
                        validInput = false;
                        while (!validInput)
                        {
                            Console.WriteLine("ENTER CITY ID: ");
                            cityIdString = Console.ReadLine();
                            if (Int32.TryParse(cityIdString, out int num))
                            {
                                cityID = Convert.ToInt32(cityIdString);
                                validInput = true;
                            }

                            city = context.Cities
                               .Where(s => s.ID == cityID && !s.IsDeleted)
                               .FirstOrDefault();
                        }
                    }
                }
                return cityID;
            }
        }
        //DELETE AN EXISTING CITY 
        public static void DeleteCity(int cityID)
        {
            using (var context = new DatabaseContext())
            {
                var city = context.Cities
                    .Where(s => s.ID == cityID && !s.IsDeleted)
                    .Include(s => s.Streets)
                    .ThenInclude(p => p.ParkingSlots)
                    .FirstOrDefault();

                var streets = city.Streets
                    .ToList();


                if (city == null)
                {
                    Console.WriteLine("THE CITY DOES NOT EXIST IN THE DATABASE!");
                }
                else
                {
                    if (streets != null)
                    {
                        foreach (var s in streets)
                        {
                            s.IsAvailable = false;
                            s.IsDeleted = true;
                            s.DeletionDate = DateTime.Now;
                            var slots = s.ParkingSlots
                                .ToList();
                            foreach (var p in slots)
                            {
                                p.IsAvailable = false;
                                p.IsDeleted = true;
                                p.DeletionDate = DateTime.Now;
                            }

                        }
                    }
                    city.IsAvailable = false;
                    city.IsDeleted = true;
                    city.DeletionDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE CITY IS NOW DELETED!");
                }
            }
        }
        //PRINT THE LIST OF CITIES FROM THE DATABASE
        public static void PrintCityList()
        {
            using (var context = new DatabaseContext())
            {
                var cityList = context.Cities
                   .Where(c => !c.IsDeleted)
                   .OrderBy(s => s.State)
                   .Select(c => new { c.ID, c.Name, c.State })
                   .ToList();

                if (cityList == null)
                {
                    Console.WriteLine("THERE ARE NO CITIES IN THE DATABASE");
                }
                else
                {
                    foreach (var record in cityList)
                    {
                        Console.WriteLine(record.ID);
                        Console.WriteLine(record.Name);
                        Console.WriteLine(record.State.Name);
                    }
                }
            }
        }
        //PRINT THE LIST OF CITIES THAT ARE AVAILABLE
        public static void PrintValidCityList()
        {
            using (var context = new DatabaseContext())
            {
                var cityList = context.Cities
                   .Where(c => !c.IsDeleted && c.IsAvailable)
                   .OrderBy(s => s.State)
                   .Select(c => new { c.ID, c.Name, c.State })
                   .ToList();

                if (cityList == null)
                {
                    Console.WriteLine("THERE ARE NO VALID CITIES AT THE MOMENT!");
                }
                else
                {
                    foreach (var record in cityList)
                    {
                        Console.WriteLine(record.ID);
                        Console.WriteLine(record.Name);
                        Console.WriteLine(record.State.Name);
                    }
                }
            }
        }
        //PRINT THE LIST OF CITIES THAT ARE CLOSED
        public static void PrintClosedCityList()
        {
            using (var context = new DatabaseContext())
            {
                var cityList = context.Cities
                   .Where(c => !c.IsDeleted && !c.IsAvailable)
                   .OrderBy(s => s.State)
                   .Select(c => new { c.ID, c.Name, c.State })
                   .ToList();
                if (cityList == null)
                {
                    Console.WriteLine("THERE ARE NO CLOSED CITIES");
                }
                else
                {
                    foreach (var record in cityList)
                    {
                        Console.WriteLine(record.ID);
                        Console.WriteLine(record.Name);
                        Console.WriteLine(record.State.Name);
                    }
                }
            }
        }
        //CLOSING A CITY
        public static void CloseCity(int cityID)
        {
            using (var context = new DatabaseContext())
            {
                var city = context.Cities
                    .Where(s => s.ID == cityID)
                    .FirstOrDefault();

                if (city.IsAvailable)
                {
                    city.IsAvailable = false;
                    city.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE CITY IS NOW CLOSED!");
                }
                else
                {
                    Console.WriteLine("THE CITY IS ALREADY VALIDATED!");
                }
            }
        }
        //VALIDATE A CITY
        public static void ValidateCity(int cityID)
        {
            using (var context = new DatabaseContext())
            {

                var city = context.Cities
                    .Where(s => s.ID == cityID)
                    .FirstOrDefault();

                if (!city.IsAvailable)
                {
                    city.IsAvailable = true;
                    city.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE CITY IS NOW VALIDATED!");
                }
                else
                {
                    Console.WriteLine("THE CITY IS ALREADY VALIDATED!");
                }
            }
        }
        //ADD A NEW STREET TO THE DATABASE
        public static void AddStreet()
        {
            string streetName = GetStreetName();
            bool validInput = false;
            string sidesAvailableString = null;
            int sidesAvailable = 0;
            bool isValid = false;

            while (!isValid)
            {
                while (!validInput)
                {
                    Console.WriteLine("ENTER SIDES AVAILABLE: ");
                    sidesAvailableString = Console.ReadLine();
                    if (Int32.TryParse(sidesAvailableString, out int num))
                    {
                        sidesAvailable = Convert.ToInt32(sidesAvailableString);
                        if (sidesAvailable == 1 || sidesAvailable == 2)
                        {
                            isValid = true;
                            validInput = true;
                        }
                        else
                        {
                            Console.WriteLine("INVALID VALUE! A STREET CAN HAVE 1 OR 2 SIDES!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("PLEASE ENTER A VALID NUMBER!");
                    }
                }
            }

            Console.WriteLine("TO WHICH CITY WOULD YOU ADD THIS STREET: ");
            PrintCityList();

            int cityID = GetExistingCityID();

            using (var context = new DatabaseContext())
            {
                var cityToUpdate = context.Cities
                    .Include(s => s.Streets)
                    .Where(c => c.ID == cityID)
                    .First();

                var street = context.Streets
                .Where(r => r.Name == streetName && r.City == cityToUpdate && !r.IsDeleted)
                .FirstOrDefault();

                if (street != null)
                {
                    Console.WriteLine("THE STREET ALREADY EXISTS IN THE DATABASE! ");
                }
                else if (street == null)
                {
                    var newStreet = new Street();
                    newStreet.Name = streetName;
                    newStreet.SidesAvailable = sidesAvailable;
                    newStreet.CreationDate = DateTime.Now;
                    cityToUpdate.Streets.Add(newStreet);
                    cityToUpdate.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE STREET WAS ADDED!");
                }
            }
        }
        //PRINT THE LIST OF STREETS
        public static void PrintStreetList()
        {
            using (var context = new DatabaseContext())
            {
                var streetList = context.Streets
                   .Where(c => !c.IsDeleted)
                   .OrderBy(s => s.City)
                   .Select(c => new { c.ID, c.Name, c.City })
                   .ToList();

                if (streetList == null)
                {
                    Console.WriteLine("THERE ARE NO STREETS AVAILABLE");
                }
                else
                {
                    foreach (var record in streetList)
                    {
                        Console.WriteLine(record.ID);
                        Console.WriteLine(record.Name);
                        Console.WriteLine(record.City.Name);
                    }
                }
            }
        }
        //PRINT THE LIST OF CITIES THAT ARE AVAILABLE
        public static void PrintValidStreetList()
        {
            using (var context = new DatabaseContext())
            {
                var streetList = context.Streets
                   .Where(c => !c.IsDeleted && c.IsAvailable)
                   .OrderBy(c => c.City)
                   .Select(c => new { c.ID, c.Name, c.City })
                   .ToList();
                if (streetList == null)
                {
                    Console.WriteLine("THERE ARE NO VALID STREETS");
                }
                else
                {
                    foreach (var record in streetList)
                    {
                        Console.WriteLine(record.ID);
                        Console.WriteLine(record.Name);
                        Console.WriteLine(record.City.Name);
                    }
                }
            }
        }
        //PRINT THE LIST OF CITIES THAT ARE CLOSED
        public static void PrintClosedStreetList()
        {
            using (var context = new DatabaseContext())
            {
                var streetList = context.Streets
                   .Where(c => !c.IsDeleted && !c.IsAvailable)
                   .OrderBy(c => c.City)
                   .Select(c => new { c.ID, c.Name, c.City })
                   .ToList();

                if (streetList == null)
                {
                    Console.WriteLine("THERE ARE NO CLOSED STREETS FOR THE MOMENT!");
                }
                else
                {
                    foreach (var record in streetList)
                    {
                        Console.WriteLine(record.ID);
                        Console.WriteLine(record.Name);
                        Console.WriteLine(record.City.Name);
                    }
                }
            }
        }
        //GET STREET NAME FROM THE USER
        public static string GetStreetName()
        {
            Console.WriteLine("ENTER STREET NAME: ");
            string streetName = Console.ReadLine();
            return streetName.ToUpper();
        }
        //DELETE STREET 
        public static void DeleteStreet(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(p => p.ParkingSlots)
                    .Where(s => s.ID == streetID && !s.IsDeleted)
                    .FirstOrDefault();

                var slots = street.ParkingSlots
                    .Where(s => !s.IsDeleted)
                    .ToList();

                if (street.IsDeleted)
                {
                    Console.WriteLine("THE STREET IS ALREADY DELETED!");
                }
                else
                {
                    if (slots != null)
                    {
                        foreach (var s in slots)
                        {
                            s.IsAvailable = false;
                            s.IsDeleted = true;
                            s.DeletionDate = DateTime.Now;
                        }
                    }
                    street.IsDeleted = true;
                    street.IsAvailable = false;
                    street.DeletionDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE STREET IS NOW DELETED!");
                }
            }
        }
        //CLOSE STREET
        public static void CloseStreet(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                        .Where(s => s.ID == streetID && !s.IsDeleted)
                        .FirstOrDefault();

                if (street.IsAvailable)
                {
                    Console.WriteLine("WHICH IS THE REASON FOR CLOSING THE STREET? (ENTER THE NUMBER)");
                    int i = 1;
                    foreach (string reasonOption in Enum.GetNames(typeof(Reasons)))
                    {
                        Console.WriteLine($"{i}. {reasonOption}");
                        i++;
                    }
                    bool validInput = false;
                    string chosen = null;
                    bool validReasonChoice = false;
                    Reasons reason;
                    while (!validReasonChoice)
                    {
                        validInput = false;
                        while (!validInput)
                        {
                            chosen = Console.ReadLine();
                            if (Int32.TryParse(chosen, out int num))
                            {
                                validInput = true;
                            }
                            else
                            {
                                Console.WriteLine("PLEASE ENTER A VALID NUMBER!");
                            }
                        }
                        int chosenValue = Convert.ToInt32(chosen);
                        if (Enum.IsDefined(typeof(Reasons), chosenValue))
                        {
                            reason = Enum.Parse<Reasons>(chosen);
                            street.Reason = reason;
                            validReasonChoice = true;
                            street.IsAvailable = false;
                            street.ModificationDate = DateTime.Now;
                            SaveChangesWrapper(context);
                            Console.WriteLine("THE STREET IS NOW CLOSED!");
                        }
                        else
                        {
                            Console.WriteLine("INVALID VALUE!");
                            Console.WriteLine("TRY AGAIN!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("THE STREET IS ALREADY CLOSED");
                }
            }
        }
        //VALIDATE STREET
        public static void ValidateStreet(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                        .Where(s => s.ID == streetID && !s.IsDeleted)
                        .FirstOrDefault();

                if (!street.IsAvailable)
                {
                    street.Reason = 0;
                    street.IsAvailable = true;
                    street.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE STREET IS NOW VALIDATED!");
                }
                else
                {
                    Console.WriteLine("THE STREET IS ALREADY VALIDATED!");
                }
            }
        }
        //GET STREET ID 
        public static int GetExistingStreetID()
        {
            using (var context = new DatabaseContext())
            {
                bool validInput = false;
                string streetIdString = null;
                int streetID = 0;
                while (!validInput)
                {
                    Console.WriteLine("ENTER STREET ID: ");
                    streetIdString = Console.ReadLine();
                    if (Int32.TryParse(streetIdString, out int num))
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("PLEASE ENTER A VALID NUMBER!");
                    }
                }

                streetID = Convert.ToInt32(streetIdString);

                var street = context.Streets
                    .Where(c => c.ID == streetID && !c.IsDeleted)
                    .FirstOrDefault();

                if (street == null)
                {
                    while (street == null)
                    {
                        Console.WriteLine("INVALID VALUE! THERE IS NO STREET WITH THAT ID IN OUR DATABASE!");
                        validInput = false;
                        while (!validInput)
                        {
                            Console.WriteLine("ENTER STREET ID: ");
                            streetIdString = Console.ReadLine();
                            if (Int32.TryParse(streetIdString, out int num))
                            {
                                streetID = Convert.ToInt32(streetIdString);
                                validInput = true;
                            }

                            street = context.Streets
                               .Where(s => s.ID == streetID && !s.IsDeleted)
                               .FirstOrDefault();
                        }
                    }
                }
                return streetID;
            }
        }
        //ADD PARKING SLOTS TO AN EXISTING STREET
        public static void AddParkingSlot(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                int maxPositionNr;
                var streetToUpdate = context.Streets
                       .Include(p => p.ParkingSlots.Where(p => !p.IsDeleted))
                       .Where(s => s.ID == streetID && !s.IsDeleted)
                       .FirstOrDefault();

                int parkingSlotsTotal = streetToUpdate.ParkingSlots.Count();
                if (parkingSlotsTotal == 0)
                {
                    maxPositionNr = 0;
                }
                else
                {
                    maxPositionNr = parkingSlotsTotal;
                }


                Console.WriteLine($"THERE ARE {parkingSlotsTotal} parking slots in this street.");

                Console.WriteLine("ENTER THE NUMBER OF PARKING SLOTS THAT YOU WANT TO ADD TO THIS STREET: ");
                bool validInput = false;
                string nrOfSLots = null;
                while (!validInput)
                {
                    nrOfSLots = Console.ReadLine();
                    if (Int32.TryParse(nrOfSLots, out int num) && Convert.ToInt32(nrOfSLots) >= 0)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("PLEASE ENTER A VALID NUMBER!");
                    }

                }
                int numberOfSLots = Convert.ToInt32(nrOfSLots);

                for (int i = maxPositionNr; i < (maxPositionNr + numberOfSLots); i++)
                {
                    var parkingSlot = new ParkingSlot();
                    parkingSlot.PositionNumber = i;
                    parkingSlot.CreationDate = DateTime.Now;
                    streetToUpdate.ParkingSlots.Add(parkingSlot);
                    streetToUpdate.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                }
                Console.WriteLine($"{numberOfSLots} SLOTS WERE ADDED TO {streetToUpdate.Name} STREET!");
            }
        }
        //REMOVE A PARKING SLOT
        public static void RemoveParkingSlot(int slotPositionNr, int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var streetToUpdate = context.Streets
                    .Include(s => s.ParkingSlots)
                    .Where(s => s.ID == streetID && !s.IsDeleted)
                    .FirstOrDefault();

                var slot = streetToUpdate.ParkingSlots
                    .Where(s => s.PositionNumber == slotPositionNr && !s.IsDeleted)
                    .FirstOrDefault();

                if (slot == null)
                {
                    Console.WriteLine("THE SLOT DOES NOT EXIST!");
                }
                else
                {
                    slot.IsDeleted = true;
                    slot.IsAvailable = false;
                    slot.DeletionDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE SLOT IS NOW REMOVED!");
                }
            }
        }
        //GET THE POSITION OF A SLOT
        public static int GetSlotPosition(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(s => s.ParkingSlots)
                    .Where(s => s.ID == streetID && !s.IsDeleted)
                    .FirstOrDefault();

                bool validInput = false;
                string slotPositionNrString = null;
                while (!validInput)
                {
                    Console.WriteLine("ENTER SLOT POSITION NUMBER: ");
                    slotPositionNrString = Console.ReadLine();

                    if (Int32.TryParse(slotPositionNrString, out int num))
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("PLEASE ENTER A VALID NUMBER!");
                    }

                }

                int slotPositionNr = Convert.ToInt32(slotPositionNrString);

                var slot = street.ParkingSlots
                    .Where(c => c.PositionNumber == slotPositionNr && !c.IsDeleted)
                    .FirstOrDefault();

                if (slot == null)
                {
                    while (slot == null)
                    {
                        Console.WriteLine("INVALID VALUE! THERE IS NO SLOT WITH THAT ID IN THIS STREET!");
                        validInput = false;
                        while (!validInput)
                        {
                            Console.WriteLine("ENTER SLOT POSITION NUMBER: ");
                            slotPositionNrString = Console.ReadLine();
                            if (Int32.TryParse(slotPositionNrString, out int num))
                            {
                                slotPositionNr = Convert.ToInt32(slotPositionNrString);
                                validInput = true;
                            }

                            slot = context.ParkingSlots
                               .Where(s => s.PositionNumber == slotPositionNr && !s.IsDeleted)
                               .FirstOrDefault();
                        }
                    }
                }
                return slotPositionNr;
            }
        }
        //PRINT VALID SLOTS OF A STREET
        public static void PrintValidSlotsOfAStreet(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(p => p.ParkingSlots)
                    .Where(s => s.ID == streetID)
                    .FirstOrDefault();

                var slots = street.ParkingSlots
                    .Where(p => !p.IsDeleted && p.IsAvailable)
                    .ToList();
                int totalNrOfSlots = slots.Count();
                Console.WriteLine($"THERE ARE {totalNrOfSlots} SLOTS IN THIS STREET");
                Console.WriteLine("THE ID FOR EACH SLOT IS LISTED BELOW: ");
                foreach (var slot in slots)
                {
                    Console.WriteLine(slot.PositionNumber);
                }
            }
        }
        //PRINT CLOSED SLOTS OF A STREET
        public static void PrintClosedSlotsOfAStreet(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(p => p.ParkingSlots)
                    .Where(s => s.ID == streetID)
                    .FirstOrDefault();

                var slots = street.ParkingSlots
                    .Where(p => !p.IsDeleted && !p.IsAvailable)
                    .ToList();
                int totalNrOfSlots = slots.Count();
                Console.WriteLine($"THERE ARE {totalNrOfSlots} SLOTS IN THIS STREET");
                Console.WriteLine("THE ID FOR EACH SLOT IS LISTED BELOW: ");
                foreach (var slot in slots)
                {
                    Console.WriteLine(slot.PositionNumber);
                }
            }
        }
        //PRINT THE SLOTS OF A STREET
        public static void PrintSlotsOfAStreet(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(p => p.ParkingSlots)
                    .Where(s => s.ID == streetID)
                    .FirstOrDefault();

                var slots = street.ParkingSlots
                    .Where(p => !p.IsDeleted)
                    .ToList();
                int totalNrOfSlots = slots.Count();
                Console.WriteLine($"THERE ARE {totalNrOfSlots} SLOTS IN THIS STREET");
                Console.WriteLine("THE ID FOR EACH SLOT IS LISTED BELOW: ");
                foreach (var slot in slots)
                {
                    Console.WriteLine(slot.PositionNumber);
                }
            }
        }
        //CLOSE A PARKING SLOT
        public static void CloseParkingSlot(int slotPositionNr, int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(s => s.ParkingSlots)
                    .Where(s => s.ID == streetID && !s.IsDeleted)
                    .FirstOrDefault();

                var slot = street.ParkingSlots
                    .Where(s => s.PositionNumber == slotPositionNr && !s.IsDeleted)
                    .FirstOrDefault();

                if (slot == null)
                {
                    Console.WriteLine("INVALID ID!");
                }
                else if (slot.IsAvailable)
                {
                    slot.IsAvailable = false;
                    slot.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE SLOT IS NOW CLOSED!");
                }
                else
                {
                    Console.WriteLine("THE SLOT IS ALREADY CLOSED!");
                }

            }
        }
        //VALIDATE A PARKING SLOT
        public static void ValidateParkingSlot(int slotPositionNr, int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(s => s.ParkingSlots)
                    .Where(s => s.ID == streetID)
                    .FirstOrDefault();

                var slot = street.ParkingSlots
                    .Where(s => s.ID == slotPositionNr)
                    .FirstOrDefault();

                if (slot == null)
                {
                    Console.WriteLine("INVALID ID!");
                }
                else if (!slot.IsAvailable)
                {
                    slot.IsAvailable = true;
                    slot.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                }
                else
                {
                    Console.WriteLine("THE STREET IS ALREADY VALIDATED");
                }
            }
        }
        //PARKING FUNCTION
        public static void Parking()
        {
            using (var context = new DatabaseContext())
            {
                bool isParked = false;
                Console.WriteLine("AT WHAT STREET DO YOU WISH TO PARK? ( ENTER * TO CHOOSE FROM A LIST )");
                string streetName = GetStreetName();

                if (streetName == "*")
                {
                    var streetList = context.Streets
                        .Where(s => !s.IsDeleted && s.IsAvailable)
                        .Include(p => p.ParkingSlots)
                        .ToList();

                    foreach (var street1 in streetList)
                    {
                        Console.WriteLine(street1.Name);
                        Console.WriteLine($"THERE ARE {street1.ParkingSlots.Where(p => p.IsAvailable).Count()} SLOTS IN THIS STREET");
                    }
                    streetName = GetStreetName();
                }

                var street = context.Streets
                     .Include(p => p.ParkingSlots)
                     .Where(s => s.Name == streetName.ToUpper())
                     .FirstOrDefault();

                if (street == null)
                {
                    Console.WriteLine("THE STREET YOU SPECIFIED DOES NOT EXIST IN OUR DATABASE");
                }
                else
                {
                    while (!isParked)
                    {
                        if (!street.IsAvailable)
                        {
                            Console.WriteLine($"THIS STREET IS NOT AVAILABLE DUE TO {Enum.GetName(street.Reason)}!");
                            isParked = true;
                        }

                        else
                        {
                            var slot = street.ParkingSlots
                                .Where(s => !s.IsDeleted && s.IsAvailable)
                                .ToList();


                            int slotNR = slot.Count();
                            if (slotNR == 0 && slotNR != (-1))
                            {
                                while (slotNR == 0 && slotNR != (-1))
                                {
                                    Console.WriteLine("THERE ARE NO AVAILABLE PARKING SLOTS IN THIS STREET. WOULD YOU LIKE TO TRY ANOTHER STREET?");
                                    Console.WriteLine("[1] YES ");
                                    Console.WriteLine("[2] NO ");

                                    string selected = Console.ReadLine();

                                    switch (selected)
                                    {
                                        case "1":
                                            streetName = GetStreetName();

                                            street = context.Streets
                                              .Include(p => p.ParkingSlots)
                                              .Where(s => s.Name == streetName.ToUpper())
                                              .FirstOrDefault();

                                            slot = street.ParkingSlots
                                               .Where(s => !s.IsDeleted && s.IsAvailable)
                                               .ToList();

                                            slotNR = slot.Count();

                                            break;
                                        case "2":
                                            isParked = true;
                                            Console.WriteLine("WE ARE SORRY WE COULD NOT HELP YOU!");
                                            slotNR = -1;
                                            break;
                                        default:
                                            Console.WriteLine("INVALID VALUE!");
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var s in slot)
                                {
                                    Console.WriteLine(s.PositionNumber);
                                }
                                bool validInput = false;
                                string slotPositionNrString = null;
                                while (!validInput)
                                {
                                    Console.WriteLine("ENTER SLOT POSITION NUMBER: ");
                                    slotPositionNrString = Console.ReadLine();

                                    if (Int32.TryParse(slotPositionNrString, out int num))
                                    {
                                        validInput = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("PLEASE ENTER A VALID NUMBER!");
                                    }
                                }

                                int slotPositionNr = Convert.ToInt32(slotPositionNrString);

                                var selectedSlot = street.ParkingSlots
                                    .Where(c => c.PositionNumber == slotPositionNr && !c.IsDeleted && c.IsAvailable)
                                    .FirstOrDefault();


                                if (selectedSlot == null)
                                {
                                    while (selectedSlot == null || selectedSlot.IsDeleted)
                                    {
                                        Console.WriteLine("INVALID VALUE! THERE IS NO AVAILABLE SLOT WITH THAT ID IN THIS STREET!");
                                        validInput = false;
                                        while (!validInput)
                                        {
                                            Console.WriteLine("ENTER SLOT POSITION NUMBER: ");
                                            slotPositionNrString = Console.ReadLine();
                                            if (Int32.TryParse(slotPositionNrString, out int num))
                                            {
                                                slotPositionNr = Convert.ToInt32(slotPositionNrString);
                                                validInput = true;
                                            }

                                            selectedSlot = context.ParkingSlots
                                               .Where(s => s.PositionNumber == slotPositionNr && !s.IsDeleted && s.IsAvailable)
                                               .FirstOrDefault();
                                        }
                                    }
                                }
                                else
                                {
                                    selectedSlot.IsAvailable = false;
                                    isParked = true;
                                    Console.WriteLine("THE SLOT IS NOW RESERVED FOR YOU!");
                                    SaveChangesWrapper(context);
                                }
                            }
                        }
                    }
                }
            }
        }
        //UNPARKING FUNCTION
        public static void Unparking()
        {
            Console.WriteLine("AT WHAT STREET HAVE YOU PARKED? ");
            PrintValidStreetList();
            int streetID = GetExistingStreetID();
            PrintClosedSlotsOfAStreet(streetID);
            int slotNr = GetSlotPosition(streetID);
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(p => p.ParkingSlots)
                    .Where(s => s.ID == streetID)
                    .FirstOrDefault();

                var slot = street.ParkingSlots
                    .Where(s => s.PositionNumber == slotNr)
                    .FirstOrDefault();

                if (!slot.IsAvailable)
                {
                    slot.IsAvailable = true;
                    slot.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE SLOT IS NOW FREE!");
                }
                else if (slot.IsAvailable && !slot.IsDeleted)
                {
                    Console.WriteLine("THE SLOT IS ALREADY FREE");
                }
                else
                {
                    Console.WriteLine("INVALID VALUE");
                }
            }
        }
        //MARK A SLOT AS INVALID
        public static void TurnSlotInvalid()
        {
            Console.WriteLine("AT WHAT STREET IS THE SLOT POSITIONED? ");
            PrintStreetList();
            int streetID = GetExistingStreetID();
            PrintSlotsOfAStreet(streetID);
            int slotNr = GetSlotPosition(streetID);

            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(p => p.ParkingSlots)
                    .Where(s => s.ID == streetID)
                    .FirstOrDefault();

                var slot = street.ParkingSlots
                    .Where(s => s.PositionNumber == slotNr)
                    .FirstOrDefault();

                if (slot.IsValid && !slot.IsDeleted)
                {
                    slot.IsValid = false;
                    slot.IsAvailable = false;
                    slot.ModificationDate = DateTime.Now;
                    SaveChangesWrapper(context);
                    Console.WriteLine("THE SLOT MARKED AS INVALID!");
                }
                else if (!slot.IsValid && !slot.IsDeleted)
                {
                    Console.WriteLine("THE SLOT IS ALREADY INVALID");
                }
                else
                {
                    Console.WriteLine("INVALID VALUE");
                }
            }
        }
        //PRINT CITY STATISTICS
        public static void CityStatistics(int cityID)
        {
            using (var context = new DatabaseContext())
            {
                var city = context.Cities
                    .Where(c => c.ID == cityID)
                    .Include(s => s.Streets)
                    .ThenInclude(s => s.ParkingSlots)
                    .FirstOrDefault();

                var streets = city.Streets
                        .Where(s => !s.IsDeleted)
                        .ToList();

                if (city == null)
                {
                    Console.WriteLine("THERE ARE NO CITIES WITH THIS ID!");
                }
                else if (streets == null)
                {
                    Console.WriteLine("THERE ARE NO STREETS IN THIS CITY");
                }
                else if (streets != null)
                {
                    var LessOccupied = new List<Street>();
                    var HeavyOccupied = new List<Street>();

                    foreach (var street in streets)
                    {
                        var slots = street.ParkingSlots
                           .Where(s => !s.IsDeleted)
                           .ToList();

                        int parkingSlotsTotal = slots.Count();

                        if (parkingSlotsTotal == 0)
                        {
                            Console.WriteLine("THERE ARE NO PARKING SLOTS IN THIS STREET!");
                        }
                        else
                        {
                            double parkingOccuppiedSlots = slots
                                 .Where(s => !s.IsAvailable && s.IsValid)
                                 .Count();

                            double parkingFreeSlots = slots
                                .Where(s => s.IsAvailable)
                                .Count();

                            double parkingInvalidSlots = slots
                                .Where(s => !s.IsValid && !s.IsDeleted)
                                .Count();

                            double occupiedSlotsPercentage = (parkingOccuppiedSlots / parkingSlotsTotal) * 100;
                            double freeSlotsPercentage = (parkingFreeSlots / parkingSlotsTotal) * 100;
                            double invalidSLotsPercentage = (parkingInvalidSlots / parkingSlotsTotal) * 100;
                            Console.WriteLine($"STREET {street.Name}");
                            Console.WriteLine($"OCCUPIED SLOTS: {occupiedSlotsPercentage} %");
                            Console.WriteLine($"FREE SLOTS: {freeSlotsPercentage}%");
                            Console.WriteLine($"INVALID SLOTS: { invalidSLotsPercentage }%");

                            if (occupiedSlotsPercentage > 75.0)
                            {
                                HeavyOccupied.Add(street);
                            }
                            else if (occupiedSlotsPercentage < 25.0)
                            {
                                LessOccupied.Add(street);
                            }
                        }
                    }

                    int lessOccupiedCount = LessOccupied.Count();
                    int heavyOccupiedCount = HeavyOccupied.Count();

                    if (lessOccupiedCount != 0)
                    {
                        Console.WriteLine("LESS OCCUPIED STREETS IN THIS CITY ARE: ");
                        foreach (var item in LessOccupied)
                        {
                            Console.WriteLine($"{item.Name}");
                        }
                    }
                    else if (heavyOccupiedCount != 0)
                    {
                        Console.WriteLine("HEAVY OCCUPIED STREETS IN THIS CITY ARE: ");
                        foreach (var item in HeavyOccupied)
                        {
                            Console.WriteLine($"{item.Name}");
                        }
                    }
                    else if(lessOccupiedCount==0)
                    {
                        Console.WriteLine("THERE ARE NO STREETS WITH LESS THAN 25% OCCUPIED SLOTS!");
                    }

                    else if (heavyOccupiedCount==0)
                    {
                        Console.WriteLine("THERE ARE NO STREETS WITH MORE THAN 75% OCCUPIED SLOTS!");
                    }                                  
                }
            }
        }
        //PRINT STREET STATISTICS
        public static void StreetStatistics(int streetID)
        {
            using (var context = new DatabaseContext())
            {
                var street = context.Streets
                    .Include(s => s.ParkingSlots)
                    .Where(s => s.ID == streetID)
                    .FirstOrDefault();

                int parkingSlotsTotal = street.ParkingSlots.Count();

                if (parkingSlotsTotal == 0)
                {
                    Console.WriteLine("THERE ARE NO PARKING SLOTS IN THIS STREET!");
                }
                else
                {
                    double parkingOccuppiedSlots = street.ParkingSlots
                    .Where(s => !s.IsAvailable && s.IsValid)
                    .Count();

                    double parkingFreeSlots = street.ParkingSlots
                        .Where(s => s.IsAvailable)
                        .Count();

                    double parkingInvalidSlots = street.ParkingSlots
                        .Where(s => !s.IsValid && !s.IsDeleted)
                        .Count();

                    Console.WriteLine($"STREET {street.Name} ");
                    Console.WriteLine($"OCCUPIED SLOTS: { (parkingOccuppiedSlots / parkingSlotsTotal) * 100}%");
                    Console.WriteLine($"FREE SLOTS: { (parkingFreeSlots / parkingSlotsTotal) * 100}%");
                    Console.WriteLine($"INVALID SLOTS: { (parkingInvalidSlots / parkingSlotsTotal) * 100}%");
                }
            }
        }
        static void SaveChangesWrapper(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}



