let lastRoomId = 0
class Room {
    constructor(name, description, reward) {
        this.name = name;
        this.description = description;
        this.cleared = false;
        this.reward = reward;
        this.doors = {}; // Object to store connections to other rooms
        this.zombies = []; // Array to store zombies in the room
        this.blackScreen = document.getElementById('blackScreen');
        this.anwseredQuestion = false;
        this.id = lastRoomId++;
        this.time_room = 0;
        this.startTime = null; // Start time when question is created
        this.chestState = { open: false }; // Estado inicial do baú

    }

    getGameMap() {
        for (const mapRoom of Object.values(gameMap.rooms)) {
            if (this === mapRoom) {
                return gameMap;
            }
        }
        return null;
    }

    updateChestForCurrentRoom() {
        const chest = document.getElementById('chest');
        const chestMessage = document.getElementById('chestMessage');
        const currentRoom = gameMap.getCurrentRoom();
    
        if (!currentRoom.chestState) {
            currentRoom.chestState = { open: false };
        }
    
        console.log(`Atualizando o estado do baú na sala: ${currentRoom.name}. Estado: ${currentRoom.chestState.open ? 'Aberto' : 'Fechado'}`);
        
        if (currentRoom.chestState.open) {
            chest.classList.remove('closed');
            chest.classList.add('open');
            chestMessage.classList.add('hidden');
        } else {
            chest.classList.remove('open');
            chest.classList.add('closed');
            chestMessage.classList.remove('hidden');
        }
    }

    startDate(){
        this.startTime = new Date()
    }

    addDoor(direction, room) {
        this.doors[direction] = room;
    }

    addZombie(zombie) {
        this.zombies.push(zombie);
    }

    removeZombie(zombie) {
        const index = this.zombies.indexOf(zombie);
        if (index !== -1) {
            this.zombies.splice(index, 1);
            if (this.zombies.length === 0) {
                this.cleared = true;
                var map = this.getGameMap()
                map.everythingClear()
            }
        }
    }

    setAnwser(value){
        this.anwseredQuestion = value
    }

    isClear() {
        return this.cleared;
    }

    getDoor(direction) {
        return this.doors[direction];
    }

    async enterDoor(player) {
        if (this.enteringDoor) return; // Exit early if already entering a door
        const doorId = this.findDoorCollision(player);
        if (!doorId) return; // No door found, exit early
    
        const direction = doorId.split('_')[1]; // Extract direction from door id
        if (!this.isValidDirection(direction, player)) return; // Invalid direction, exit early
            const endTime = new Date();
            this.time_room = (endTime - this.startTime) / 1000; // Calculate time spent in seconds
            logAddRoomT(this.time_room)
            console.log(`Time spent on room: ${this.time_room} seconds`);

        console.log(`Player entered door ${direction} in room ${this.name}`);

        if (this.isClear()) {
            player.stopLevelTimer();
            player.resetLevelStats(); // Reset status do jogador para o próximo nível
        }
            
        this.enteringDoor = true; // Set enteringDoor flag to true
    
        this.showBlackScreen();
        player.freeze();
        const nextRoom = this.doors[direction];
        if (nextRoom) {
            switch(direction){
                case "up":
                    player.element.style.left = '940px';
                    player.element.style.top = '870px';
                break;
                case "down":
                    player.element.style.left = '940px';
                    player.element.style.top = '110px';
                break;
                case "left":
                    player.element.style.left = '1710px';
                    player.element.style.top = '480px';
                break;
                case "right":
                    player.element.style.left = '110px';
                    player.element.style.top = '480px';
                break;
            }
    
        try {
            if(nextRoom.anwseredQuestion === false){
                await this.showQuestionAndWait(nextRoom.id,player);
                nextRoom.setAnwser(true)
                }
        } catch (error) {
            console.error('Error while showing question:', error);
        } finally {
            gameMap.setCurrentRoom(nextRoom);
            nextRoom.updateRoomDisplay();
            this.hideBlackScreen();
            player.unfreeze();
            this.removeDoors();
            this.removeBullets();
            this.enteringDoor = false;
            nextRoom.startDate();
            if(nextRoom.isClear()){
                nextRoom.openDoors();
            }else{
                this.startTime = new Date();
                initZombie(gameMap,600)
            }
        }
    }
    }

