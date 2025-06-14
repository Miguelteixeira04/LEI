// Sons permitidos
function somPermitido(tipos = []) {
  const settings = JSON.parse(localStorage.getItem("soundSettings") || "{}");
  
  // Associar sons às categorias
  const mapa = {
    tiro: ["effects"],
    explosion: ["effects"],
    levelup: ["music"],
    victory: ["music"],
    death: ["music"],
    background: ["ambient"]
  };

  for (const tipo of tipos) {
    const categorias = mapa[tipo] || [];
    // Se qq categoria estiver desligada, o som é bloqueado
    for (const cat of categorias) {
      if (settings[cat] === false) {
        return false;
      }
    }
  }
  return true;
}

// Sistema de níveis
let currentLevel;
let vidas;
let pontuacao=0;
let invaderMoveWait = true;
let lastMoveTime = 0;
let invaderShootingEnabled = false;
let invaderShootInterval = 1500;
let playerMoveSpeed = 0.7;

let playerShootInterval = 250; // Intervalo base de tiro da nave
let invaderMoveSpeed = 0.03; // Velocidade de movimento dos invasores
let barreirasAtivas = false; // Controla se as barreiras estão ativas no nível

// Controlo da câmera
let isOrthoCamera = false;
let followCamera;
let orthoCamera;

// Sistema de pausa
let jogoPausado = false;
let menuPausa = null;

// Verifica o cache 
const proximoNivelStorage = localStorage.getItem("proximoNivel");
const pontuacaoAtualStorage = localStorage.getItem("pontuacaoAtual");

if (proximoNivelStorage) {
    currentLevel = parseInt(proximoNivelStorage);
    pontuacao = pontuacaoAtualStorage ? parseInt(pontuacaoAtualStorage) : 0;
    vidas = localStorage.getItem("vidas") ? parseInt(localStorage.getItem("vidas")) : 3; // Mantém vidas do nível anterior

    localStorage.removeItem("proximoNivel");
    localStorage.removeItem("pontuacaoAtual");
    atualizarPontuacao();
    console.log(`Continuando para o Nível: ${currentLevel}, Pontuação: ${pontuacao}, Vidas: ${vidas}`);
} 
else {
    // Novo jogo (ou "Play Again")
    currentLevel = localStorage.getItem("currentLevel") ? parseInt(localStorage.getItem("currentLevel")) : 1;
    vidas = localStorage.getItem("vidas") ? parseInt(localStorage.getItem("vidas")) : 3;
    pontuacao = localStorage.getItem("pontuacao") ? parseInt(localStorage.getItem("pontuacao")) : 0;

    // Garante que, se for um "Play Again", as vidas e nível sejam resetados como definimos no gameover.html e a pontuação seja zero.
    if (localStorage.getItem("currentLevel") === "1" && localStorage.getItem("vidas") === "3" && localStorage.getItem("pontuacao") === "0") {
        console.log("Iniciando novo jogo (Play Again) - Nível 1, 3 vidas, Pontuação 0");
    } 
    else if (!proximoNivelStorage) {
        currentLevel = parseInt(localStorage.getItem("currentLevel") || "1");
        vidas = parseInt(localStorage.getItem("vidas") || "3");
        pontuacao = parseInt(localStorage.getItem("pontuacao") || "0");
        console.log(`Iniciando Nível: ${currentLevel}, Pontuação: ${pontuacao}, Vidas: ${vidas} (estado recuperado ou padrão para novo jogo)`);
    }
}

// Vidas nunca sejam inválidas
if (isNaN(vidas) || (vidas <= 0 && currentLevel === 1 && !proximoNivelStorage)) {
    vidas = 3;
}
// Coloca pontuação a zero
if (currentLevel === 1 && !proximoNivelStorage) {
    pontuacao = 0;
}

// Atualiza o localStorage com os valores definitivos para o início do jogo
localStorage.setItem("vidas", vidas.toString());
localStorage.setItem("pontuacao", pontuacao.toString());
localStorage.setItem("currentLevel", currentLevel.toString());

// Configuração da cena e camera
const canvas = document.getElementById("renderCanvas")
const engine = new BABYLON.Engine(canvas, true)

function atualizarInfoCamera(tipo) {
  const label = document.getElementById("camera-info");
  if (label) {
    label.textContent = `${tipo}`;
  }
}

