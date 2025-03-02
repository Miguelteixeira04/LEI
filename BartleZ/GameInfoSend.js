const rankinghist = []; 
const allbadges = [];

// Mapeamento das URLs das imagens dos badges
const badgeImages = {
    "Bronze": "https://i.ibb.co/6HhqWM6/Bronze.jpg",
    "Silver": "https://i.ibb.co/WHkGFH2/Badge-Silver.jpg",
    "Gold": "https://i.ibb.co/t2GYGgC/Badge-Gold.jpg",
    "Platinum": "https://i.ibb.co/5T6jFhs/Badge-Platiunium.jpg",
    "Ruby": "https://i.ibb.co/ZgJ8kDx/Badge-Ruby.jpg",
    "Diamond": "https://i.ibb.co/GJW9qVg/Badge-Diamond.jpg"
};

// Função para adicionar um rank à lista rankinghist 
function addRank(playerID, rankobtained) { 
    rankinghist.push({ playerID: playerID, name: rankobtained }); 
    updateRankingTable(); // Atualiza a tabela após adicionar um novo rank
}

// Função para adicionar um badge à lista allbadges 
function addBadge(playerID, badgeobtained, timetogain) { 
    allbadges.push({ playerID: playerID, name: badgeobtained, timeToGainBadge: timetogain });
}

// Função para verificar se o jogador já possui uma badge
function hasBadge(playerID, badgeName) {
    return allbadges.some(badge => badge.playerID === playerID && badge.name === badgeName);
}

function updateRank(playerID, score) {
    let rank = '';
    let badgeImage = '';

    if (score >= 0 && score <= 999) {
        rank = 'Bronze';
        badgeImage = badgeImages["Bronze"];
    } else if (score >= 1000 && score <= 2499) {
        rank = 'Silver';
        badgeImage = badgeImages["Silver"];
    } else if (score >= 2500 && score <= 4999) {
        rank = 'Gold';
        badgeImage = badgeImages["Gold"];
    } else if (score >= 5000 && score <= 9999) {
        rank = 'Platinum';
        badgeImage = badgeImages["Platinum"];
    } else if (score >= 10000 && score <= 19999) {
        rank = 'Ruby';
        badgeImage = badgeImages["Ruby"];
    } else if (score >= 20000) {
        rank = 'Diamond';
        badgeImage = badgeImages["Diamond"];
    }

    // Adiciona o rank ao histórico de ranks
    addRank(playerID, rank);
    
    // Se o jogador atingir um novo rank pela primeira vez, desbloqueia a badge correspondente
    if (!hasBadge(playerID, rank)) {
        unlockBadge(rank, badgeImage);
        addBadge(playerID, rank, new Date().getTime()); // Adiciona a badge ao histórico de badges
    }

    // Atualiza a imagem da medalha no HTML
    document.getElementById('rankimage').src = badgeImage;

    // Atualiza o rank final do jogador
    return rank;
}

// Atualiza a tabela com os dados do rankinghist
function updateRankingTable() {
    let tbody = document.querySelector('#rankingTable tbody');
    if (!tbody) return;
    tbody.innerHTML = ''; // Limpa a tabela antes de atualizar
    
    window.rankinghist.sort((a, b) => b.score - a.score); // Ordena por pontuação decrescente
    
    window.rankinghist.forEach((rank, index) => {
        let row = document.createElement('tr');
        row.innerHTML = `
            <td>${index + 1}</td>
            <td>${rank.name}</td>
            <td>${rank.score}</td>
            <td>${rank.rank}</td>
        `;
        // Adiciona um contorno vermelho se for o jogador atual
        if (rank.playerID === currentPlayerID) {
            row.style.border = '2px solid red';
        }
        tbody.appendChild(row);
    });
}

