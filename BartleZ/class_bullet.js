class Bullet {
    constructor(x, y, dx, dy) {
        this.x = x;
        this.y = y;
        this.dx = dx;
        this.dy = dy;
        this.element = null;
        this.speed = 5; // Ajustar
    }
}

function createBullet(x, y) {
    const bullet = new Bullet(x + 50, y + 50, 0, 0);
    const bulletElement = document.createElement("div");
    bulletElement.id = "bullet";
    bulletElement.style.position = "absolute";
    bulletElement.style.width = "20px";
    bulletElement.style.height = "20px";
    bulletElement.style.background = "url('bala2.png')";
    bulletElement.style.backgroundSize = "cover";
    bulletElement.style.left = x + "px";

    bulletElement.style.top = y + "px";
    document.getElementById("mapa").appendChild(bulletElement);
    bullet.element = bulletElement;
    return bullet;
}

function updateBullet(bullet,room,damage) {
    const bulletElement = bullet.element;

    bullet.x += bullet.dx * bullet.speed;
    bullet.y += bullet.dy * bullet.speed;

    bulletElement.style.left = bullet.x + "px";
    bulletElement.style.top = bullet.y + "px";

    if (hitWall(bullet.x, bullet.y)) {
        bulletElement.remove();
        return;
    }else if (zombie = hitEnemy(bullet.x, bullet.y)){
        bulletElement.remove();
        damageTarget(zombie,room,damage)
        return;
    }

    requestAnimationFrame(() => updateBullet(bullet,room,damage));
}

function hitWall(left, top) {
    const marginTop = 100; // Reduzindo a área superior
    const marginLeft = 100; // Reduzindo a área esquerda
    const marginRight = 1; // Aumentando a área direita
    const marginBottom = 0; // Aumentando a área inferior

    const minX = marginLeft;
    const maxX = mapa.clientWidth - marginRight - 150; // 150 é a largura do personagem
    const minY = marginTop;
    const maxY = mapa.clientHeight - marginBottom - 150; // 150 é a altura do personagem

    return !(left >= minX && left <= maxX && top >= minY && top <= maxY);
}

function hitEnemy(left, top) {
    const margin = 0;
    const zombies = document.querySelectorAll('.zombie');

    left = left - margin;
    top = top - margin;


    for (let i = 0; i < zombies.length; i++) {
        const zombie = zombies[i];
        const zombieTop = parseInt(getComputedStyle(zombie).top);
        const zombieLeft = parseInt(getComputedStyle(zombie).left);
        if (
            top + 20 >= zombieTop &&
            top <= zombieTop + 90 &&
            left + 20 >= zombieLeft &&
            left <= zombieLeft + 90
        ) {
            // Bullet hit the zombie
            return zombie; // Zombie hit
        }
    }
    return false; // No zombie hit
}

function damageTarget(targetElement, currentRoom, damage) {
    const zombieLife = parseInt(targetElement.getAttribute('data-health'));
    const newLife = zombieLife - Math.round(damage * 20); //normal == 20 tiros. *10 2 tiros de vida. Meter damage * 2
    targetElement.setAttribute('data-health', newLife);
    updateHealthBar(targetElement);

    if (newLife <= 0) {
        zombie = getEnemyFromElement(targetElement);
        targetElement.remove();
        currentRoom.removeZombie(zombie);

        player.incrementZombiesKilled(); // Increment the number of zombies killed

        player.score += 1000;
        document.getElementById('scoreboard').textContent = "Score: " + player.score;

        if (currentRoom.isClear()) {
            currentRoom.openDoors();
        }
    }
}