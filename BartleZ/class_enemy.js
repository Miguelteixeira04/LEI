class Enemy {
  constructor(name, x, y, health) {
    this.name = name;
    this.x = x;
    this.y = y;
    this.health = health;
    this.spawnZombie();
    this.createHealthBar();
  }

  spawnZombie() {
    const zombieDiv = document.createElement('div');
    zombieDiv.className = 'zombie';
    zombieDiv.dataset.health = this.health;
    zombieDiv.dataset.maxhealth = this.health;
    zombieDiv.style.left = this.x + 'px';
    zombieDiv.style.top = this.y + 'px';

    if (this.health == 300) {
      zombieDiv.style.width = '200px'; 
      zombieDiv.style.height = '200px'; 
    }

    const mapa = document.getElementById('mapa');
    mapa.appendChild(zombieDiv);
    this.element = zombieDiv;
    this.element.enemyInstance = this; // Assign Enemy instance to the element

    console.log(`Spawned ${this.name} at (${this.x}, ${this.y}) with health ${this.health}`);
  }

  createHealthBar() {
    const healthBar = document.createElement('div');
    healthBar.className = 'zombieHealth';
    healthBar.style.width = '80px';
    healthBar.style.height = '5px';
    healthBar.style.backgroundColor = 'green';
    healthBar.style.position = 'absolute';

    // Append health bar to the zombie element
    this.element.appendChild(healthBar);

    // Adjust position of health bar relative to the zombie element
    healthBar.style.left = '40px';
    healthBar.style.top = '-10px';

    this.healthBar = healthBar;
  }
}

function initZombie(gameMap) {
  const currentRoom = gameMap.getCurrentRoom();

  const zombie1 = new Enemy('zombie', 100, 100, 100);
  const zombie2 = new Enemy('zombie', 100, 860, 100);
  const zombie3 = new Enemy('zombie', 1700, 100, 100);

  currentRoom.addZombie(zombie1); // Add the zombies to the current room
  currentRoom.addZombie(zombie2);
  currentRoom.addZombie(zombie3);

  // Add Final Boss only if the current room is room 9
  if (currentRoom.id == 8) {
    const zombie4 = new Enemy('zombie', 1700, 860, 300);
    currentRoom.addZombie(zombie4);
  } else {
    const zombiesToRemove = currentRoom.zombies.filter(zombie => zombie.health == 300);
    zombiesToRemove.forEach(zombie => {
      if (zombie.element && zombie.element.parentNode) {
        zombie.element.parentNode.removeChild(zombie.element);
      }
    });
    currentRoom.zombies = currentRoom.zombies.filter(zombie => zombie.health != 300);
  }

  const zombies = currentRoom.zombies; // Get the zombies from the current room
  moveAllZombies(zombies, currentRoom); // Pass the current room to moveAllZombies
}

function moveAllZombies(zombies) {
  zombies.forEach((zombie) => {
    if (zombie instanceof Enemy) { // Check if zombie is an instance of Enemy
      let speed = 2;
      if (zombie.health == 300) {
        speed = 1; // Half the speed for zombie4
      }
      moveZombieTowardsCharacter(zombie.element, speed);
    }
  });

  updatePlayerHealth(zombies);
  requestAnimationFrame(() => moveAllZombies(zombies));
}

function moveZombieTowardsCharacter(zombie, speed) {
  const zombieTop = parseInt(zombie.style.top);
  const zombieLeft = parseInt(zombie.style.left);
  const characterTop = parseInt(getComputedStyle(personagem).top);
  const characterLeft = parseInt(getComputedStyle(personagem).left);

  const deltaX = characterLeft - zombieLeft;
  const deltaY = characterTop - zombieTop;

  const angle = Math.atan2(deltaY, deltaX);

  const newZombieLeft = zombieLeft + speed * Math.cos(angle);
  const newZombieTop = zombieTop + speed * Math.sin(angle);

  zombie.style.left = newZombieLeft + 'px';
  zombie.style.top = newZombieTop + 'px';

  const zombieRotation = getRotation(speed * Math.cos(angle), speed * Math.sin(angle));
  zombie.style.transform = `rotate(${zombieRotation}deg)`;
}

function updateHealthBar(zombie) {
  const life = parseInt(zombie.getAttribute('data-health'));
  const maxhealth = parseInt(zombie.getAttribute('data-maxhealth'));
  const healthBar = zombie.querySelector('.zombieHealth');

  if (healthBar) { // Check if healthBar exists before updating
    if (life > 0) {
      const newWidth = (life / maxhealth) * 80;
      healthBar.style.width = newWidth + 'px';

      if (life <= (life / maxhealth * 20)) {
        healthBar.style.backgroundColor = 'red';
      } else {
        healthBar.style.backgroundColor = 'green';
      }
    } else {
      healthBar.style.display = 'none';
      zombie.health = null;
    }
  }
}

function updatePlayerHealth(zombies) {
  const playerTop = parseInt(getComputedStyle(personagem).top);
  const playerLeft = parseInt(getComputedStyle(personagem).left);
  let playerLife = parseInt(player.health);

  zombies.forEach(zombie => {
    // Check if the zombie is an instance of the Enemy class
    if (zombie instanceof Enemy) {
      // Get the DOM element of the zombie
      const zombieElement = zombie.element;

      // Check if the zombie element is valid
      if (zombieElement instanceof Element) {
        const zombieTop = parseInt(getComputedStyle(zombieElement).top);
        const zombieLeft = parseInt(getComputedStyle(zombieElement).left);

        // Check if the zombie is near the player and update player's health
        if (Math.abs(playerTop - zombieTop) < 50 && Math.abs(playerLeft - zombieLeft) < 50 && !player.isInvuln()) {
          if (playerLife > 0) {
            playerLife -= 1;
            player.startInvulTimer();
          }
          player.updateLife(playerLife);
          personagem.setAttribute('health', playerLife);
          const healthBar = document.getElementById('playerHealthBarMiddle');
          updatePlayerHealthBar(personagem, healthBar);
        }
      }
    }
  });
}

function updatePlayerHealthBar(playerElement, healthBar) {
  const life = parseInt(playerElement.getAttribute('health'));
  const newWidth = (life / 100) * 600; // Supposing 50px is the total width of the health bar
  healthBar.style.width = newWidth + 'px';

  if (life <= 20) { // Supposing 20 is the critical life limit
    healthBar.style.backgroundColor = 'red';
    if (life < 1) {
      if (player.score > 0) {
        player.deaths += 1;
        player.score = player.score * 0.5;
        document.getElementById('scoreboard').textContent = "Score: " + player.score;
      }
      player.updateLife(100);
      const newWidth = (life / 100) * 600; // Supposing 50px is the total width of the health bar
      healthBar.style.width = newWidth + 'px';
    }
  } else {
    healthBar.style.backgroundColor = '#4CFF00';
  }
}

function getEnemyFromElement(element) {
  if (element && element.enemyInstance instanceof Enemy) {
    return element.enemyInstance;
  }
  return null;
}