// Modificar addOrUpdatePlayer para atualizar a tabela automaticamente
function addOrUpdatePlayer(playerID, name, score) { 
    if (!window.rankinghist) window.rankinghist = [];
    
    let rank = '';
    if (score >= 0 && score <= 999) rank = 'Bronze';
    else if (score >= 1000 && score <= 2499) rank = 'Silver';
    else if (score >= 2500 && score <= 4999) rank = 'Gold';
    else if (score >= 5000 && score <= 9999) rank = 'Platinum';
    else if (score >= 10000 && score <= 19999) rank = 'Ruby';
    else if (score >= 20000) rank = 'Diamond';
    
    let player = window.rankinghist.find(p => p.playerID === playerID);
    if (player) {
        player.score = score;
        player.rank = rank;
    } else {
        window.rankinghist.push({ playerID, name, score, rank });
    }
    
    updateRankingTable();
}

// Função para atualizar a pontuação do jogador atual
function updateCurrentPlayerScore(playerID, score) {
    const playerName = `Player`;
    currentPlayerID = playerID; // Define o ID do jogador atual
    addOrUpdatePlayer(playerID, playerName, score);
}

// Variável global para armazenar o ID do jogador atual
let currentPlayerID = null;

// Criar a tabela ao carregar a página e adicionar jogadores fictícios
document.addEventListener('DOMContentLoaded', function () {
    if (!window.rankinghist) window.rankinghist = [];
    
    // Jogadores fictícios para cada rank
    addOrUpdatePlayer(1, "Zaidu", 500);     // Bronze
    addOrUpdatePlayer(2, "Samu", 1560);   // Silver
    addOrUpdatePlayer(3, "Aursnes", 3480);     // Gold
    addOrUpdatePlayer(4, "Arthur Cabral", 6150);   // Platinum
    addOrUpdatePlayer(5, "Marega", 12390);      // Ruby
    addOrUpdatePlayer(6, "Ronaldo", 25490); // Diamond
});

// Esta é a imagem que deverá estar no HTML que sofrerá as alterações!
// <img id="rankimage" src="bronze_medal.png" alt="Medalha do Jogador">

// Função para inicializar as badges com imagem de ponto de interrogação
function initializeBadges(badgeIDs) {
    badgeIDs.forEach(badgeID => {
        document.getElementById(badgeID).src = 'question_mark.png'; // Caminho para a imagem de ponto de interrogação
    });
}

// Função para atualizar a imagem da badge quando desbloqueada
function unlockBadge(badgeID, badgeImage) {
    document.getElementById(badgeID).src = badgeImage; // Caminho para a imagem da badge desbloqueada
}

/* Imagens das badges inciais

<img src="https://i.ibb.co/WHkGFH2/Badge-Silver.jpg" alt="Badge-Silver"> // Medalha que se ganha quando se chega ao rank "Silver"
<img src="https://i.ibb.co/t2GYGgC/Badge-Gold.jpg" alt="Badge-Gold"> // Medalha que se ganha quando se chega ao rank "Gold"
<img src="https://i.ibb.co/5T6jFhs/Badge-Platiunium.jpg" alt="Badge-Platiunium"> // Medalha que se ganha quando se chega ao rank "Platinum"
<img src="https://i.ibb.co/ZgJ8kDx/Badge-Ruby.jpg" alt="Badge-Ruby"> // Medalha que se ganha quando se chega ao rank "Ruby"
<img src="https://i.ibb.co/GJW9qVg/Badge-Diamond.jpg" alt="Badge-Diamond"> // Medalha que se ganha quando se chega ao rank "Diamond"

<img src="https://i.ibb.co/fqqpc9g/Zombie-Slayer.jpg" alt="Zombie-Slayer"> // Medalha que se ganha quando se mata 50 zombies
<img src="https://i.ibb.co/g4v1PZF/Speed-Runner.jpg" alt="Speed-Runner"> // Medalha quando se completa um nível em menos de 2 minutos

<img src="https://i.ibb.co/tBjB7h6/HIT1.jpg" alt="HIT1"> // Medalha que se ganha quando se completa a HIT 1 na dificuldade "hard".
<img src="https://i.ibb.co/NF2Mk1J/HIT2.jpg" alt="HIT2"> // Medalha que se ganha quando se completa a HIT 2 na dificuldade "hard".
<img src="https://i.ibb.co/vvw3YMk/HIT3.jpg" alt="HIT3"> // Medalha que se ganha quando se completa a HIT 3 na dificuldade "hard".

<img src="https://i.ibb.co/kmp0KvR/Survivor.jpg" alt="Survivor"> // Medalha que se ganha quando se completa um nível sem morrer
<img src="https://i.ibb.co/X8PR9zF/NoDeaths.jpg" alt="NoDeaths"> // Medalha que se ganha quando se completa o jogo sem morrer
<img src="https://i.ibb.co/hXTNF7N/Treasures.jpg" alt="Treasures"> // Medalha que se ganha quando se apanha todos os tesouros do jogo

<img id="badgeboss" src="question_mark.png" alt="Boss"> // Medalha que se ganha quando se mata o boss 

*/ 