const createScene = () => {
  const scene = new BABYLON.Scene(engine)
  scene.clearColor = new BABYLON.Color4.FromHexString("#030819")

  // camara em 3ª pessoa
  followCamera = new BABYLON.FollowCamera("followCamera", new BABYLON.Vector3(0, -12, -3), scene);
  followCamera.radius = 0; // Distância da câmera ao alvo
  followCamera.heightOffset = -12; // Altura da câmera em relação ao alvo
  followCamera.rotationOffset = 0; // Rotação da câmera em relação ao alvo
  followCamera.cameraAcceleration = 0.1; // Aceleração da câmera
  followCamera.maxCameraSpeed = 10; // Velocidade máxima da câmera
  followCamera.lockedTarget = null; // Alvo da câmera
  followCamera.fov = 1.2; // Campo de visão da câmera

    // Câmera ortogonal (vista de cima)
  orthoCamera = new BABYLON.FreeCamera("orthoCamera", new BABYLON.Vector3(0, -80, -180), scene);
  orthoCamera.mode = BABYLON.Camera.ORTHOGRAPHIC_CAMERA;
  orthoCamera.orthoTop = 30;
  orthoCamera.orthoBottom = -10;
  orthoCamera.orthoLeft = -50;
  orthoCamera.orthoRight = 50;
  orthoCamera.setTarget(new BABYLON.Vector3(0, 0, 0));

  // Definir a câmera ativa inicial
  // Restaurar estado da câmara do localStorage
  const cameraGuardada = localStorage.getItem("isOrthoCamera");
  isOrthoCamera = cameraGuardada === "1";

  if (isOrthoCamera) {
    scene.activeCamera = orthoCamera;
    atualizarInfoCamera("Top-Down View");
    
  } else {
    scene.activeCamera = followCamera;
    atualizarInfoCamera("Third-Person View");
  }

  // ==============Luzes =========================
  const ambientLight = new BABYLON.HemisphericLight("ambientLight", new BABYLON.Vector3(0, -1, 0), scene)
  ambientLight.intensity = 0.7
  ambientLight.diffuseColor = new BABYLON.Color3(1, 1, 1)
  ambientLight.specularColor = new BABYLON.Color3(1, 1, 1)
  ambientLight.groundColor = new BABYLON.Color3(0.2, 0.2, 0.3)

  // Luz direcional para criar sombras e destacar detalhes
  const directionalLight = new BABYLON.DirectionalLight("directionalLight", new BABYLON.Vector3(0, -1, 1), scene)
  directionalLight.intensity = 0.8

  // =============Carregar o modelo da nave==================
  BABYLON.SceneLoader.ImportMesh(
    null,
    "../Bonecos/",
    "T-65 X-Wing Starfighter.glb",
    scene,
    (meshes) => {

      nave = meshes[0];
      //tamanho da nave
      nave.scaling = new BABYLON.Vector3(0.2, 0.2, 0.2);

      //Posição da nave
      nave.position = new BABYLON.Vector3(0, -9, 0);
      
      //Rotação da nave
      nave.rotation = new BABYLON.Vector3(-1.5, Math.PI / 5, 5.65)

      //Set the camera to follow the ship
      followCamera.lockedTarget = nave

      scene.registerBeforeRender(() => {
        if (nave) {
          // Atualizar apenas a câmera perpetiva se ela estiver ativa
          if (!isOrthoCamera) {
            const naveY = nave.position.y;
            const cameraY = naveY - 1.8;
            const cameraZ = -3 + (naveY * -0.2);

            followCamera.position = new BABYLON.Vector3(nave.position.x, cameraY, cameraZ);
            followCamera.setTarget(new BABYLON.Vector3(nave.position.x, naveY + 15, 0)); // Olha para cima do campo de jogo
          
          } else {
            orthoCamera.position.x = nave.position.x;
            orthoCamera.setTarget(new BABYLON.Vector3(nave.position.x, 0, 0));
          }
        }
      })
    },
    null,
    (scene, message) => {
      console.error("Erro ao carregar o modelo:", message)
    },
  )
  atualizarVidas();
  return scene;
}

function salvarPontuacaoFinal() {
  localStorage.setItem("pontuacaoFinal", pontuacao.toString());

  // Atualizar highscore se for superior
  let highscore = parseInt(localStorage.getItem("highscore") || "0");
  if (pontuacao > highscore) {
    localStorage.setItem("highscore", pontuacao.toString());
  }
}

// Função para alternar entre câmeras
function trocarCamera() {
  isOrthoCamera = !isOrthoCamera;

  if (isOrthoCamera) {
    scene.activeCamera = orthoCamera;
    console.log("Top-Down View ativada");
    atualizarInfoCamera("Top-Down View");

  } else {
    scene.activeCamera = followCamera;
    console.log("Third-Person View ativada");
    atualizarInfoCamera("Third-Person View");
  }
}

const scene = createScene()

//==================Criar Barreiras=================
// Criar o grupo de barreiras
const barreirasGroup = new BABYLON.TransformNode("barrierGroup", scene)

//Configurações
const spacingBarreira = 10 // Ajusta o espaçamento entre as barreiras
const numBarreiras = 3 // Número de barreiras
const startXBarreira = -((numBarreiras - 1) * spacingBarreira) / 2 // Posição inicial x
const posYBarreira = 0 // Posição y das barreiras
const posZBarreira = 0 // Distância ao jogador

const barreiras = []

for (let i = 0; i < numBarreiras; i++) {
  const posX = startXBarreira + i * spacingBarreira;
  const barreira = createBarrier(scene, posX, posYBarreira, posZBarreira);
  barreira.parent = barreirasGroup;
  barreiras.push(barreira);
}

atualizarVisibilidadeBarreiras(); 

// Função para controlar visibilidade das barreiras
function atualizarVisibilidadeBarreiras() {
  barreirasGroup.setEnabled(barreirasAtivas);
}

//==================Invasores=================
// Criar o grupo principal de todos os invasores
const invadersGroup = new BABYLON.TransformNode("invadersGroup", scene)

// Configurações dos invasores
const spacing = 5 // Espaçamento entre os invasores
const rowHeight = 5 // Altura entre as linhas
const numCols = 6 // Número de colunas
const startX = (-(numCols - 1) * spacing) / 2 // Posição inicial x

// Ajustar a posição vertical do grupo de invasores para melhor visibilidade
invadersGroup.position.y = 10

// Criar as 6 linhas de invasores (2 vermelhos, 2 amarelos, 2 rosas)
const allInvaders = []

// 2 linhas de invasores 
if (typeof createRedInvader === 'function' && typeof createYellowInvader === 'function' && typeof createPinkInvader === 'function') {
  // 2 linhas de invasores vermelhos
  for (let row = 0; row < 2; row++) {
    for (let col = 0; col < numCols; col++) {
      const invader = createRedInvader(scene, startX + col * spacing, 15 + row * rowHeight, 0)
      invader.parent = invadersGroup
      allInvaders.push(invader)
    }
  }
  // 2 linhas de invasores amarelos
  for (let row = 0; row < 2; row++) {
    for (let col = 0; col < numCols; col++) {
      const invader = createYellowInvader(scene, startX + col * spacing, 5 + row * rowHeight, 0)
      invader.parent = invadersGroup
      allInvaders.push(invader)
    }
  }
  // 2 linhas de invasores rosas
  for (let row = 0; row < 2; row++) {
    for (let col = 0; col < numCols; col++) {
      const invader = createPinkInvader(scene, startX + col * spacing, -5 + row * rowHeight, 0)
      invader.parent = invadersGroup
      allInvaders.push(invader)
    }
  }
} else {
  console.error("Invader creation functions (createRedInvader, etc.) are not defined.");
}

