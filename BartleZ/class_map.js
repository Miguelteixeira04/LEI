class GameMap {
    constructor(mapaElement) {
        this.mapaElement = mapaElement;
        this.rooms = {};
        this.currentRoom = null; // Initialize currentRoom property
        this.time_map = 0; // Time spent on question
        this.timeSpentByRoom = {};
        this.startTime = null; // Start time when question is created
    }

    addRoom(name, description) {
        const room = new Room(name, description);
        this.rooms[name] = room;
        return room;
    }

    connectRooms(room1, direction1, room2, direction2) {
        room1.addDoor(direction1, room2);
        room2.addDoor(direction2, room1);
    }

    getRoom(name) {
        return this.rooms[name];
    }

    setCurrentRoom(room) {
        this.currentRoom = room;
    }

    getCurrentRoom() {
        return this.currentRoom;
    }

    everythingClear(){
        for (const roomName in this.rooms) {
            if (!this.rooms.hasOwnProperty(roomName)) continue; // Skip inherited properties
            const room = this.rooms[roomName];
            if (!room.isClear()) {
                console.log("Rooms yet to be cleared.")
                return false; // If any room is not cleared, return false immediately
            }
        }
        console.log("All rooms cleared.")
        this.calculateTimeSpent()

        console.log("Time Spent by room: ");
        for(const [roomName, timeSpent] of Object.entries(this.timeSpentByRoom)) {
            console.log(`- Room '${roomName}': ${timeSpent.toFixed(2)} seconds`);
        }

        

        return true; // All rooms are cleared
    }

    calculateTimeSpent() {
        const endTime = Date.now();
        this.time_map = endTime - this.startTime;
        console.log(`Time spent on game: ${this.time_map} milliseconds`);
        logAddTotalT(this.time_map);
        logPlayerData();
    }

    setCurrentRoom(room) {
        if (this.currentRoom) {
            // Registra o tempo gasto na sala anterior
            const endTime = Date.now();
            const roomName = this.currentRoom.name;
            const timeSpent = (endTime - this.startTime) / 1000; // Tempo em segundos
            this.timeSpentByRoom[roomName] = timeSpent;
            console.log(`Time spent in room '${roomName}': ${timeSpent.toFixed(2)} seconds`);
        }
    
        // Define a nova sala e reinicia o cronômetro
        this.currentRoom = room;
        this.startTime = Date.now();
    }
    
}

// Function to update the displayed room name
function updateRoomName(room) {
    const roomNameElement = document.getElementById('roomName');
    roomNameElement.textContent = room.name; // Set the text content to the current room's name
}