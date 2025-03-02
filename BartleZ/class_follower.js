let followerCount = 1;

class Follower{
    constructor(element,x,y,position){
        this.element = element;
        this.id = followerCount++;
        this.x = x;
        this.y = y;
        this.position = position;
        this.reloadSpeed = 2;
        this.initReloadTimer = 500;
        this.reloadTimer = this.initReloadTimer;
        this.followerDamage = 3;
    }

setReloadSpeed(nuValue){
        this.initReloadTimer = nuValue
        this.reloadTimer = this.initReloadTimer
    }

addFollower(nux,nuy){
    console.log("Made a Friend :D")
    const followerDiv = document.createElement('div');
    followerDiv.className = 'follower';
    followerDiv.style.left = nux + 'px';
    followerDiv.style.top = nuy + 'px';

    const fellower = new Follower(followerDiv,nux,nuy,120)

    const mapa = document.getElementById('mapa');
    mapa.appendChild(followerDiv);
    fellower.moveThisFollower()
}

initiatedFollower(nux, nuy, numFollowers = 5) {
    console.log("Made a Friend :D")
    
    for (let i = 0; i < numFollowers; i++) {
        // Gerar um deslocamento aleatório para cada seguidor
        const offsetX = Math.floor(Math.random() * 200) - 100;  // Se os quiser mais afastados é aqui 
        const offsetY = Math.floor(Math.random() * 200) - 100;  

        // Gerar o HTML do seguidor
        const followerDiv = document.createElement('div');
        followerDiv.className = 'follower';
        followerDiv.style.left = (nux + offsetX) + 'px';
        followerDiv.style.top = (nuy + offsetY) + 'px';

        // Criar o seguidor com as novas posições
        const follower = new Follower(followerDiv, nux + offsetX, nuy + offsetY, 120);

        const mapa = document.getElementById('mapa');
        mapa.appendChild(followerDiv);
        follower.moveThisFollower();
    }
}


moveThisFollower() {
    const followerTop = parseInt(this.element.style.top);
    const followerLeft = parseInt(this.element.style.left);
    const characterTop = parseInt(getComputedStyle(personagem).top);
    const characterLeft = parseInt(getComputedStyle(personagem).left);

    const deltaX = characterLeft - followerLeft;
    const deltaY = characterTop - followerTop;
    const distance = Math.sqrt(deltaX * deltaX + deltaY * deltaY);

    if (distance >= this.position) {
        this.moveFollowerTowardsCharacter();
    }

    // Calcular a rotação em relação ao zombi mais próximo
    this.rotateTowardsClosestZombie();

    if (this.canShoot()) {
        this.shoot(); // Garanta que o *follower* está sempre disparando
    }

    requestAnimationFrame(() => this.moveThisFollower());
}

// Função para mover o seguidor em direção ao jogador
moveFollowerTowardsCharacter() {
    const speed = returnPlayerSpeed() * 4 / 5;

    const followerTop = parseInt(this.element.style.top);
    const followerLeft = parseInt(this.element.style.left);

    const characterTop = parseInt(getComputedStyle(personagem).top);
    const characterLeft = parseInt(getComputedStyle(personagem).left);

    const deltaX = characterLeft - followerLeft;
    const deltaY = characterTop - followerTop;

    const angle = Math.atan2(deltaY, deltaX);

    let newFollowerLeft = followerLeft + speed * Math.cos(angle);
    let newFollowerTop = followerTop + speed * Math.sin(angle);

    const followers = document.querySelectorAll('.follower');
    const minDistance = 50; // Distância mínima 

    followers.forEach((otherFollower) => {
        if (otherFollower === this.element) return;

        const otherLeft = parseInt(otherFollower.style.left);
        const otherTop = parseInt(otherFollower.style.top);

        const distance = Math.sqrt(
            (newFollowerLeft - otherLeft) ** 2 + (newFollowerTop - otherTop) ** 2
        );

        if (distance < minDistance) {
            const avoidAngle = Math.atan2(newFollowerTop - otherTop, newFollowerLeft - otherLeft);
            newFollowerLeft += (minDistance - distance) * Math.cos(avoidAngle);
            newFollowerTop += (minDistance - distance) * Math.sin(avoidAngle);
        }
    });

    this.element.style.left = newFollowerLeft + 'px';
    this.element.style.top = newFollowerTop + 'px';
}

// Função para rotação baseada no zombi mais próximo
rotateTowardsClosestZombie() {
    const zombies = document.querySelectorAll('.zombie');
    if (zombies.length === 0) return;

    let closestZombie = null;
    let closestDistance = Infinity;

    const followerX = parseInt(this.element.style.left);
    const followerY = parseInt(this.element.style.top);

    // Encontrar o zombi mais próximo
    zombies.forEach((zombie) => {
        const zombieX = parseInt(zombie.style.left);
        const zombieY = parseInt(zombie.style.top);
        const distance = Math.sqrt(
            (zombieX - followerX) ** 2 + (zombieY - followerY) ** 2
        );

        if (distance < closestDistance) {
            closestZombie = zombie;
            closestDistance = distance;
        }
    });

    if (closestZombie) {
        const zombieX = parseInt(closestZombie.style.left);
        const zombieY = parseInt(closestZombie.style.top);

        const angle = Math.atan2(zombieY - followerY, zombieX - followerX);

        this.element.style.transform = `rotate(${angle}rad)`; // Rotaciona para o zombi mais próximo
    }
}



moveFollowerTowardsCharacter() {
    const pRotation = returnPlayerRotation();
    const speed = returnPlayerSpeed() * 4 / 5;

    const followerTop = parseInt(this.element.style.top);
    const followerLeft = parseInt(this.element.style.left);

    const characterTop = parseInt(getComputedStyle(personagem).top);
    const characterLeft = parseInt(getComputedStyle(personagem).left);

    const deltaX = characterLeft - followerLeft;
    const deltaY = characterTop - followerTop;

    const angle = Math.atan2(deltaY, deltaX);

    let newFollowerLeft = followerLeft + speed * Math.cos(angle);
    let newFollowerTop = followerTop + speed * Math.sin(angle);

    const followers = document.querySelectorAll('.follower');
    const minDistance = 50; // Distância mínima 

    followers.forEach((otherFollower) => {
        if (otherFollower === this.element) return;

        const otherLeft = parseInt(otherFollower.style.left);
        const otherTop = parseInt(otherFollower.style.top);

        const distance = Math.sqrt(
            (newFollowerLeft - otherLeft) ** 2 + (newFollowerTop - otherTop) ** 2
        );

        if (distance < minDistance) {
            const avoidAngle = Math.atan2(newFollowerTop - otherTop, newFollowerLeft - otherLeft);
            newFollowerLeft += (minDistance - distance) * Math.cos(avoidAngle);
            newFollowerTop += (minDistance - distance) * Math.sin(avoidAngle);
        }
    });

    this.element.style.left = newFollowerLeft + 'px';
    this.element.style.top = newFollowerTop + 'px';

    this.element.style.transform = `rotate(${pRotation}deg)`;
}



shoot() {
    const room = returnCurrentGameRoom(); 
    const zombies = document.querySelectorAll('.zombie'); // Liste todos os zombis

    if (zombies.length === 0) return; // Não há zombis

    let closestZombie = null;
    let closestDistance = Infinity;

    const followerX = parseInt(this.element.style.left);
    const followerY = parseInt(this.element.style.top);

    // Identifique o zombi mais próximo
    zombies.forEach((zombie) => {
        const zombieX = parseInt(zombie.style.left);
        const zombieY = parseInt(zombie.style.top);
        const distance = Math.sqrt(
            (zombieX - followerX) ** 2 + (zombieY - followerY) ** 2
        );

        if (distance < closestDistance) {
            closestZombie = zombie;
            closestDistance = distance;
        }
    });

    // Delay para disparar
    const delayTime = 500; // Delay de 1 segundo (1000ms)

    // Adicionar controle de disparo com flag
    if (!this.isShooting) {
        this.isShooting = true; // Marque que o disparo foi iniciado

        if (closestZombie) {
            setTimeout(() => {
                const zombieX = parseInt(closestZombie.style.left);
                const zombieY = parseInt(closestZombie.style.top);

                const angle = Math.atan2(zombieY - followerY, zombieX - followerX);

                // Criação e disparo da bala
                const bullet = createBullet(followerX, followerY);
                bullet.dx = Math.cos(angle);
                bullet.dy = Math.sin(angle);

                updateBullet(bullet, room, this.followerDamage);
                this.startShotTimer(); // Reinicia o tempo de recarga

                // Após disparar, definir novamente para permitir um novo disparo depois da recarga
                setTimeout(() => {
                    this.isShooting = false; // Liberar o próximo disparo
                }, this.initReloadTimer); // Definir o tempo de recarga baseado no reloadTimer
            }, delayTime); // Espera o tempo definido antes de disparar
        }
    }
}




canShoot (){
    return (this.reloadTimer === this.initReloadTimer ); //&& this.isFrozen === false
}

startShotTimer() {
    const interval = setInterval(() => {
        if (this.reloadTimer > 0) {
            this.reloadTimer -= this.reloadSpeed;
            if (this.reloadTimer <= 0) {
                clearInterval(interval);
                this.reloadTimer = this.initReloadTimer;
            }
        }
    }, 1000 / 60);
    return this.reloadTimer;
    }
}