// Variaveis para controlar o movimento dos invasores
let direction = 1 // 1 para direita, -1 para esquerda
const moveSpeed = 0.03
const moveDistance = 20 
const initialPosition = invadersGroup.position.x
let shouldDescend = false
const descendAmount = 2

// =============Animação para mover os invasores ==============
scene.registerBeforeRender(() => {

  if(jogoPausado) return;

  const currentTime = Date.now();

  if (!invaderMoveWait || currentTime - lastMoveTime > 1000) {
    invadersGroup.position.x += moveSpeed * direction;

    lastMoveTime = currentTime;
    
    // Verificar se chegou ao limite do movimento horizontal
    if (direction > 0 && invadersGroup.position.x >= initialPosition + moveDistance / 2) {
      direction = -1
      shouldDescend = true
    } else if (direction < 0 && invadersGroup.position.x <= initialPosition - moveDistance / 2) {
      direction = 1
      shouldDescend = true
    }

    // Descer após completar o movimento horizontal
    if (shouldDescend) {
      invadersGroup.position.y -= descendAmount
      shouldDescend = false

      // Verificar se os invasores alcançaram a altura das barreiras ou do jogador
      if (invadersGroup.position.y <= posYBarreira + 2 || (nave && invadersGroup.position.y <= nave.position.y + 5)) {
          localStorage.setItem("pontuacaoFinal", pontuacao.toString());
          // Limpar estado para não tentar continuar
          salvarPontuacaoFinal() 
          localStorage.removeItem("proximoNivel");
          localStorage.removeItem("pontuacaoAtual");
          setTimeout(() => {
            window.location.href = "../Game/gameover.html";
          }, 200);
          return;
      }
    }
  }

  for (let i = tiros.length - 1; i >= 0; i--) {
    const tiro = tiros[i]
    tiro.position.y += 0.5
    
    if (tiro.position.y > 50) {
      tiro.dispose()
      tiros.splice(i, 1)
    }
  }

  // Movimento e colisão dos tiros inimigos
  for (let i = tirosInimigos.length - 1; i >= 0; i--) {
    const tiro = tirosInimigos[i]
    tiro.position.y -= 0.3

    // Verificar colisão com a nave
    if (nave) {
      const naveMeshes = nave.getChildMeshes()
      for (let j = 0; j < naveMeshes.length; j++) {
        if (tiro.intersectsMesh(naveMeshes[j], false)) {
          tiro.dispose()
          tirosInimigos.splice(i, 1)
          reduzirVida()
          SomExplosao()
          break
        }
      }
    }

    // Se sair do ecrã
    if (tiro.position.y < -20) {
      tiro.dispose()
      tirosInimigos.splice(i, 1)
    }
  }

  verificarColisoes(); // colisão entre tiros da nave e invasores

  if (barreirasAtivas) {
    verificarColisoesTirosInimigosComBarreiras(); // colisão entre tiros inimigos e barreiras
    verificarColisoesTirosComBarreiras(); // colisão entre tiros da nave e barreiras
  }

  // Verificar colisões entre invasores e a nave
  if (nave) {
    for (let i = allInvaders.length - 1; i >= 0; i--) {
      const invasor = allInvaders[i];
      if (invasor.isDisposed()) continue; 
      const filhos = invasor.getChildMeshes(true); 
      const naveMeshes = nave.getChildMeshes(true);

      // Verificar se qualquer invasor toca a nave
      for (let j = 0; j < filhos.length; j++) {
        for (let k = 0; k < naveMeshes.length; k++) {
          if (filhos[j].intersectsMesh(naveMeshes[k], false)) {
            localStorage.setItem("pontuacaoFinal", pontuacao.toString());
            localStorage.removeItem("proximoNivel");
            localStorage.removeItem("pontuacaoAtual");
            setTimeout(() => {
              window.location.href = "../Game/gameover.html"
            }, 200);
            return
          }
        }
      }
    }
  }

  // Verificar colisões entre invasores e as barreiras
  for (let i = allInvaders.length - 1; i >= 0; i--) {
    const invasor = allInvaders[i];
    const filhos = invasor.getChildMeshes();

    for (let j = 0; j < filhos.length; j++) {
      for (let k = barreiras.length - 1; k >= 0; k--) {
        const barreira = barreiras[k];
        const blocos = barreira.getChildMeshes();

        for (let l = 0; l < blocos.length; l++) {
          if (filhos[j].intersectsMesh(blocos[l], false)) {
            // Remove o bloco da barreira
            blocos[l].dispose();
            barreira.removeChild(blocos[l]); // Remove o bloco do TransformNode da barreira

            // Se a barreira já não tiver blocos, remove do array
          if (barreira.getChildMeshes().length === 0) {
            barreiras.splice(j, 1);
            barreira.dispose();
          }

          SomExplosao(); // Som de impacto
          return;
          }
        }
      }
    }
  }

  // Movimento horizontal suave da nave
  if (nave) {
    let move = 0;
    if (teclasPressionadas["ArrowLeft"] || teclasPressionadas["KeyA"]) {
      move -= velocidadeNave;
      // Inclinar a nave para a esquerda quando se move para a esquerda
      nave.rotation.z = Math.min(nave.rotation.z + 0.03, 5.85); // Limite de inclinação
    }
    if (teclasPressionadas["ArrowRight"] || teclasPressionadas["KeyD"]) {
      move += velocidadeNave;
      // Inclinar a nave para a direita quando se move para a direita
      nave.rotation.z = Math.max(nave.rotation.z - 0.03, 5.45); // Limite de inclinação
    }
    // Retornar à rotação normal quando não está se movendo
    const epsilon = 0.01;
    if (
      !teclasPressionadas["ArrowLeft"] &&
      !teclasPressionadas["KeyA"] &&
      !teclasPressionadas["ArrowRight"] &&
      !teclasPressionadas["KeyD"]
    ) {
      if (Math.abs(nave.rotation.z - 5.65) > epsilon) {
        if (nave.rotation.z < 5.65) {
          nave.rotation.z += 0.02;
        } else if (nave.rotation.z > 5.65) {
          nave.rotation.z -= 0.02;
        }
      } else {
        nave.rotation.z = 5.65; // Corrige o valor para eliminar tremores
      }
    }

    nave.position.x += move;
    if (nave.position.x < limiteEsquerdo) nave.position.x = limiteEsquerdo;
    if (nave.position.x > limiteDireito) nave.position.x = limiteDireito;

    // Disparo contínuo com intervalo
    const agora = Date.now();
    if ((teclasPressionadas["Space"] || teclasPressionadas["KeyW"]) && agora - ultimoTiro > intervaloTiro) {
      createTiros(nave.position);
      ultimoTiro = agora;
    }
  }
});

