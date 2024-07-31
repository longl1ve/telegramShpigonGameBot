namespace telegramShpigonGameBot
{
    // This class stores localization for English language
    internal class TopicsEnglish
    {
        // List for topics
        private List<string> topics = new List<string>
        {
            "Airport",
            "Beach",
            "Film Studio",
            "Hospital",
            "Military Base",
            "Casino",
            "Space Station",
            "Submarine",
            "Cruise Ship",
            "Museum"
        };

        // Dictionary with locations
        private Dictionary<string, List<string>> locations = new Dictionary<string, List<string>>
        {
            {"Airport", new List<string>
                {
                    "Departure Area",
                    "Waiting Hall",
                    "Runway",
                    "Baggage Claim",
                    "Security Check"
                }},
            {"Beach", new List<string>
                {
                    "Beach Bar",
                    "Swimming Area",
                    "Surfing Area",
                    "Beach Volleyball Court",
                    "Umbrellas and Sunbeds"
                }},
            {"Film Studio", new List<string>
                {
                    "Filming Set",
                    "Makeup Room",
                    "Director's Room",
                    "Camera Service Point",
                    "Editing Room"
                }},
            {"Hospital", new List<string>
                {
                    "Emergency Department",
                    "Operating Room",
                    "Intensive Care Unit",
                    "Laboratory",
                    "Patient Room"
                }},
            {"Military Base", new List<string>
                {
                    "Command Center",
                    "Barracks",
                    "Training Ground",
                    "Shooting Range",
                    "Vehicle Hangar"
                }},
            {"Casino", new List<string>
                {
                    "Slot Machine Hall",
                    "Poker Table",
                    "Roulette",
                    "Bar",
                    "VIP Room"
                }},
            {"Space Station", new List<string>
                {
                    "Command Module",
                    "Space Laboratory",
                    "Living Module",
                    "Airlock",
                    "Space Gym"
                }},
            {"Submarine", new List<string>
                {
                    "Captain's Bridge",
                    "Torpedo Bay",
                    "Engine Room",
                    "Living Quarters",
                    "Galley"
                }},
            {"Cruise Ship", new List<string>
                {
                    "Pool Deck",
                    "Restaurant",
                    "Nightclub",
                    "Cabins",
                    "Gym"
                }},
            {"Museum", new List<string>
                {
                    "Exhibition Hall",
                    "Ancient Artifacts Room",
                    "Interactive Exhibits Room",
                    "Souvenir Shop",
                    "Archive"
                }},
        };

        // Dictionary with roles
        private Dictionary<string, List<string>> roles = new Dictionary<string, List<string>>
        {
            {"Departure Area", new List<string>
                {
                    "Passenger",
                    "Airline Representative",
                    "Customs Officer",
                    "Security Guard",
                    "Pilot",
                    "Flight Attendant",
                    "Airport Worker",
                    "Tourist",
                    "Businessman",
                    "Duty-Free Shop Employee"
                }},
            {"Waiting Hall", new List<string>
                {
                    "Passenger",
                    "Crew Member",
                    "Security Guard",
                    "Cleaner",
                    "Tourist",
                    "Businessman",
                    "Musician",
                    "Barista",
                    "Child",
                    "Blogger"
                }},
            {"Runway", new List<string>
                {
                    "Pilot",
                    "Flight Attendant",
                    "Technician",
                    "Baggage Handler",
                    "Security Guard",
                    "Air Traffic Controller",
                    "Tourist",
                    "Photographer",
                    "Security Inspector",
                    "Firefighter"
                }},
            {"Baggage Claim", new List<string>
                {
                    "Passenger",
                    "Security Guard",
                    "Baggage Handler",
                    "Tourist",
                    "Businessman",
                    "Customs Officer",
                    "Airport Employee",
                    "Parent with Child",
                    "Cleaner",
                    "Photographer"
                }},
            {"Security Check", new List<string>
                {
                    "Security Guard",
                    "Passenger",
                    "Customs Officer",
                    "Security Service Employee",
                    "Tourist",
                    "Businessman",
                    "Airport Worker",
                    "Crew Member",
                    "Detector Specialist",
                    "Service Dog"
                }},
            {"Beach Bar", new List<string>
                {
                    "Bartender",
                    "Tourist",
                    "Local Resident",
                    "DJ",
                    "Security Guard",
                    "Cleaner",
                    "Barista",
                    "Musician",
                    "Beach Vendor",
                    "Chef"
                }},
            {"Swimming Area", new List<string>
                {
                    "Lifeguard",
                    "Swimmer",
                    "Child",
                    "Parent with Child",
                    "Tourist",
                    "Local Resident",
                    "Photographer",
                    "Swimming Coach",
                    "Security Guard",
                    "Ice Water Vendor"
                }},
            {"Surfing Area", new List<string>
                {
                    "Surfer",
                    "Surf Instructor",
                    "Tourist",
                    "Local Resident",
                    "Photographer",
                    "Lifeguard",
                    "Surf Equipment Vendor",
                    "Bartender",
                    "Child",
                    "Surf Coach"
                }},
            {"Beach Volleyball Court", new List<string>
                {
                    "Volleyball Player",
                    "Coach",
                    "Tourist",
                    "Local Resident",
                    "Photographer",
                    "Lifeguard",
                    "Referee",
                    "Security Guard",
                    "Fan",
                    "Beverage Vendor"
                }},
            {"Umbrellas and Sunbeds", new List<string>
                {
                    "Tourist",
                    "Local Resident",
                    "Waiter",
                    "Beverage Vendor",
                    "Lifeguard",
                    "Photographer",
                    "Bartender",
                    "Child",
                    "Cleaner",
                    "Security Guard"
                }},
            {"Filming Set", new List<string>
                {
                    "Director",
                    "Actor",
                    "Cameraman",
                    "Makeup Artist",
                    "Screenwriter",
                    "Producer",
                    "Sound Engineer",
                    "Lighting Technician",
                    "Stuntman",
                    "Security Guard"
                }},
            {"Makeup Room", new List<string>
                {
                    "Makeup Artist",
                    "Actor",
                    "Costume Designer",
                    "Assistant Director",
                    "Photographer",
                    "Hairdresser",
                    "Massage Therapist",
                    "Security Guard",
                    "Cleaner",
                    "Producer"
                }},
            {"Director's Room", new List<string>
                {
                    "Director",
                    "Assistant Director",
                    "Screenwriter",
                    "Producer",
                    "Cameraman",
                    "Actor",
                    "Makeup Artist",
                    "Costume Designer",
                    "Lighting Technician",
                    "Security Guard"
                }},
            {"Camera Service Point", new List<string>
                {
                    "Cameraman",
                    "Technician",
                    "Assistant Director",
                    "Sound Engineer",
                    "Lighting Technician",
                    "Cleaner",
                    "Photographer",
                    "Producer",
                    "Security Guard",
                    "Screenwriter"
                }},
            {"Editing Room", new List<string>
                {
                    "Editor",
                    "Director",
                    "Assistant Director",
                    "Screenwriter",
                    "Producer",
                    "Sound Engineer",
                    "Photographer",
                    "Cleaner",
                    "Security Guard",
                    "Technician"
                }},
            {"Emergency Department", new List<string>
                {
                    "Doctor",
                    "Nurse",
                    "Patient",
                    "Administrator",
                    "Security Guard",
                    "Paramedic",
                    "Cleaner",
                    "Technician",
                    "Paramedic",
                    "Pharmacist"
                }},
            {"Operating Room", new List<string>
                {
                    "Surgeon",
                    "Anesthesiologist",
                    "Nurse",
                    "Patient",
                    "Surgical Assistant",
                    "Technician",
                    "Cleaner",
                    "Pharmacist",
                    "Security Guard",
                    "Lab Technician"
                }},
            {"Intensive Care Unit", new List<string>
                {
                    "Doctor",
                    "Nurse",
                    "Patient",
                    "Technician",
                    "Cleaner",
                    "Security Guard",
                    "Paramedic",
                    "Paramedic",
                    "Pharmacist",
                    "Lab Technician"
                }},
            {"Laboratory", new List<string>
                {
                    "Lab Technician",
                    "Doctor",
                    "Technician",
                    "Lab Assistant",
                    "Cleaner",
                    "Security Guard",
                    "Nurse",
                    "Paramedic",
                    "Paramedic",
                    "Pharmacist"
                }},
            {"Patient Room", new List<string>
                {
                    "Doctor",
                    "Nurse",
                    "Patient",
                    "Cleaner",
                    "Security Guard",
                    "Administrator",
                    "Paramedic",
                    "Paramedic",
                    "Pharmacist",
                    "Lab Technician"
                }},
            {"Command Center", new List<string>
                {
                    "Commander",
                    "Communication Officer",
                    "Analyst",
                    "Security Guard",
                    "Technician",
                    "Soldier",
                    "Administrator",
                    "Cleaner",
                    "Drone Operator",
                    "Cipher Specialist"
                }},
            {"Barracks", new List<string>
                {
                    "Soldier",
                    "Sergeant",
                    "Commander",
                    "Technician",
                    "Medic",
                    "Security Guard",
                    "Cook",
                    "Cleaner",
                    "Instructor",
                    "Paramedic"
                }},
            {"Training Ground", new List<string>
                {
                    "Soldier",
                    "Instructor",
                    "Commander",
                    "Technician",
                    "Medic",
                    "Security Guard",
                    "Paramedic",
                    "Cleaner",
                    "Cook",
                    "Observer"
                }},
            {"Shooting Range", new List<string>
                {
                    "Soldier",
                    "Instructor",
                    "Commander",
                    "Technician",
                    "Medic",
                    "Security Guard",
                    "Paramedic",
                    "Cleaner",
                    "Cook",
                    "Shooter"
                }},
            {"Vehicle Hangar", new List<string>
                {
                    "Mechanic",
                    "Technician",
                    "Soldier",
                    "Commander",
                    "Security Guard",
                    "Cleaner",
                    "Paramedic",
                    "Cook",
                    "Observer",
                    "Driver"
                }},
            {"Slot Machine Hall", new List<string>
                {
                    "Gambler",
                    "Casino Employee",
                    "Security Guard",
                    "Manager",
                    "Cleaner",
                    "Beverage Vendor",
                    "Musician",
                    "Photographer",
                    "Tourist",
                    "Dealer"
                }},
            {"Poker Table", new List<string>
                {
                    "Player",
                    "Dealer",
                    "Manager",
                    "Security Guard",
                    "Beverage Vendor",
                    "Cleaner",
                    "Tourist",
                    "Photographer",
                    "Musician",
                    "Gambler"
                }},
            {"Roulette", new List<string>
                {
                    "Player",
                    "Dealer",
                    "Security Guard",
                    "Manager",
                    "Cleaner",
                    "Beverage Vendor",
                    "Tourist",
                    "Musician",
                    "Photographer",
                    "Gambler"
                }},
            {"Bar", new List<string>
                {
                    "Bartender",
                    "Customer",
                    "Security Guard",
                    "Cleaner",
                    "Beverage Vendor",
                    "Musician",
                    "Photographer",
                    "Manager",
                    "Waiter",
                    "Tourist"
                }},
            {"VIP Room", new List<string>
                {
                    "VIP Guest",
                    "Host",
                    "Security Guard",
                    "Cleaner",
                    "Beverage Vendor",
                    "Manager",
                    "Musician",
                    "Photographer",
                    "Waiter",
                    "Tourist"
                }},
            {"Command Module", new List<string>
                {
                    "Commander",
                    "Astronaut",
                    "Technician",
                    "Mission Control Specialist",
                    "Security Guard",
                    "Cleaner",
                    "Engineer",
                    "Communications Specialist",
                    "Pilot",
                    "Navigator"
                }},
            {"Space Laboratory", new List<string>
                {
                    "Scientist",
                    "Astronaut",
                    "Technician",
                    "Engineer",
                    "Mission Control Specialist",
                    "Cleaner",
                    "Security Guard",
                    "Lab Assistant",
                    "Navigator",
                    "Pilot"
                }},
            {"Living Module", new List<string>
                {
                    "Astronaut",
                    "Technician",
                    "Mission Control Specialist",
                    "Cleaner",
                    "Security Guard",
                    "Engineer",
                    "Communications Specialist",
                    "Cook",
                    "Navigator",
                    "Pilot"
                }},
            {"Airlock", new List<string>
                {
                    "Astronaut",
                    "Mission Control Specialist",
                    "Engineer",
                    "Technician",
                    "Security Guard",
                    "Cleaner",
                    "Navigator",
                    "Pilot",
                    "Communications Specialist",
                    "Medical Officer"
                }},
            {"Space Gym", new List<string>
                {
                    "Astronaut",
                    "Trainer",
                    "Technician",
                    "Cleaner",
                    "Security Guard",
                    "Mission Control Specialist",
                    "Engineer",
                    "Communications Specialist",
                    "Navigator",
                    "Pilot"
                }},
            {"Captain's Bridge", new List<string>
                {
                    "Captain",
                    "Navigator",
                    "First Officer",
                    "Technician",
                    "Engineer",
                    "Security Guard",
                    "Cleaner",
                    "Cook",
                    "Helmsman",
                    "Officer"
                }},
            {"Torpedo Bay", new List<string>
                {
                    "Officer",
                    "Technician",
                    "Engineer",
                    "Security Guard",
                    "Cleaner",
                    "Navigator",
                    "Captain",
                    "Cook",
                    "First Officer",
                    "Gunner"
                }},
            {"Engine Room", new List<string>
                {
                    "Engineer",
                    "Technician",
                    "Security Guard",
                    "Cleaner",
                    "Captain",
                    "Navigator",
                    "Cook",
                    "First Officer",
                    "Gunner",
                    "Machinist"
                }},
            {"Living Quarters", new List<string>
                {
                    "Crew Member",
                    "Captain",
                    "Engineer",
                    "Technician",
                    "Security Guard",
                    "Cleaner",
                    "Cook",
                    "Navigator",
                    "First Officer",
                    "Gunner"
                }},
            {"Galley", new List<string>
                {
                    "Cook",
                    "Chef",
                    "Crew Member",
                    "Captain",
                    "Engineer",
                    "Technician",
                    "Security Guard",
                    "Cleaner",
                    "Navigator",
                    "First Officer"
                }},
            {"Pool Deck", new List<string>
                {
                    "Lifeguard",
                    "Tourist",
                    "Local Resident",
                    "Waiter",
                    "Pool Attendant",
                    "Security Guard",
                    "Cleaner",
                    "Bartender",
                    "Musician",
                    "Photographer"
                }},
            {"Restaurant", new List<string>
                {
                    "Chef",
                    "Waiter",
                    "Customer",
                    "Manager",
                    "Security Guard",
                    "Cleaner",
                    "Musician",
                    "Photographer",
                    "Beverage Vendor",
                    "Tourist"
                }},
            {"Nightclub", new List<string>
                {
                    "DJ",
                    "Bartender",
                    "Bouncer",
                    "Customer",
                    "Security Guard",
                    "Cleaner",
                    "Photographer",
                    "Waiter",
                    "Musician",
                    "Manager"
                }},
            {"Cabins", new List<string>
                {
                    "Guest",
                    "Housekeeper",
                    "Security Guard",
                    "Manager",
                    "Cleaner",
                    "Bartender",
                    "Musician",
                    "Photographer",
                    "Waiter",
                    "Tourist"
                }},
            {"Gym", new List<string>
                {
                    "Trainer",
                    "Guest",
                    "Housekeeper",
                    "Security Guard",
                    "Manager",
                    "Cleaner",
                    "Photographer",
                    "Waiter",
                    "Bartender",
                    "Tourist"
                }},
            {"Exhibition Hall", new List<string>
                {
                    "Visitor",
                    "Guide",
                    "Curator",
                    "Security Guard",
                    "Cleaner",
                    "Photographer",
                    "Ticket Seller",
                    "Tourist",
                    "Museum Employee",
                    "Manager"
                }},
            {"Ancient Artifacts Room", new List<string>
                {
                    "Curator",
                    "Guide",
                    "Security Guard",
                    "Visitor",
                    "Cleaner",
                    "Photographer",
                    "Ticket Seller",
                    "Tourist",
                    "Museum Employee",
                    "Manager"
                }},
            {"Interactive Exhibits Room", new List<string>
                {
                    "Visitor",
                    "Guide",
                    "Curator",
                    "Security Guard",
                    "Cleaner",
                    "Photographer",
                    "Ticket Seller",
                    "Tourist",
                    "Museum Employee",
                    "Manager"
                }},
            {"Souvenir Shop", new List<string>
                {
                    "Shop Assistant",
                    "Customer",
                    "Security Guard",
                    "Cleaner",
                    "Photographer",
                    "Tourist",
                    "Manager",
                    "Cashier",
                    "Guide",
                    "Museum Employee"
                }},
            {"Archive", new List<string>
                {
                    "Archivist",
                    "Researcher",
                    "Security Guard",
                    "Cleaner",
                    "Photographer",
                    "Visitor",
                    "Guide",
                    "Museum Employee",
                    "Manager",
                    "Archivist Assistant"
                }},
        };

        Random random = new Random();

        // Method for choosing random topic for the game
        public string chooseTopic()
        {
            string topic = topics[random.Next(topics.Count())];
            return topic;
        }

        // Method for choosing random location for the game
        public (string, List<string>) chooseLocation(string topic)
        {
            string location = locations[topic][random.Next(locations[topic].Count())];
            List<string> rolesForLocation = roles[location];
            return (location, rolesForLocation);
        }

        // Method for choosing random player's role for the game
        public (string, List<string>) chooseRole(List<string> rolesForLocation)
        {
            string role = rolesForLocation[random.Next(rolesForLocation.Count())];
            rolesForLocation.Remove(role);
            return (role, rolesForLocation);
        }
    }
}
