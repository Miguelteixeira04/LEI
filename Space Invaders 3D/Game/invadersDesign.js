// Criar invasor vermelho 
function createRedInvader(scene, posX, posY, posZ) {
    const invaderGroup = new BABYLON.TransformNode("redInvader", scene)
  
    // Design do invasor vermelho 
    const invaderDesign = [
      [0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0],
      [0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0],
      [0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0],
      [0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0],
      [0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0],
      [0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0],
      [0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0],
      [0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0],
    ]
  
    // Material vermelho
    const invaderMaterial = new BABYLON.StandardMaterial("redMaterial", scene)
    invaderMaterial.diffuseColor = new BABYLON.Color3(1, 0, 0) // Vermelho
    invaderMaterial.specularColor = new BABYLON.Color3(1, 0.3, 0.3)
  
    // Material vermelho mais escuro para profundidade
    const invaderMaterialDarker = new BABYLON.StandardMaterial("redMaterialDarker", scene)
    invaderMaterialDarker.diffuseColor = new BABYLON.Color3(0.8, 0, 0) // Vermelho escuro -- para a profundidade
    invaderMaterialDarker.specularColor = new BABYLON.Color3(0.8, 0.2, 0.2)
  
    // Tamanho de cada bloco
    const blockSize = 0.5
    const depth = blockSize
  
    // Criar blocos para formar o invasor
    for (let y = 0; y < invaderDesign.length; y++) {
      for (let x = 0; x < invaderDesign[y].length; x++) {
        if (invaderDesign[y][x] === 1) {
          const cube = BABYLON.MeshBuilder.CreateBox(
            "cube",
            {
              width: blockSize,
              height: blockSize,
              depth: depth,
            },
            scene,
          )
  
          // Aplicar material
          if (y === invaderDesign.length - 1 || x === 0 || x === invaderDesign[y].length - 1) {
            cube.material = invaderMaterialDarker
          } else {
            cube.material = invaderMaterial
          }
  
          // Posicionar o cubo
          cube.position.x = x * blockSize - (invaderDesign[y].length * blockSize) / 2 + blockSize / 2
          cube.position.y = (invaderDesign.length - y) * blockSize - (invaderDesign.length * blockSize) / 2
          cube.position.z = 0
  
          // Adicionar ao grupo
          cube.parent = invaderGroup
        }
      }
    }
  
    // Posicionar o grupo
    invaderGroup.position.x = posX
    invaderGroup.position.y = posY
    invaderGroup.position.z = posZ
  
    return invaderGroup
  }
  
  // Criar invasor amarelo
  function createYellowInvader(scene, posX, posY, posZ) {
    const invaderGroup = new BABYLON.TransformNode("yellowInvader", scene)
  
    // Design do invasor amarelo 
    const invaderDesign = [
      [0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0],
      [0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0],
      [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
      [0, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0],
      [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
      [0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0],
      [0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0],
      [0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0],
    ]
  
    // Material amarelo
    const invaderMaterial = new BABYLON.StandardMaterial("yellowMaterial", scene)
    invaderMaterial.diffuseColor = new BABYLON.Color3(1, 1, 0) // Amarelo
    invaderMaterial.specularColor = new BABYLON.Color3(1, 1, 0.3)
  
    // Material amarelo mais escuro para profundidade
    const invaderMaterialDarker = new BABYLON.StandardMaterial("yellowMaterialDarker", scene)
    invaderMaterialDarker.diffuseColor = new BABYLON.Color3(0.8, 0.8, 0) // Amarelo escuro -- para a profundidade
    invaderMaterialDarker.specularColor = new BABYLON.Color3(0.8, 0.8, 0.2)
  
    // Tamanho de cada bloco
    const blockSize = 0.4
    const depth = blockSize
  
    // Criar blocos para formar o invasor
    for (let y = 0; y < invaderDesign.length; y++) {
      for (let x = 0; x < invaderDesign[y].length; x++) {
        if (invaderDesign[y][x] === 1) {
          const cube = BABYLON.MeshBuilder.CreateBox(
            "cube",
            {
              width: blockSize,
              height: blockSize,
              depth: depth,
            },
            scene,
          )
  
          // Aplicar material
          if (y === invaderDesign.length - 1 || x === 0 || x === invaderDesign[y].length - 1) {
            cube.material = invaderMaterialDarker
          } else {
            cube.material = invaderMaterial
          }
  
          // Posicionar o cubo
          cube.position.x = x * blockSize - (invaderDesign[y].length * blockSize) / 2 + blockSize / 2
          cube.position.y = (invaderDesign.length - y) * blockSize - (invaderDesign.length * blockSize) / 2
          cube.position.z = 0
  
          // Adicionar ao grupo
          cube.parent = invaderGroup
        }
      }
    }
  
    // Posicionar o grupo
    invaderGroup.position.x = posX
    invaderGroup.position.y = posY
    invaderGroup.position.z = posZ
  
    return invaderGroup
  }
  
  // Criar invasor rosa 
  function createPinkInvader(scene, posX, posY, posZ) {
    const invaderGroup = new BABYLON.TransformNode("pinkInvader", scene)
  
    // Design do invasor rosa 
    const invaderDesign = [
      [0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0],
      [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
      [0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0],
      [1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1],
      [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1],
      [0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0],
      [0, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 0],
      [1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1],
    ]
  
    // Material rosa
    const invaderMaterial = new BABYLON.StandardMaterial("pinkMaterial", scene)
    invaderMaterial.diffuseColor = new BABYLON.Color3(1, 0.41, 0.7) // Rosa (FF69B4)
    invaderMaterial.specularColor = new BABYLON.Color3(1, 0.6, 0.9)
  
    // Material rosa mais escuro para profundidade
    const invaderMaterialDarker = new BABYLON.StandardMaterial("pinkMaterialDarker", scene)
    invaderMaterialDarker.diffuseColor = new BABYLON.Color3(0.9, 0.23, 0.65) // Rosa escuro -- para profundidade
    invaderMaterialDarker.specularColor = new BABYLON.Color3(0.8, 0.4, 0.7)
  
    // Tamanho de cada bloco
    const blockSize = 0.3
    const depth = blockSize
  
    // Criar blocos para formar o invasor
    for (let y = 0; y < invaderDesign.length; y++) {
      for (let x = 0; x < invaderDesign[y].length; x++) {
        if (invaderDesign[y][x] === 1) {
          const cube = BABYLON.MeshBuilder.CreateBox(
            "cube",
            {
              width: blockSize,
              height: blockSize,
              depth: depth,
            },
            scene,
          )
  
          // Aplicar material
          if (y === invaderDesign.length - 1 || x === 0 || x === invaderDesign[y].length - 1) {
            cube.material = invaderMaterialDarker
          } else {
            cube.material = invaderMaterial
          }
  
          // Posicionar o cubo
          cube.position.x = x * blockSize - (invaderDesign[y].length * blockSize) / 2 + blockSize / 2
          cube.position.y = (invaderDesign.length - y) * blockSize - (invaderDesign.length * blockSize) / 2
          cube.position.z = 0
  
          // Adicionar ao grupo
          cube.parent = invaderGroup
        }
      }
    }
  
    // Posicionar o grupo
    invaderGroup.position.x = posX
    invaderGroup.position.y = posY
    invaderGroup.position.z = posZ
  
    return invaderGroup
  }

  // Criar Barreira
function createBarrier(scene, posX, posY, posZ) {
    const barrierGroup = new BABYLON.TransformNode("barrier", scene)
  
    // Design da barreira
    const barrierDesign = [
      [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
      [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
      [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
      [0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0],
      [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
      [0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0],
      [0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0],
      [0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0],
    ]

    // Material da barreira verde
    const barrierMaterial = new BABYLON.StandardMaterial("barrierMaterial", scene)
    barrierMaterial.diffuseColor = new BABYLON.Color3(0, 1, 0) // Verde
    barrierMaterial.specularColor = new BABYLON.Color3(0.3, 1, 0.3)

    // Material da barreira verde mais escuro para profundidade
    const barrierMaterialDarker = new BABYLON.StandardMaterial("barrierMaterialDarker", scene)
    barrierMaterialDarker.diffuseColor = new BABYLON.Color3(0, 0.8, 0) // Verde escuro -- para profundidade
    barrierMaterialDarker.specularColor = new BABYLON.Color3(0.2, 0.8, 0.2)

    // Tamanho de cada bloco
    const blockSize = 0.75
    const depth = blockSize

    // Criar blocos para formar a barreira
    for (let y = 0; y < barrierDesign.length; y++) {
      for (let x = 0; x < barrierDesign[y].length; x++) {
        if (barrierDesign[y][x] === 1) {
          const cube = BABYLON.MeshBuilder.CreateBox(
            "cube",
            {
              width: blockSize,
              height: blockSize,
              depth: depth,
            },
            scene,
          )

          // Aplicar material
          if (y === barrierDesign.length - 1 || x === 0 || x === barrierDesign[y].length - 1) {
            cube.material = barrierMaterialDarker
          } else {
            cube.material = barrierMaterial
          }

          // Posicionar o cubo
          cube.position.x = x * blockSize - (barrierDesign[y].length * blockSize) / 2 + blockSize / 2
          cube.position.y = (barrierDesign.length - y) * blockSize - (barrierDesign.length * blockSize) / 2
          cube.position.z = 0

          // Adicionar ao grupo
          cube.parent = barrierGroup
        }
      }
    }

    // Posicionar o grupo
    barrierGroup.position.x = posX
    barrierGroup.position.y = posY
    barrierGroup.position.z = posZ

    return barrierGroup
  }