// Lidar com redimensionamento da janela
window.addEventListener("resize", () => {
  engine.resize()
})

// Executar o loop de renderização
engine.runRenderLoop(() => {
  if (scene) { 
    scene.render()
  }
})

//================disparo flash=================
function criarFlashDisparo(scene, position) {
  const particleSystem = new BABYLON.ParticleSystem("flashDisparo", 30, scene);

  particleSystem.particleTexture = new BABYLON.Texture("https://playground.babylonjs.com/textures/flare.png", scene);
  particleSystem.emitter = position.clone();
  particleSystem.minEmitBox = new BABYLON.Vector3(-0.1, -0.1, -0.1);
  particleSystem.maxEmitBox = new BABYLON.Vector3(0.1, 0.1, 0.1);

  particleSystem.color1 = new BABYLON.Color4(1, 1, 0.5, 1);  // amarelo claro
  particleSystem.color2 = new BABYLON.Color4(1, 0.8, 0, 1);  // laranja
  particleSystem.colorDead = new BABYLON.Color4(1, 0.5, 0, 0);

  particleSystem.minSize = 0.2;
  particleSystem.maxSize = 0.4;

  particleSystem.minLifeTime = 0.1;
  particleSystem.maxLifeTime = 0.2;

  particleSystem.emitRate = 100;

  particleSystem.blendMode = BABYLON.ParticleSystem.BLENDMODE_ONEONE;
  particleSystem.direction1 = new BABYLON.Vector3(-1, 1, -1);
  particleSystem.direction2 = new BABYLON.Vector3(1, 1, 1);
  particleSystem.minEmitPower = 2;
  particleSystem.maxEmitPower = 4;
  particleSystem.gravity = new BABYLON.Vector3(0, 0, 0);
  particleSystem.updateSpeed = 0.02;

  particleSystem.start();

  // Parar e remover o sistema após 200ms
  setTimeout(() => {
    particleSystem.stop();
    particleSystem.dispose();
  }, 200);
}

//================Criar rasto do tiro=================
function criarRastroDoTiro(tiro) {
  const trailParticles = new BABYLON.ParticleSystem("trailParticles", 100, scene);
  trailParticles.particleTexture = new BABYLON.Texture("https://playground.babylonjs.com/textures/flare.png", scene);

  trailParticles.emitter = tiro;
  trailParticles.minEmitBox = new BABYLON.Vector3(0, 0, 0);
  trailParticles.maxEmitBox = new BABYLON.Vector3(0, 0, 0);

  trailParticles.color1 = new BABYLON.Color4(1, 1, 0, 1);
  trailParticles.color2 = new BABYLON.Color4(1, 0.8, 0, 1);
  trailParticles.colorDead = new BABYLON.Color4(1, 0.5, 0, 0);

  trailParticles.minSize = 0.1;
  trailParticles.maxSize = 0.2;

  trailParticles.minLifeTime = 0.1;
  trailParticles.maxLifeTime = 0.2;

  trailParticles.emitRate = 60;
  trailParticles.blendMode = BABYLON.ParticleSystem.BLENDMODE_ONEONE;
  trailParticles.gravity = new BABYLON.Vector3(0, 0, 0);

  trailParticles.direction1 = new BABYLON.Vector3(0, -1, 0);
  trailParticles.direction2 = new BABYLON.Vector3(0, -1, 0);

  trailParticles.minEmitPower = 1;
  trailParticles.maxEmitPower = 1;
  trailParticles.updateSpeed = 0.02;

  trailParticles.start();

  // Parar o trail quando o tiro for destruído
  tiro.onDisposeObservable.add(() => {
    trailParticles.stop();
    trailParticles.dispose();
  });
}

//================Tiros=================
let nave
const tiros = []

function createTiros(position) {
  const tiro = BABYLON.MeshBuilder.CreateSphere("tiro", { diameter: 0.3 }, scene)
  tiro.position = position.clone()
  tiro.material = new BABYLON.StandardMaterial("tiroMaterial", scene)
  tiro.material.diffuseColor = new BABYLON.Color3(1, 1, 0) // Cor amarela

  criarFlashDisparo(scene, position)
  criarRastroDoTiro(tiro)

  tiros.push(tiro)
  SomTiros()
}

//================Movimentar Nave e Disparo Suave=================
let velocidadeNave = playerMoveSpeed

const limiteEsquerdo = -40;
const limiteDireito = 40;

const teclasPressionadas = {};
const podeDisparar = true;
let intervaloTiro = playerShootInterval; // milissegundos entre tiros
let ultimoTiro = 0;

