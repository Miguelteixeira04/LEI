class dataHold {
    constructor() {
        this.total_time = []; // Initialize as an empty array
        this.room_time = []; // Initialize as an empty array
        this.choice_time = []; // Initialize as an empty array
    }

    setTotal(nuValue) {
        this.total_time.push(nuValue); // Add nuValue to the total_time array
    }

    setRoom(nuValue) {
        this.room_time.push(nuValue); // Add nuValue to the room_time array
    }

    setChoice(nuValue) {
        this.choice_time.push(nuValue); // Add nuValue to the choice_time array
    }

    getTotalT() {
        return this.total_time; // Return the total_time array
    }

    getRoomT() {
        return this.room_time; // Return the room_time array
    }

    getChoiceT() {
        return this.choice_time; // Return the choice_time array
    }
}

function logAddTotalT(nuValue){
    record.setTotal(nuValue)
}

function logAddRoomT(nuValue){
    record.setRoom(nuValue)
}

function logAddChoiceT(nuValue){
    record.setChoice(nuValue)
}

function logPlayerData() {
    const data = {
        player_Dom: player.mainPers(),
        percents: player.arrayPers(),
        total_time: record.getTotalT(),
        room_time: record.getRoomT(),
        choice_time: record.getChoiceT(),
        deaths: player.getDeaths(),
        score: player.getScore()
    };

    const jsonData = JSON.stringify(data);
    const blob = new Blob([jsonData], { type: 'application/json' });
    const url = URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.href = url;
    a.download = 'playerData.json';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);

    URL.revokeObjectURL(url); // Clean up

    console.log('Data downloaded as playerData.json');
}