    async showQuestionAndWait(id,player) {
        var questionText, questionOptions, rewardArray
        var specialRewardEventFlag = 0
        switch (id){
            case 1:
                questionText = 'UPGRADE\nChoose which one you would rather have?'
                questionOptions = ['A weapon that shoots faster.','More allies']
                specialRewardEventFlag = 1
                rewardArray = [[0,0,10,-10],[0,0,-5,10]]
                //Cada array define, em ordem, quanto vai alterar em: [Achiever, Explorer, Killer, Socializer] e em ordem da pergunta 
            break;
            case 2:
                questionText = 'You find a mysterious block in a game. What is your first thought?'
                questionOptions = ['What importance does it have for the story',"I wonder if it's an easter egg",'Hit it to see if it gives points']
                rewardArray = [[0,10,0,-5],[10,0,-5,0],[0,0,10,-5]]
//HIT1

            break;
            case 3:
                questionText = 'The winner in a game is the one who:'
                questionOptions = ['Has the highest score','Had the best Kill/Death ratio','Had the most fun with their friends']
                rewardArray = [[10,-5,0,0],[0,10,0,-5],[0,0,-5,10]]
            break;
            case 4:
                questionText = "It's the release cycle for a new update in your favorite game, you must go check the:"
                questionOptions = ['New story events','What builds will be good','The discussions online']
                rewardArray = [[0,10,-5,0],[0,-5,10,0],[-5,0,0,10]]
            break;
            case 5:
                questionText = 'What would you rather gain as a reward from exploring?'
                questionOptions = ['Friends','Items','Knowing is its own reward']
                rewardArray = [[-5,0,0,10],[0,0,10,-5],[0,10,-5,0]]
//HIT2


            break;
            case 6: //Não utilizada nos testes.
                questionText = 'One of your friends tells you to come with him to check something he found. What is your first thought?'
                questionOptions = ['He must be luring me into a trap to steal my items','He must have found a cool secret','I just want to spend time with my friends']
                rewardArray = [[0,0,10,-5],[0,10,-5,0],[0,0,-5,10]]
            break;

//HIT3
        }
        const questionSpawner = new Question(
            questionText,
            questionOptions,
            (selectedOptionIndex) => {
                player.editValues(rewardArray[selectedOptionIndex])
                if(specialRewardEventFlag > 0){
                    player.specialEventChanger(specialRewardEventFlag,selectedOptionIndex)
                }
            }
        );
        await questionSpawner.createQuestion();
    }

    isValidDirection(direction, player) {
        if (player.dy < 0 && direction === 'up') {
            return true;
        }
        if (player.dy > 0 && direction === 'down') {
            return true;
        }
        if (player.dx < 0 && direction === 'left') {
            return true;
        }
        if (player.dx > 0 && direction === 'right') {
            return true;
        }
        return false;
    }

    openDoors() {
        const doorWidth = 140;
        const doorHeight = 140; // Talvez tenha que ajustar um pouco mais
        const mapa = document.getElementById('mapa');
        const leftPosition = (mapa.offsetWidth / 2) - (doorWidth / 2); // Center horizontally
    
        // Verifica se os quartos adjacentes estão limpos e altera a imagem da porta de acordo
        for (const [direction, adjacentRoom] of Object.entries(this.doors)) {
            const doorElement = document.createElement('div');
            doorElement.className = 'door';
            doorElement.id = `door_${direction}`;
    
            if (adjacentRoom.isClear()) {
                doorElement.style.backgroundImage = "url('porta_aberta.png')";
            } else {
                doorElement.style.backgroundImage = "url('porta_antiga.png')";
            }
    
            doorElement.style.width = `${doorWidth}px`;
            doorElement.style.height = `${doorHeight}px`;
    
            switch (direction) {
                case 'up':
                    doorElement.style.transform = 'rotate(-90deg)';
                    doorElement.style.top = '0px';
                    doorElement.style.left = `${leftPosition}px`;
                    break;
                case 'down':
                    doorElement.style.transform = 'rotate(90deg)';
                    doorElement.style.bottom = '0px';
                    doorElement.style.left = `${leftPosition}px`;
                    break;
                case 'left':
                    doorElement.style.transform = 'rotate(180deg)';
                    doorElement.style.top = `${(mapa.offsetHeight / 2) - (doorHeight / 2)}px`;
                    doorElement.style.left = '0px';
                    break;
                case 'right':
                    doorElement.style.transform = 'rotate(0deg)';
                    doorElement.style.top = `${(mapa.offsetHeight / 2) - (doorHeight / 2)}px`;
                    doorElement.style.right = '0px';
                    break;
            }
    
            mapa.appendChild(doorElement);
        }
    }
    

    findDoorCollision(player) {
        const playerRect = player.element.getBoundingClientRect();
        const doors = document.getElementsByClassName('door');

        for (const door of doors) {
            const doorRect = door.getBoundingClientRect();
            if (
                playerRect.top < doorRect.bottom &&
                playerRect.bottom > doorRect.top &&
                playerRect.left < doorRect.right &&
                playerRect.right > doorRect.left
            ) {
                return door.id;
            }
        }

        return null;
    }

    showBlackScreen() {
        this.blackScreen.classList.remove('hidden');
        setTimeout(() => {
            this.blackScreen.style.opacity = '1';
        }, 25);
    }

    hideBlackScreen() {
        setTimeout(() => {
            this.blackScreen.classList.add('hidden');
            this.blackScreen.style.opacity = '0';
        }, 550); 
    }
    

    removeDoors() {
        const doorElements = document.querySelectorAll('.door');
        doorElements.forEach((doorElement) => {
            doorElement.remove();
        });
    }

    removeBullets() {
        const bulletElements = document.querySelectorAll('#bullet');
        bulletElements.forEach((bulletElement) => {
            bulletElement.remove();
        });
    }

    updateRoomDisplay() {
        const currentRoomDisplay = document.getElementById('currentRoomDisplay');
        if (currentRoomDisplay) {
            currentRoomDisplay.textContent = `Current Room: ${this.id + 1}`; // Assumindo que `id` é o número da sala
        }
    }
}