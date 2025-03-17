disp('Por quantas cidades Portuguesas o Caixeiro Viajante passa?');
disp('a) 14 cidades'); 
disp('b) 20 cidades');
disp('c) 30 cidades');
resposta = input('Escolha a opção (a, b ou c): ', 's');

% Determina o número de cidades baseado na escolha
switch resposta
    case 'a'
        nCidades = 14;
    case 'b'
        nCidades = 20;
    case 'c'
        nCidades = 30;
    otherwise
        disp('Opção inválida.');
        return;
end

try
    [cidades, nomesCidades] = cidadesEscolhidas(resposta);
catch ME
    disp(ME.message);
    return;
end

% Função para calcular a distância entre duas cidades (usando fórmula da esfera)
distancia = @(lat1, lon1, lat2, lon2) ...
    6371 * acos(sin(deg2rad(lat1)) .* sin(deg2rad(lat2)) + ...
    cos(deg2rad(lat1)) .* cos(deg2rad(lat2)) .* cos(deg2rad(lon2 - lon1)));

% Matriz de distâncias
matrizDist = zeros(nCidades);

for i = 1:nCidades
    for j = 1:nCidades
        if i ~= j
            matrizDist(i, j) = distancia(cidades(i, 1), cidades(i, 2), ...
                                         cidades(j, 1), cidades(j, 2));
        end
    end
end

% Função de custo (distância total para um dado percurso)
custo = @(rota) sum(arrayfun(@(i) matrizDist(rota(i), rota(mod(i, nCidades) + 1)), 1:nCidades));

% Configuração do algoritmo Simulated Annealing
T = 1000;          % Temperatura inicial
T_min = 1e-3;      % Temperatura mínima
alpha = 0.88;      % Taxa de resfriamento
iter = 50;         % Iterações por temperatura

% Inicialização dos gráficos
temperaturas = [];  % Para armazenar a temperatura ao longo das iterações
probabilidades = [];  % Para armazenar as probabilidades de Boltzmann

% Inicialização
rotaAtual = randperm(nCidades);  % Rota inicial (aleatória)
melhorRota = rotaAtual;
melhorCusto = custo(melhorRota);

while T > T_min
    for k = 1:iter
        % Gerar nova solução (troca de duas cidades)
        novaRota = rotaAtual;
        idx = randperm(nCidades, 2);
        novaRota(idx) = novaRota(flip(idx));

        % Calcular custos
        custoAtual = custo(rotaAtual);
        novoCusto = custo(novaRota);

        % Calcular a diferença de custo (energia)
        deltaE = novoCusto - custoAtual;  % Diferença de custo (energia)
        
        % Calcular a probabilidade de Boltzmann
        if deltaE < 0
            probabilidade = 1;  % Aceita automaticamente se a solução for melhor
        else
            probabilidade = exp(-deltaE / T);  % Probabilidade de Boltzmann
        end
        
        % Armazenar a probabilidade de Boltzmann para posterior visualização
        probabilidades = [probabilidades; probabilidade];

        % Aceitar ou rejeitar nova solução
        if novoCusto < custoAtual || rand < probabilidade
            rotaAtual = novaRota;
        end

        % Atualizar melhor solução
        if custo(rotaAtual) < melhorCusto
            melhorRota = rotaAtual;
            melhorCusto = custo(melhorRota);
        end
    end

    % Armazenar a temperatura para cada iteração
    temperaturas = [temperaturas, T];

    % Reduzir temperatura
    T = T * alpha;
end

% Exibir a melhor rota
rotaNomes = nomesCidades(melhorRota); % Substitui índices pelos nomes
fprintf('Melhor rota encontrada entre as %d cidades: \n', nCidades);

% Exibir as cidades em linhas com no máximo 6 cidades por linha
for i = 1:nCidades
    if mod(i-1, 6) == 0 && i > 1
        fprintf('\n');  % Nova linha após 6 cidades
    end
    fprintf('%s', rotaNomes{i});
    if mod(i, 6) ~= 0 && i ~= nCidades
        fprintf(' -> ');
    end
end

fprintf('\nCusto da melhor rota = %.2f km\n', melhorCusto);

% Nome do arquivo onde os resultados serão salvos
nomeArquivo = 'melhores_resultados.txt';

% Abrir o arquivo para leitura para verificar se já existem entradas para o número de cidades
fileID = fopen(nomeArquivo, 'r');
if fileID == -1
    % Se o arquivo não existe, criar um novo
    fileID = fopen(nomeArquivo, 'w');
    fclose(fileID);
end

% Verificar se o arquivo já contém um resultado para o número de cidades
fileID = fopen(nomeArquivo, 'r');
conteudoArquivo = fread(fileID, '*char')';
fclose(fileID);