window.addEventListener("keydown", (event) => {
  teclasPressionadas[event.code] = true
    // Adicionar detecção da tecla C para alternar câmera
  if (event.code === "KeyC") {
    trocarCamera();
  }
})

window.addEventListener("keyup", (event) => {
  teclasPressionadas[event.code] = false
})

//================Sistema de Pausa Completo=================

// Função para salvar o estado completo do jogo
function salvarEstadoJogo() {
  const estadoJogo = {
    // Informações básicas
    currentLevel: currentLevel,
    pontuacao: pontuacao,
    vidas: vidas,
    
    // Estado da nave
    navePosition: nave ? {
      x: nave.position.x,
      y: nave.position.y,
      z: nave.position.z
    } : null,
    naveRotation: nave ? {
      x: nave.rotation.x,
      y: nave.rotation.y,
      z: nave.rotation.z
    } : null,
    
    // Estado dos invasores
    invadersGroupPosition: {
      x: invadersGroup.position.x,
      y: invadersGroup.position.y,
      z: invadersGroup.position.z
    },
    direction: direction,
    shouldDescend: shouldDescend,
    lastMoveTime: lastMoveTime,
    
    // Invasores ativos (posições relativas ao grupo)
    invasoresAtivos: allInvaders.map(invasor => ({
      position: {
        x: invasor.position.x,
        y: invasor.position.y,
        z: invasor.position.z
      },
      invaderType: invasor.invaderType || 'unknown'
    })),
    
    // Tiros da nave
    tirosNave: tiros.map(tiro => ({
      position: {
        x: tiro.position.x,
        y: tiro.position.y,
        z: tiro.position.z
      }
    })),
    
    // Tiros dos inimigos
    tirosInimigos: tirosInimigos.map(tiro => ({
      position: {
        x: tiro.position.x,
        y: tiro.position.y,
        z: tiro.position.z
      }
    })),
    
    // Estado das barreiras
    barreirasEstado: barreiras.map((barreira, index) => ({
      index: index,
      blocosAtivos: barreira.getChildMeshes().map(bloco => ({
        position: {
          x: bloco.position.x,
          y: bloco.position.y,
          z: bloco.position.z
        }
      }))
    })),
    
    // Configurações do nível
    invaderMoveWait: invaderMoveWait,
    invaderShootingEnabled: invaderShootingEnabled,
    invaderShootInterval: invaderShootInterval,
    playerMoveSpeed: playerMoveSpeed,
    playerShootInterval: playerShootInterval,
    invaderMoveSpeed: invaderMoveSpeed,
    barreirasAtivas: barreirasAtivas,
    
    // Timestamp para controle
    timestamp: Date.now()
  };
  
  localStorage.setItem("estadoJogoPausado", JSON.stringify(estadoJogo));
  console.log("Estado do jogo salvo:", estadoJogo);
}

// Função para carregar o estado do jogo
function carregarEstadoJogo() {
  const estadoSalvo = localStorage.getItem("estadoJogoPausado");
  if (!estadoSalvo) return false;
  
  try {
    const estado = JSON.parse(estadoSalvo);
    console.log("Carregando estado do jogo:", estado);
    
    // Restaurar informações básicas
    currentLevel = estado.currentLevel;
    pontuacao = estado.pontuacao;
    vidas = estado.vidas;
    
    // Restaurar configurações do nível
    invaderMoveWait = estado.invaderMoveWait;
    invaderShootingEnabled = estado.invaderShootingEnabled;
    invaderShootInterval = estado.invaderShootInterval;
    playerMoveSpeed = estado.playerMoveSpeed;
    playerShootInterval = estado.playerShootInterval;
    invaderMoveSpeed = estado.invaderMoveSpeed;
    barreirasAtivas = estado.barreirasAtivas;
    
    // Restaurar variáveis de movimento
    direction = estado.direction;
    shouldDescend = estado.shouldDescend;
    lastMoveTime = estado.lastMoveTime;
    velocidadeNave = playerMoveSpeed;
    intervaloTiro = playerShootInterval;
    
    // Atualizar UI
    atualizarPontuacao();
    atualizarVidas();
    document.getElementById("level").textContent = currentLevel;
    atualizarVisibilidadeBarreiras();
    
    // Aguardar a nave carregar antes de restaurar posição
    const aguardarNave = setInterval(() => {
      if (nave && estado.navePosition) {
        nave.position.x = estado.navePosition.x;
        nave.position.y = estado.navePosition.y;
        nave.position.z = estado.navePosition.z;
        
        if (estado.naveRotation) {
          nave.rotation.x = estado.naveRotation.x;
          nave.rotation.y = estado.naveRotation.y;
          nave.rotation.z = estado.naveRotation.z;
        }
        clearInterval(aguardarNave);
      }
    }, 100);
    
    // Restaurar posição do grupo de invasores
    invadersGroup.position.x = estado.invadersGroupPosition.x;
    invadersGroup.position.y = estado.invadersGroupPosition.y;
    invadersGroup.position.z = estado.invadersGroupPosition.z;
    
    // Remover invasores que foram destruídos
    for (let i = allInvaders.length - 1; i >= 0; i--) {
      const invasorAtual = allInvaders[i];
      const invasorSalvo = estado.invasoresAtivos.find(inv => 
        Math.abs(inv.position.x - invasorAtual.position.x) < 0.1 &&
        Math.abs(inv.position.y - invasorAtual.position.y) < 0.1
      );
      
      if (!invasorSalvo) {
        invasorAtual.dispose();
        allInvaders.splice(i, 1);
      }
    }
    
    // Restaurar tiros da nave
    // Limpar tiros existentes
    tiros.forEach(tiro => tiro.dispose());
    tiros.length = 0;
    
    // Recriar tiros salvos
    estado.tirosNave.forEach(tiroData => {
      const tiro = BABYLON.MeshBuilder.CreateSphere("tiro", { diameter: 0.3 }, scene);
      tiro.position.x = tiroData.position.x;
      tiro.position.y = tiroData.position.y;
      tiro.position.z = tiroData.position.z;
      tiro.material = new BABYLON.StandardMaterial("tiroMaterial", scene);
      tiro.material.diffuseColor = new BABYLON.Color3(1, 1, 0);
      criarRastroDoTiro(tiro);
      tiros.push(tiro);
    });
    
    // Restaurar tiros dos inimigos
    // Limpar tiros existentes
    tirosInimigos.forEach(tiro => tiro.dispose());
    tirosInimigos.length = 0;
    
    // Recriar tiros salvos
    estado.tirosInimigos.forEach(tiroData => {
      const tiro = BABYLON.MeshBuilder.CreateSphere("tiroInimigo", { diameter: 0.3 }, scene);
      tiro.position.x = tiroData.position.x;
      tiro.position.y = tiroData.position.y;
      tiro.position.z = tiroData.position.z;
      tiro.material = new BABYLON.StandardMaterial("tiroInimigoMaterial", scene);
      tiro.material.diffuseColor = new BABYLON.Color3(1, 0, 0);
      tirosInimigos.push(tiro);
    });
    
    // Restaurar estado das barreiras
    estado.barreirasEstado.forEach(barreiraData => {
      if (barreiraData.index < barreiras.length) {
        const barreira = barreiras[barreiraData.index];
        const blocosAtuais = barreira.getChildMeshes();
        
        // Remover blocos que não existiam no estado salvo
        for (let i = blocosAtuais.length - 1; i >= 0; i--) {
          const blocoAtual = blocosAtuais[i];
          const blocoSalvo = barreiraData.blocosAtivos.find(bloco =>
            Math.abs(bloco.position.x - blocoAtual.position.x) < 0.1 &&
            Math.abs(bloco.position.y - blocoAtual.position.y) < 0.1
          );
          
          if (!blocoSalvo) {
            blocoAtual.dispose();
          }
        }
      }
    });
    
    // Reiniciar sistema de tiros dos invasores
    clearInterval(invaderShootIntervalId);
    if (invaderShootingEnabled) {
      invaderShootIntervalId = setInterval(shootFromRandomInvader, invaderShootInterval);
    }
    
    // Limpar estado salvo
    localStorage.removeItem("estadoJogoPausado");
    
    console.log("Estado do jogo restaurado com sucesso!");
    return true;
    
  } catch (error) {
    console.error("Erro ao carregar estado do jogo:", error);
    localStorage.removeItem("estadoJogoPausado");
    return false;
  }
}

