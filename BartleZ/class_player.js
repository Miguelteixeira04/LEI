class Player {
    constructor(element, playerID) {
        this.element = element;
        this.playerID = playerID; 
        this.dx = 0;
        this.dy = 0;
        this.speed = 5;
        this.health = 100;
        this.healthBar = null;
        this.reloadSpeed = 2;
        this.initReloadTimer = 40;
        this.deaths = 0;
        this.reloadTimer = this.initReloadTimer;
        this.invulnTime;
        this.tiro = null;
        this.score = 0;
        this.direction = 0;
        this.createHealthBar();
        this.isFrozen = false;
        this.pointsAchiever = 25;
        this.pointsExplorer = 25;
        this.pointsKiller = 25;
        this.pointsSocializer = 25;
        this.weaponDamage = 5;
        this.levelTimer = 0;
        this.totalTime = 0;
        this.timeInterval = null;
        this.timeDisplay = document.getElementById('timeDisplay');
        this.deathCountDisplay = document.getElementById('deathCount');
        
        // Variáveis novas
        this.zombiesKilled = 0;
        this.levelCompletionTime = 0;
        this.completedLevelWithoutDying = true;
        this.completedGameWithoutDying = true;

        this.levelCount = 1; 
        this.chestsOpened = 0;
    }

    initialize(){
        // Método inicializador (pode ser usado para configurar estados iniciais, se necessário)
    }

    mainPers(){
        const points = {
            Achiever: this.pointsAchiever,
            Explorer: this.pointsExplorer,
            Killer: this.pointsKiller,
            Socializer: this.pointsSocializer,
        };

        const maxPoints = Math.max(...Object.values(points));
        const dominantRoles = [];

        for (const role in points) {
            if (points[role] === maxPoints) {
                dominantRoles.push(role);
            }
        }

        return dominantRoles.length === 1 ? dominantRoles[0] : dominantRoles;
    }

    arrayPers(){
        var arrayPoints = [this.pointsAchiever,this.pointsExplorer,this.pointsKiller,this.pointsSocializer]
        return arrayPoints;
    }

    getDeaths(){
        return this.deaths;
    }

    getScore(){
        return this.score;
    }

    createHealthBar() {
        const healthBar = document.createElement('div');
        healthBar.id = 'playerHealthBar';
        document.getElementById('mapa').appendChild(healthBar);
    
        const layers = ['Background', 'Middle', 'Top'];
        layers.forEach(layer => {
            const layerDiv = document.createElement('div');
            layerDiv.classList.add('healthBarLayer');
            layerDiv.id = `playerHealthBar${layer}`;
            healthBar.appendChild(layerDiv);
        });
    
        this.healthBar = healthBar;
    }

    updateLife(newHealth){
        this.health = newHealth;
    }

    freeze(){
        this.isFrozen = true;
    }

    unfreeze(){
        this.isFrozen = false;
    }

    moveCharacter() {
        let top = parseInt(getComputedStyle(this.element).top);
        let left = parseInt(getComputedStyle(this.element).left);
    
        top += this.dy;
        left += this.dx;
    
        // altera os limites de movimento
        if (backgroundChanged) {
            const marginTop = 40;
            const marginLeft = 40;
            const marginRight = 0;
            const marginBottom = 0;
            const minX = marginLeft;
            const maxX = mapa.clientWidth - marginRight - 150;
            const minY = marginTop;
            const maxY = mapa.clientHeight - marginBottom - 150;
    
            if (top >= minY && top <= maxY && left >= minX && left <= maxX) {
                player.element.style.top = top + 'px';
                player.element.style.left = left + 'px';
            }
        } else {
            // Aplica os limites de movimento padrão
            const marginTop = 110;
            const marginLeft = 110;
            const marginRight = 60;
            const marginBottom = 60;
            const minX = marginLeft;
            const maxX = mapa.clientWidth - marginRight - 150;
            const minY = marginTop;
            const maxY = mapa.clientHeight - marginBottom - 150;
    
            if (top >= minY && top <= maxY && left >= minX && left <= maxX) {
                player.element.style.top = top + 'px';
                player.element.style.left = left + 'px';
            }
        }
    }

    shoot(rotation, room) {
        this.startShotTimer(this.reloadTimer, this.reloadSpeed); // Start the shot timer

        if (!this.canShoot()) return;

        const playerPositionX = parseInt(getComputedStyle(personagem).left);
        const playerPositionY = parseInt(getComputedStyle(personagem).top);
    
        const bullet = createBullet(playerPositionX, playerPositionY);
    
        bullet.dx = Math.cos(rotation);
        bullet.dy = Math.sin(rotation);
    
        updateBullet(bullet, room, this.weaponDamage);
    }

    getPlayerRotation(){
        return this.rotation;
    }

    getPlayerSpeed(){
        return this.speed;
    }

    setPlayerRotation(nuValue){
        this.rotation = nuValue;
    }

    setPlayerDamage(nuValue){
        this.weaponDamage = nuValue;
    }

    canShoot (){
        return (this.reloadTimer === this.initReloadTimer && !this.isFrozen);
    }

    startShotTimer() {
        if (!this.canShoot()) return; // Prevent multiple intervals - could be melhor

        this.isReloading = true;
        this.reloadTimer = this.initReloadTimer;

        const interval = setInterval(() => {
            if (this.reloadTimer > 0) {
                this.reloadTimer -= this.reloadSpeed;
                if (this.reloadTimer <= 0) {
                    clearInterval(interval);
                    this.reloadTimer = this.initReloadTimer;
                    this.isReloading = false; // Reset the flag
                }
            }
        }, 1000 / 60); // Run at approximately 60fps
    }

    startInvulTimer() {
        this.invulnTime = 10; // Set initial invulnerability time
        const intervalId = setInterval(() => {
            if(this.invulnTime > 0){
                this.invulnTime--; // Decrement the remaining time
                if (this.invulnTime <= 0) {
                    clearInterval(intervalId); // Stop the timer when invulnerability time is over
                    this.invulnTime = 0;
                  }
            }
        }, 1000/60); // Tick down every second (adjust as needed)
        return this.reloadTimer;
    }

    isInvuln() {
        return this.invulnTime > 0; // Return true if invulnerability time is still remaining
    }

    editValues(array){
        this.pointsAchiever = this.pointsAchiever + array[0]
        this.pointsExplorer = this.pointsExplorer + array[1]
        this.pointsKiller = this.pointsKiller + array[2]
        this.pointsSocializer = this.pointsSocializer + array[3]
        console.log("Achiever: " + this.pointsAchiever + "\nExplorer: "+ this.pointsExplorer + "\nKiller: "+ this.pointsKiller + "\nSocializer: "+this.pointsSocializer)
    }

    specialEventChanger(eventID, option){
        const playerPositionX = parseInt(getComputedStyle(personagem).left);
        const playerPositionY = parseInt(getComputedStyle(personagem).top);
        switch (eventID){
            case 1:
                if(option === 0){this.reloadSpeed = this.reloadSpeed + 1}
                else {
                    const fellower = new Follower();
                    fellower.addFollower(playerPositionX,playerPositionY);
                }
            break;
        }
    }

    startLevelTimer() {
        console.log('Timer iniciado'); // Adicionado para depuração
        this.levelTimer = 0;
        this.timerInterval = setInterval(() => {
            this.levelTimer++;
            const minutes = Math.floor(this.levelTimer / 60);
            const seconds = this.levelTimer % 60;
            const formattedTime = `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
            console.log(`Tempo: ${formattedTime}`); // Adicionado para depuração
            this.timeDisplay.innerText = `Time: ${formattedTime}`; // Atualiza o marcador de tempo
        }, 1000); // Atualiza a cada segundo
    }

    stopLevelTimer() {
        clearInterval(this.timerInterval);
        this.totalTime += this.levelTimer;
        this.levelCompletionTime = this.levelTimer;

        console.log(`🏆 Nível ${this.levelCount} concluído!`);
        console.log(`Morreu neste nível? ${!this.completedLevelWithoutDying}`);

        if (this.completedLevelWithoutDying) {
            let badge = document.getElementById('badge11');
            if (badge && badge.src.includes("Locked.jpg")) {
                window.unlockBadge('badge11', 'https://i.ibb.co/kmp0KvR/Survivor.jpg');
                console.log('✅ Badge 11 desbloqueado!');
            }
        }

        // 🔥 Aumenta o contador de níveis
        this.levelCount++;

        console.log(`📢 Agora está no nível ${this.levelCount}`);

        // Verifica se o jogo foi completado (nível 9)
        if (this.levelCount > 9) {
            this.checkGameCompletion();
            console.log(`entrou`);
        } else {
            this.resetLevelStats();
            console.log(`deu reset`);
        }
    }
    
    
    resetLevelStats() {
        this.levelTimer = 0;
        this.completedLevelWithoutDying = true;
    }

    incrementDeaths() {
        this.deaths++;
        this.completedLevelWithoutDying = false; // Player died in this level
        this.completedGameWithoutDying = false; // Player died during the game
        console.log(`Deaths incremented: ${this.deaths}`); 
        this.deathCountDisplay.innerText = `Deaths: ${this.deaths}`; 
    }

    openChest() {
        const currentRoom = gameMap.getCurrentRoom();
        if (!currentRoom.chestState.open) {
            currentRoom.chestState.open = true;
            this.chestsOpened++;
        }
    }
    
    incrementZombiesKilled(){
        this.zombiesKilled++;
    }

    checkGameCompletion() {

        console.log(`🎉 Jogo Concluído!`);
        console.log(`Total Time: ${this.totalTime} seconds`);
        console.log(`Zombies Killed: ${this.zombiesKilled}`);
        console.log(`Deaths: ${this.deaths}`);
        console.log(`Chests Opened: ${this.chestsOpened}`);
        console.log(`Completou sem morrer? ${this.completedGameWithoutDying}`);

        if (this.completedGameWithoutDying) {
            console.log('🏆 Desbloqueando Badge 12...');

            if (typeof window.unlockBadge === "function") {  
                window.unlockBadge('badge12', 'https://i.ibb.co/X8PR9zF/NoDeaths.jpg');
            } else {
                console.error("❌ Erro crítico: unlockBadge ainda não está acessível!");
            }
        
        }
    }


}