function EndGame(event) {
    // Contador de de respostas de cada tipo
    var kcount = 0;
    var scount = 0;
    var acount = 0;
    var ecount = 0;
    
    // Análise às respostas da PTDs
    
    // As variaveis "QXanswer" (X sendo o numero da pergunta) não se encontram declaradas aqui, sãos as varáveis onde se deve guardar as respostas às PTDs do jogador
    
    // PTD1 : Explorer vs Achiever
    if(ptd1answer == "E") ecount += 1; // Incrementa o Explorer count			
    else if(ptd1answer == "A") acount += 1; // Incrementa o Achiever count	
    
    // PTD2 : Killer vs Achiever
    if(ptd2answer == "A") acount += 1; // Incrementa o Achiever count			
    else if(ptd2answer == "K") kcount += 1; // Incrementa o Killer count	
    
    // PTD3 : Killer vs Explorer
    if(ptd3answer == "K") kcount += 1;			
    else if(ptd3answer == "E") ecount += 1;
    
    // PTD4 : Socializer vs Achiever
    if(ptd4answer == "S") scount += 1;			
    else if(ptd4answer == "A") acount += 1;
    
    // PTD5 : Socialiser vs Explorer
    if(ptd5answer == "S") scount += 1;			
    else if(ptd5answer == "E") ecount += 1;
    
    // PTD6 : Socializer vs Killer
    if(ptd6answer == "K") kcount += 1;			
    else if(ptd6answer == "S") scount += 1;
    
    // Cálculo das percentagens
    var KPercentage = (kcount / 6) * 100;
    var APercentage = (acount / 6) * 100;
    var SPercentage = (scount / 6) * 100;
    var EPercentage = (ecount / 6) * 100;
    
    // Apresentação na consola
    console.log("Killer: " + KPercentage + "%");
    console.log("Achiever: " + APercentage + "%");
    console.log("Socializer: " + SPercentage + "%");
    console.log("Explorer: " + EPercentage + "%");
    
    // Calcular a maior percentagem para descobrir o Dominant Type
    const highest = Math.max(KPercentage, APercentage, SPercentage, EPercentage);
    
    var dominanttype = "";
    
    // Atribuição do tipo dominante
    if(highest == KPercentage) dominanttype = dominanttype + "Killer";
    if(highest == APercentage) dominanttype = dominanttype + "-Achiever";
    if(highest == SPercentage) dominanttype = dominanttype + "-Socializer";
    if(highest == EPercentage) dominanttype = dominanttype + "-Explorer";
    
    //Apresentação do tipo de jogador dominante ao jogador
    console.log("You are a: " + dominanttype + " with " + highest + "%");
    alert("You are a: " + dominanttype + " with " + highest + "%");

    // Lista da info das PTDs
    const ptds = [{
            playerID: pNum, // Numero do jogador
            questionID: 1, // Id da PTD
            answer: ptd1answer, // Resposta à PTD1
            timeOnDecision: ptd1time // Tempo na PTD1
        }, {
            playerID: pNum, // Numero do jogador
            questionID: 2, // Id da PTD
            answer: ptd2answer, // Resposta à PTD2
            timeOnDecision: ptd2time // Tempo na PTD2
        }, {
            playerID: pNum, // Numero do jogador
            questionID: 3, // Id da PTD
            answer: ptd3answer, // Resposta à PTD3
            timeOnDecision: ptd3time // Tempo na PTD3
        }, {
            playerID: pNum, // Numero do jogador
            questionID: 4, // Id da PTD
            answer: ptd4answer, // Resposta à PTD4
            timeOnDecision: ptd4time // Tempo na PTD4
        }, {
            playerID: pNum, // Numero do jogador
            questionID: 5, // Id da PTD
            answer: ptd5answer, // Resposta à PTD5
            timeOnDecision: ptd5time // Tempo na PTD5
        }, {
            playerID: pNum, // Numero do jogador
            questionID: 6, // Id da PTD
            answer: ptd1answer, // Resposta à PTD6
            timeOnDecision: ptd1time // Tempo na PTD6
        }];
        
        // Lista da Info das CVCs
        const cvcs = [{ 
        playerID: pNum, // Numero do jogador
        questionID: 1, // Id da CvC
        nature: cvc1nature, // Natureza da resposta: "Comp" para resposta (A) e "Coop" para resposta (B)
        answer: cvc1answer, // Resposta à CvC1
        timeOnDecision: cvc1time // Tempo na CvC1
        }, { 
        playerID: pNum, // Numero do jogador
        questionID: 2, // Id da CvC
        nature: cvc2nature, // Natureza da resposta: "Comp" para resposta (A) e "Coop" para resposta (B)
        answer: cvc2answer, // Resposta à CvC2
        timeOnDecision: cvc2time // Tempo na CvC2
        }, { 
        playerID: pNum, // Numero do jogador
        questionID: 3, // Id da CvC
        nature: cvc3nature, // Natureza da resposta: "Comp" para resposta (A) e "Coop" para resposta (B)
        answer: cvc3answer, // Resposta à CvC3
        timeOnDecision: cvc3time // Tempo na CvC3
        }, { 
        playerID: pNum, // Numero do jogador
        questionID: 4, // Id da CvC
        nature: cvc4nature, // Natureza da resposta: "Comp" para resposta (A) e "Coop" para resposta (B)
        answer: cvc4answer, // Resposta à CvC4
        timeOnDecision: cvc4time // Tempo na CvC4
        }, { 
        playerID: pNum, // Numero do jogador
        questionID: 5, // Id da CvC
        nature: cvc5nature, // Natureza da resposta: "Comp" para resposta (A) e "Coop" para resposta (B)
        answer: cvc5answer, // Resposta à CvC5
        timeOnDecision: cvc5time // Tempo na CvC5
        }, { 
        playerID: pNum, // Numero do jogador
        questionID: 6, // Id da CvC
        nature: cvc6nature, // Natureza da resposta: "Comp" para resposta (A) e "Coop" para resposta (B)
        answer: cvc6answer, // Resposta à CvC6
        timeOnDecision: cvc6time // Tempo na CvC6
        }];
        
        // Lista da Info das HITs
        const hits = [{ 
        playerID: pNum, // Numero do jogador
        questionID: 1, // Id da HIT
        difficultyChosen: hit1diff, // Dificuldade que o jogador escolheu para fazer a HIT
        correct: hit1correct, // Variável booleana que indica se ele acertou ou não
        answer: hit1answer, // Resposta à HIT
        pointsGathered: hit1pointsgained, // Pontos ganhos nesta HIT
        timeOnDecision: hit1time // Tempo de realização da HIT
        }, { 
        playerID: pNum, // Numero do jogador
        questionID: 2, // Id da HIT
        difficultyChosen: hit2diff, // Dificuldade que o jogador escolheu para fazer a HIT
        correct: hit2correct, // Variável booleana que indica se ele acertou ou não
        answer: hit2answer, // Resposta à HIT
        pointsGathered: hit2pointsgained, // Pontos ganhos nesta HIT
        timeOnDecision: hit2time // Tempo de realização da HIT
        }, { 
        playerID: pNum, // Numero do jogador
        questionID: 3, // Id da HIT
        difficultyChosen: hit3diff, // Dificuldade que o jogador escolheu para fazer a HIT
        correct: hit3correct, // Variável booleana que indica se ele acertou ou não
        answer: hit3answer, // Resposta à HIT
        pointsGathered: hit3pointsgained, // Pontos ganhos nesta HIT
        timeOnDecision: hit3time // Tempo de realização da HIT
        },];
        
        // Historico de ranks que ele teve no jogo
        /* ESTA LISTA NÃO DEVE SER CRIADA AQUI MAS SIM NO INICIO E PREENCHIDA AO LONGO DO JOGO
        const rankinghist = [esta lista deverá ser preechida ao longo do jogo objetos com esta especificação
        /*
        { 
        playerID: pNum, // Numero do jogador
        name: rankobtained // Rank obtido
        } 
        ]; */
    
        // Bages que ele teve no jogo
        /* ESTA LISTA NÃO DEVE SER CRIADA AQUI MAS SIM NO INICIO E PREENCHIDA AO LONGO DO JOGO
        const allbadges = [esta lista deverá ser preechida ao longo do jogo objetos com esta especificação
        /*
        { 
        playerID: pNum, // Numero do jogador
        name: badgeobtained, // Badge conquistado
        timeToGainBadge: timetogain // Tempo que demorou a conquistar o badge
        } 
        ]; */

    const data = { 
    pNumber: pNum, // variável do numero do jogador 
    totalScore: scoretotal,  // variável do numero do jogador 
    totalTime: timetotal, // variável do tempo total de jogo (segundos)
    timeSpentRoom1: timeroom1, // variável do tempo total no nível 1: Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!)
    timeSpentRoom2: timeroom2, // variável do tempo total no nível 2: Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!)
    timeSpentRoom3: timeroom3, // variável do tempo total no nível 3: Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!)
    timeSpentRoom4: timeroom4, // variável do tempo total no nível 4 : Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!)
    timeSpentRoom5: timeroom5, // variável do tempo total no nível 5: Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!) 
    timeSpentRoom6: timeroom6, // variável do tempo total no nível 6: Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!)
    timeSpentRoom7: timeroom7, // variável do tempo total no nível 7: Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!)
    timeSpentRoom8: timeroom8, // variável do tempo total no nível 8: Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!)
    timeSpentRoom9: timeroom9, // variável do tempo total no nível 9: Desde que começa o nível até que entra na porta de saída (em segundos) (se voltar ao nível, não conta!)
    timeOnLevelX: timeroomX, // variável do tempo total no nível 5: (Caso escolha essa opção na CvC 4!) (em segundos) (se voltar ao nível, não conta!)
    numberOfReturns: nreturns, // variável numerica do número de vezes que voltou a níveis completos
    numberOfDeaths: ndeaths, // variável numerica do número de mortes
    pTDAnswers: ptds, // Respostas às PTDs (lista acima especificada)
    cvCAnswers: cvcs, // Respostas aos CVCs (lista acima especificada)
    hITAnswers: hits, // Respostas às HITs (lista acima especificada)
    positionOnLeaderboard: position, // posição com que ele terminou o jogo
    finalRank: rank, // rank com que ele terminou o jogo
    rankingHistory: rankinghist, // historico de ranks que ele teve no jogo, tanto progressões como regressões (lista acima especificada)
    numberOfBadges: numbadge, // numero total de badges conquistadas
    badges: allbadges // Lista de badges conquistadas (lista acima especificada)
    };

    // Send POST request with JSON body
    // O URL vamos alterar com base no domínio que colocarmos!
    fetch('http://localhost:5087/api/Values/ScaledQuiz30', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    });
}
