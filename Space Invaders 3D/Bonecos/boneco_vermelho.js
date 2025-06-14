// Configuração básica da cena, câmera e renderizador
const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(window.innerWidth, window.innerHeight);
renderer.setClearColor(0x000000);
document.body.appendChild(renderer.domElement);

// Esperar até que o OrbitControls seja carregado
let controls;
function initControls() {
    if (typeof THREE.OrbitControls === 'function') {
        controls = new THREE.OrbitControls(camera, renderer.domElement);
        controls.enableDamping = true;
        controls.dampingFactor = 0.05;
        controls.autoRotate = false;
    } else {
        setTimeout(initControls, 100);
    }
}
initControls();

// Luz ambiente
const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
scene.add(ambientLight);

// Luz direcional
const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
directionalLight.position.set(1, 1, 1);
scene.add(directionalLight);

// Função para criar o invasor do Space Invaders
function createSpaceInvader() {
    // Grupo para conter todas as partes do invasor
    const invaderGroup = new THREE.Group();
    
    // Design do invasor
    const invaderDesign = [
        [0,0,0,0,0,1,0,0,0,0,0],
        [0,0,0,0,1,1,1,0,0,0,0],
        [0,0,0,1,0,1,0,1,0,0,0],
        [0,0,1,1,1,1,1,1,1,0,0],
        [0,0,1,1,1,1,1,1,1,0,0],
        [0,0,0,1,0,1,0,1,0,0,0],
        [0,0,1,0,0,0,0,0,1,0,0],
        [0,0,0,1,0,0,0,1,0,0,0]
    ];

    // Cor e reflexao da luz no specular
    const invaderMaterial = new THREE.MeshPhongMaterial({ 
        color: 0xFF0000,
        shininess: 30,
        specular: 0xffffff
    });
    
    // Cor da profundidade
    const invaderMaterialDarker = new THREE.MeshPhongMaterial({ 
        color: 0xCC3333,
        shininess: 30,
        specular: 0xffffff
    });
    
    // Tamanho de cada bloco do invasor
    const blockSize = 1;
    const depth = blockSize; // Profundidade igual à largura/altura para manter proporções
    
    // Criar blocos para formar o invasor
    for (let y = 0; y < invaderDesign.length; y++) {
        for (let x = 0; x < invaderDesign[y].length; x++) {
            if (invaderDesign[y][x] === 1) {
                const geometry = new THREE.BoxGeometry(blockSize, blockSize, depth);

                // Materiais por face: [right, left, top, bottom, front, back]
                const cubeMaterials = [
                    invaderMaterialDarker, // right
                    invaderMaterialDarker, // left
                    invaderMaterialDarker, // top
                    invaderMaterialDarker, // bottom
                    invaderMaterial,       // front
                    invaderMaterialDarker  // back
                ];

                const cube = new THREE.Mesh(geometry, cubeMaterials);

                // Posicionar o cubo
                cube.position.set(
                    x * blockSize - (invaderDesign[y].length * blockSize / 2) + blockSize / 2,
                    (invaderDesign.length - y) * blockSize - (invaderDesign.length * blockSize / 2),
                    0
                );

                invaderGroup.add(cube);
            }
        }
    }

    return invaderGroup;
}

// Criar o invasor e adicioná-lo à cena
const spaceInvader = createSpaceInvader();
spaceInvader.scale.set(2, 2, 2); // Aumentar o tamanho do boneco
scene.add(spaceInvader);

// Posicionar a câmera
camera.position.z = 20;

// Animação
function animate() {
    requestAnimationFrame(animate);
    
    // Atualizar controles se estiverem disponíveis
    if (controls) {
        controls.update();
    }
    
    // Renderizar a cena
    renderer.render(scene, camera);
}

// Lidar com redimensionamento da janela
window.addEventListener('resize', () => {
    const width = window.innerWidth;
    const height = window.innerHeight;
    
    camera.aspect = width / height;
    camera.updateProjectionMatrix();
    
    renderer.setSize(width, height);
});

// Iniciar animação
animate();