// Modificar o sistema de pausa
window.addEventListener("keydown", (event) => {
  if (event.code === "Escape") {
    // Salvar estado antes de pausar
    salvarEstadoJogo();
    
    // Pausar o jogo
    jogoPausado = true;
    
    // Parar o disparo dos invasores
    clearInterval(invaderShootIntervalId);
    
    // Ir para a página de pausa
    window.location.href = "/Game/Pause.html";
  }
});

//================Níveis=================
function setupLevel(level) {
  currentLevel = level;
  document.getElementById("level").textContent = level;
  
  // Configurações específicas para cada nível
  switch(level) {
    case 1:
      invaderMoveWait = true;
      invaderShootingEnabled = false;
      playerMoveSpeed = 0.7;
      playerShootInterval = 200;
      invaderMoveSpeed = 0.03;
      barreirasAtivas = false;
      break;
      
    case 2:
      invaderMoveWait = false; // Movimento contínuo dos invasores
      invaderShootingEnabled = true; // Invasores podem disparar
      invaderShootInterval = 2000; // TTempo entre tiros dos invasores
      playerMoveSpeed = 0.7; // Velocidade da nave
      playerShootInterval = 200; // Intervalo de tiro do jogador
      invaderMoveSpeed = 0.02; // Movimento dos invasores
      barreirasAtivas = false;
      break;
      
    case 3:
      invaderMoveWait = false;
      invaderShootingEnabled = true;
      invaderShootInterval = 1500;
      playerMoveSpeed = 0.8; 
      playerShootInterval = 200;
      invaderMoveSpeed = 0.03;
      barreirasAtivas = true;
      break;
      
    case 4:
      invaderMoveWait = false;
      invaderShootingEnabled = true;
      invaderShootInterval = 1000; 
      playerMoveSpeed = 0.8;
      playerShootInterval = 225;
      invaderMoveSpeed = 0.03;
      barreirasAtivas = true;
      break;
      
    case 5:
      invaderMoveWait = false;
      invaderShootingEnabled = true;
      invaderShootInterval = 750; 
      playerMoveSpeed = 0.9; 
      playerShootInterval = 250; 
      invaderMoveSpeed = 0.03;
      barreirasAtivas = true;
      break;
  }

  // Atualizar variáveis globais
  velocidadeNave = playerMoveSpeed;
  intervaloTiro = playerShootInterval;

  // Atualizar visibilidade das barreiras
  atualizarVisibilidadeBarreiras();
  
  // Reiniciar o intervalo de tiro dos invasores
  clearInterval(invaderShootIntervalId);
  if (invaderShootingEnabled) {
    invaderShootIntervalId = setInterval(shootFromRandomInvader, invaderShootInterval);
  }

   console.log(`Nível ${level} configurado:`, {
    invaderMoveWait,
    invaderShootingEnabled,
    playerMoveSpeed,
    playerShootInterval,
    invaderMoveSpeed,
    barreirasAtivas,
    invaderShootInterval
  });
}