% Verificar se já existe uma entrada para o número de cidades escolhido
if contains(conteudoArquivo, sprintf('%d cidades', nCidades))
    % Procurar o custo existente no registo
    padrao = sprintf('Melhor resultado para %d cidades:\\n.*?Custo: ([0-9]+\\.?[0-9]*) km', nCidades);
    tokens = regexp(conteudoArquivo, padrao, 'tokens');

    if ~isempty(tokens)
        custoExistente = str2double(tokens{1}{1});
        
        if melhorCusto < custoExistente
            % Substituir o registo antigo pelo novo
            conteudoArquivoAtualizado = regexprep(conteudoArquivo, ...
                sprintf('Melhor resultado para %d cidades:\\n.*?----------------------------------------\\n', nCidades), '');

            % Reescrever o conteúdo atualizado sem o registo antigo
            fileID = fopen(nomeArquivo, 'w');
            fwrite(fileID, conteudoArquivoAtualizado);
            fclose(fileID);

            % Adicionar o novo registo
            fileID = fopen(nomeArquivo, 'a');
            fprintf(fileID, 'Melhor resultado para %d cidades:\n', nCidades);
            fprintf(fileID, 'Rota: ');
            for i = 1:nCidades
                fprintf(fileID, '%s', rotaNomes{i});
                if i ~= nCidades
                    fprintf(fileID, ' -> ');
                end
            end
            fprintf(fileID, '\nCusto: %.2f km\n', melhorCusto);
            fprintf(fileID, '----------------------------------------\n');
            fclose(fileID);

            disp(['O registo para ', num2str(nCidades), ' cidades foi atualizado com um menor custo no arquivo: ', nomeArquivo]);
        else
            disp(['O custo registado para ', num2str(nCidades), ' cidades já é menor ou igual. Nenhuma atualização foi feita.']);
        end
    else
        disp('Erro ao processar o arquivo: registo inválido.');
    end
else
    % Adicionar o novo registo se não existir
    fileID = fopen(nomeArquivo, 'a');
    fprintf(fileID, 'Melhor resultado para %d cidades:\n', nCidades);
    fprintf(fileID, 'Rota: ');
    for i = 1:nCidades
        fprintf(fileID, '%s', rotaNomes{i});
        if i ~= nCidades
            fprintf(fileID, ' -> ');
        end
    end
    fprintf(fileID, '\nCusto: %.2f km\n', melhorCusto);
    fprintf(fileID, '----------------------------------------\n');
    fclose(fileID);

    disp(['O melhor resultado para ', num2str(nCidades), ' cidades foi salvo no arquivo: ', nomeArquivo]);
end


% Gráfico com subplots
figure;

% Subplot 1: Temperatura em função das iterações
subplot(2, 1, 1);
plot(temperaturas, 'o-', 'LineWidth', 2, 'MarkerSize', 2);
title('Temperatura em Função das Iterações');
xlabel('Iterações');
ylabel('Temperatura');
grid on;

% Suavizar as probabilidades de Boltzmann usando média móvel
janela = 50; % Tamanho da janela para suavização
probabilidades_suavizadas = movmean(probabilidades, janela);

% Subplot 2: Probabilidade de Boltzmann suavizada ao longo das iterações
subplot(2, 1, 2);
iteracoes = 1:length(probabilidades_suavizadas);
scatter(iteracoes, probabilidades_suavizadas, 1, 'b', 'filled'); % Pontos pequenos
title('Probabilidade de Boltzmann ao longo das Iterações');
xlabel('Iterações');
ylabel('Probabilidade');
grid on;

% Ajustar layout da figura
sgtitle('Análise do Algoritmo Simulated Annealing');

% Adicionar o ponto inicial ao final para criar um circuito fechado
rotaCircuito = [melhorRota, melhorRota(1)];

% Plotar rota com linhas vermelhas e estrelas amarelas
figure;
geoplot(cidades(rotaCircuito, 1), cidades(rotaCircuito, 2), '-r', ... % Linhas vermelhas
    'LineWidth', 2, 'Marker', '*', 'MarkerSize', 8, ... % Estrelas amarelas
    'MarkerEdgeColor', 'y', 'MarkerFaceColor', 'y'); 

% Adicionar nomes das cidades
for i = 1:nCidades
    text(cidades(i, 1), cidades(i, 2), nomesCidades{i}, 'FontSize', 8, 'Color', 'black', 'FontWeight', 'bold', 'VerticalAlignment', 'bottom', 'HorizontalAlignment', 'right');
end

% Ajustar título e limites
title('Melhor Rota Encontrada entre as cidades');
geolimits([min(cidades(:, 1)) max(cidades(:, 1))], [min(cidades(:, 2)) max(cidades(:, 2))]);

% Alterar mapa de fundo para colorido
geobasemap('streets');