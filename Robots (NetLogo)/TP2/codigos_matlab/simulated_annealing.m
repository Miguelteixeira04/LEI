function [x_best, f_best, x_history, f_history, temperatures, probabilities, iteracoes] = simulated_annealing(alfa)
    % Parâmetros iniciais do Simulated Annealing
    T = 100;                % Temperatura inicial
    nRep = 500;             % Número de repetições para cada valor de T
    max_iter = 300;         % Número total de iterações
    x_min = 0;
    x_max = 1.6;
    delta_x = 0.02 * (x_max - x_min); % Passo máximo de movimento

    % Inicialização
    x_current = x_min + (x_max - x_min) * rand; % Ponto inicial aleatório
    f_current = f1(x_current);
    x_best = x_current;
    f_best = f_current;

    % Vetores para salvar o histórico
    x_history = zeros(max_iter * nRep, 1);
    f_history = zeros(max_iter * nRep, 1);
    temperatures = zeros(max_iter, 1);         % Armazenar temperaturas
    probabilities = zeros(max_iter * nRep, 1); % Armazenar probabilidades
    iteracoes = zeros(max_iter * nRep, 1);     % Número de iteração para cada ponto
    step = 1; % Índice para salvar as iterações

    for iter = 1:max_iter
        for i = 1:nRep
            % Perturbação aleatória no intervalo [-delta_x, delta_x]
            x_new = x_current + delta_x * (2 * rand - 1);
            x_new = max(x_min, min(x_max, x_new)); % Garantir que x_new está no intervalo

            % Avaliação da nova solução
            f_new = f1(x_new);
            dE = f_new - f_current; % Gradiente de energia

            % Probabilidade de Boltzmann
            prob = exp(dE / T);

            % Critério de aceitação
            if dE > 0 || prob > rand
                x_current = x_new;
                f_current = f_new;
            end

            % Atualização do melhor valor encontrado
            if f_current > f_best
                x_best = x_current;
                f_best = f_current;
            end

            % Armazenar histórico
            x_history(step) = x_current;
            f_history(step) = f_current;
            probabilities(step) = prob; % Salvar probabilidade
            iteracoes(step) = (iter - 1) * nRep + i; % Número da iteração global
            step = step + 1;
        end

        % Salvar temperatura atual
        temperatures(iter) = T;

        % Atualização da temperatura
        T = T * alfa;
    end

    % Reduzir os vetores ao tamanho usado
    x_history = x_history(1:step-1);
    f_history = f_history(1:step-1);
    probabilities = probabilities(1:step-1);
    iteracoes = iteracoes(1:step-1);
end