//================Criar Explosão =================
function criarExplosao(scene, position) {
  const particleSystem = new BABYLON.ParticleSystem("explosao", 100, scene);

  particleSystem.particleTexture = new BABYLON.Texture("https://playground.babylonjs.com/textures/flare.png", scene);

  particleSystem.emitter = position.clone();
  particleSystem.minEmitBox = new BABYLON.Vector3(-0.5, -0.5, -0.5);
  particleSystem.maxEmitBox = new BABYLON.Vector3(0.5, 0.5, 0.5);

  particleSystem.color1 = new BABYLON.Color4(1, 0.5, 0, 1);
  particleSystem.color2 = new BABYLON.Color4(1, 0, 0, 1);
  particleSystem.colorDead = new BABYLON.Color4(0, 0, 0, 0);

  particleSystem.minSize = 0.2;
  particleSystem.maxSize = 0.6;

  particleSystem.minLifeTime = 0.2;
  particleSystem.maxLifeTime = 0.5;

  particleSystem.emitRate = 500;

  particleSystem.blendMode = BABYLON.ParticleSystem.BLENDMODE_ONEONE;

  particleSystem.gravity = new BABYLON.Vector3(0, -9.81, 0);

  particleSystem.direction1 = new BABYLON.Vector3(-1, 1, -1);
  particleSystem.direction2 = new BABYLON.Vector3(1, 1, 1);

  particleSystem.minAngularSpeed = 0;
  particleSystem.maxAngularSpeed = Math.PI;

  particleSystem.minEmitPower = 1;
  particleSystem.maxEmitPower = 3;
  particleSystem.updateSpeed = 0.01;

  particleSystem.start();

  // Parar e remover o sistema após 1 segundo
  setTimeout(() => {
    particleSystem.stop();
    particleSystem.dispose();
  }, 500);
}

//================Colisão entre tiros e inimigos=================

function verificarColisoes() {
  for (let i = tiros.length - 1; i >= 0; i--) {
    const tiro = tiros[i];

    if (!tiro || tiro.isDisposed()) continue; // Segurança

    for (let j = allInvaders.length - 1; j >= 0; j--) {
      const invasor = allInvaders[j];
      if (!invasor || invasor.isDisposed()) continue; // Segurança

      const filhos = invasor.getChildMeshes(true);

      // Verifica colisão com qualquer um dos filhos
      for (let k = 0; k < filhos.length; k++) {
        if (tiro.intersectsMesh(filhos[k], false)) {
          
          //Remove o tiro e o invasor
          tiro.dispose();
          tiros.splice(i, 1);

          // Cria uma explosão na posição do invasor
          criarExplosao(scene, invasor.getAbsolutePosition());

          // Remover dos arrays específicos também
          const invaderType = invasor.invaderType;
          if (invaderType === 'red') {
            const index = redInvaders.indexOf(invasor);
            if (index > -1) redInvaders.splice(index, 1);
          } else if (invaderType === 'yellow') {
            const index = yellowInvaders.indexOf(invasor);
            if (index > -1) yellowInvaders.splice(index, 1);
          } else if (invaderType === 'pink') {
            const index = pinkInvaders.indexOf(invasor);
            if (index > -1) pinkInvaders.splice(index, 1);
          }

          invasor.dispose(); //remover o invasor
          allInvaders.splice(j, 1);       
          pontuacao += 100;
          atualizarPontuacao();
          SomExplosao();

          // Verifica se quando nao tiver inimigos faz winner
          if (allInvaders.length === 0) {
            console.log(`FIM DO NIVEL : ${currentLevel}`);

            if (currentLevel < 5) {
              const mensagem = document.getElementById("levelup");
              mensagem.textContent = 'LEVEL UP!';
              mensagem.style.display = 'block';
               SomLevelUp();

              localStorage.setItem("pontuacaoAtual", pontuacao.toString());
              localStorage.setItem("proximoNivel", (currentLevel + 1).toString());
              localStorage.setItem("vidas", vidas.toString()); 
              localStorage.setItem("isOrthoCamera", isOrthoCamera ? "1" : "0");

              setTimeout(() => {
                window.location.reload();
              }, 1500);

            } else {
              salvarPontuacaoFinal();
              localStorage.setItem("pontuacaoFinal", pontuacao.toString());
              localStorage.removeItem("proximoNivel");
              localStorage.removeItem("pontuacaoAtual");

              setTimeout(() => {
                window.location.href = "../Game/victory.html";
              }, 1500);
            }
          }
          return;
        }
      }
    }
  }
}

//================Colisão entre tiros inimigos e Barreiras=================
function verificarColisoesTirosInimigosComBarreiras() {
  for (let i = tirosInimigos.length - 1; i >= 0; i--) {
    const tiro = tirosInimigos[i];

    for (let j = barreiras.length - 1; j >= 0; j--) {
      const barreira = barreiras[j];
      const blocos = barreira.getChildMeshes(); // Cada barreira é composta por pequenos blocos

      for (let k = blocos.length - 1; k >= 0; k--) {
        const bloco = blocos[k];

        if (tiro.intersectsMesh(bloco, false)) {

          //Remove Tiro
          tiro.dispose();
          tirosInimigos.splice(i, 1);
          
          //Remove bloco da barreira
          bloco.dispose();

          // Se a barreira já não tiver blocos, remove do array
          if (barreira.getChildMeshes().length === 0) {
            barreiras.splice(j, 1);
            barreira.dispose();
          }
          SomExplosao();
          return; // Sai após uma colisão
        }
      }
    }
  }
}

//================Colisão entre tiros da Nave e Barreiras=================
function verificarColisoesTirosComBarreiras() {
  for (let i = tiros.length - 1; i >= 0; i--) {
    const tiro = tiros[i];

    for (let j = barreiras.length - 1; j >= 0; j--) {
      const barreira = barreiras[j];
      const blocos = barreira.getChildMeshes(); // Cada barreira é composta por pequenos blocos

      for (let k = blocos.length - 1; k >= 0; k--) {
        const bloco = blocos[k];

        if (tiro.intersectsMesh(bloco, false)) {
          // Remove o tiro
          tiro.dispose();
          tiros.splice(i, 1);

          return;
        }
      }
    }
  }
}

//================Som=================
// Função para tocar o som de explosão
function SomTiros() {
  if (!somPermitido(["tiro"])) return;

  const audio = new Audio("../Sons/tiro.mp3");
  audio.volume = 0.1;
  audio.play();
}

function SomExplosao() {
  if (!somPermitido(["explosion"])) return;

  const audio = new Audio("../Sons/explosion.mp3");
  audio.volume = 0.1;
  audio.play();
}

function SomLevelUp() {
  if (!somPermitido(["levelup"])) return;

  const audio = new Audio("../Sons/levelup.mp3");
  audio.volume = 1;
  audio.play();
}

function SomVitoria(){
  if (!somPermitido(["victory"])) return;

  const audio = new Audio("../Sons/victory.mp3")
  audio.volume = 0.8
  audio.play()
}

function SomMorte(){
  if (!somPermitido(["death"])) return;

  const audio = new Audio("../Sons/death.mp3")
  audio.volume = 0.8
  audio.play()
}

function SomFundo(){
  if (!somPermitido(["background"])) return;

  const audio = new Audio("../Sons/background.mp3");
  audio.volume = 0.1;
  audio.loop = true;
  audio.play();
}

//================Atualizar Pontuação=================
// Função para atualizar a pontuação na interface
function atualizarPontuacao() {
  // Atualiza a pontuação visível
  const pontuacaoElement = document.getElementById("pontuacao");
  if (pontuacaoElement) {
    pontuacaoElement.textContent = pontuacao.toString();
  }

  // Guarda pontuação atual
  localStorage.setItem("pontuacao", pontuacao.toString());

  // Verifica ou cria highscore
  let highscore = parseInt(localStorage.getItem("highscore") || "0");
  if (pontuacao > highscore) {
    highscore = pontuacao;
    localStorage.setItem("highscore", highscore.toString());
  }

  // Atualiza a interface do highscore
  const highscoreElement = document.getElementById("highscore");
  if (highscoreElement) {
    highscoreElement.textContent = highscore.toString();
  }
}

//===============Vidas da nave==================
function atualizarVidas() {
  document.getElementById("vidas").textContent = vidas; // Corrigido: atribuição
}

function reduzirVida() {
  vidas--;
  if (vidas < 0) vidas = 0;

  localStorage.setItem("vidas", vidas); // Guarda no localStorage
  atualizarVidas(); // Atualiza visualmente

  // Só vai para game over quando as vidas chegarem a 0
  if (vidas === 0) {
    salvarPontuacaoFinal() 
    setTimeout(() => {
      window.location.href = "../Game/gameover.html";
    }, 200);
  }
}

//================Sistema de disparo dos inimigos==================
const tirosInimigos = [];

function criarTiroInimigo(posicao) {
  const tiro = BABYLON.MeshBuilder.CreateSphere("tiroInimigo", { diameter: 0.3 }, scene);
  tiro.position = posicao.clone();
  tiro.material = new BABYLON.StandardMaterial("tiroInimigoMaterial", scene);
  tiro.material.diffuseColor = new BABYLON.Color3(1, 0, 0);
  tirosInimigos.push(tiro);
}

//Disparos aleatorios dos inimigos
function shootFromRandomInvader() {
  if (allInvaders.length === 0 || !invaderShootingEnabled) return;

  const invasorAleatorio = allInvaders[Math.floor(Math.random() * allInvaders.length)];
  if (invasorAleatorio) {
    criarTiroInimigo(invasorAleatorio.getAbsolutePosition());

    // No nível 5, dispara 2 tiros de cada vez
    if (currentLevel === 5) {
      setTimeout(() => {
        if (invasorAleatorio.isDisposed) return; // Verificar se o invasor ainda existe
        criarTiroInimigo(invasorAleatorio.getAbsolutePosition());
      }, 150); // Pequeno atraso entre os tiros
    }
  }
}

// Iniciar o sistema de tiros dos invasores
let invaderShootIntervalId = setInterval(shootFromRandomInvader, invaderShootInterval);

// Inicializar o jogo com o nível correto
window.addEventListener("DOMContentLoaded", () => {

  //Apagar cache do highscore
  //localStorage.removeItem("highscore");

  SomFundo(); 

  // Verificar se estamos voltando de uma pausa
  if (carregarEstadoJogo()) {
    console.log("Jogo restaurado da pausa!");
    return; // Se carregou da pausa, não fazer inicialização normal
  }

  console.log("DOMContentLoaded: Estado inicial - Nível:", currentLevel, "Vidas:", vidas, "Pontuação:", pontuacao);

  
  // Inicialização normal (novo jogo ou próximo nível)
  // Verificar se estamos vindo de um nível anterior
  const proximoNivel = localStorage.getItem("proximoNivel");
  if (proximoNivel) {
    currentLevel = parseInt(proximoNivel);
    localStorage.removeItem("proximoNivel"); // Limpar após usar
    
    // Recuperar pontuação anterior se existir
    const pontuacaoAnterior = localStorage.getItem("pontuacaoAtual");
    if (pontuacaoAnterior) {
      pontuacao = parseInt(pontuacaoAnterior);
      localStorage.removeItem("pontuacaoAtual"); // Limpar após usar
      atualizarPontuacao();
    }
  }
  
  // Recuperar vidas do localStorage se existir
  const vidasSalvas = localStorage.getItem("vidas");
  if (vidasSalvas) {
    vidas = parseInt(vidasSalvas);
  } else {
    vidas = 3; // Valor padrão se não existir
  }
});

setupLevel(currentLevel); 
atualizarPontuacao();
if (localStorage.getItem("currentLevel") === "1" && localStorage.getItem("pontuacao") === "0") {
}