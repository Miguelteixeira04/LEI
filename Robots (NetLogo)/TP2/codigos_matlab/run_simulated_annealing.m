% Valores de alfa
alphas = [0.86, 0.90, 0.94]; % Três valores de alfa
T_inicial = 100; % Temperatura inicial
max_iter = 300;  % Número total de iterações
nRep = 500;             % Número de repetições para cada valor de T

% Executar Simulated Annealing para alfa = 0.90
[x_best, f_best, x_history, f_history, temperatures, probabilities, iteracoes] = simulated_annealing(0.90);

% Exibir o melhor valor encontrado
fprintf('Melhor valor de x: %.4f\n', x_best);
fprintf('Melhor valor de f(x): %.4f\n', f_best);

% Calcular temperaturas para diferentes valores de alfa
temperatures_all = calculate_temperatures(alphas, T_inicial, max_iter);

% Gráfico da função f(x) com o máximo encontrado
x_vals = linspace(0, 1.6, 1000); % Intervalo para x
f_vals = arrayfun(@f1, x_vals);   % Avaliar f1 em cada valor de x

figure;

% Gráfico 1: Função f(x) e o máximo encontrado
subplot(3, 1, 1); % Subplot 1
plot(x_vals, f_vals, 'b-', 'LineWidth', 1.5); % Linha azul para f(x)
hold on;
plot(x_best, f_best, 'ro', 'MarkerSize', 8, 'MarkerFaceColor', 'r'); % Máximo
xlabel('x');
ylabel('f(x)');
title('Gráfico da função f(x) com o máximo encontrado');
legend('f(x)', 'Máximo encontrado', 'Location', 'Best');
grid on;

% Gráfico 2: Temperatura inicial em função das iterações
subplot(3, 1, 2); % Subplot 2
hold on;
for j = 1:length(alphas)
    plot(1:max_iter, temperatures_all(:, j), 'LineWidth', 1.5);
end
xlabel('Iterações');
ylabel('Temperatura');
title('Temperatura em função das iterações para diferentes valores de \alpha');
legend(arrayfun(@(alpha) sprintf('\\alpha = %.2f', alpha), alphas, 'UniformOutput', false), 'Location', 'Best');
grid on;
hold off;

% Gráfico 3: Probabilidade de Boltzmann em função das iterações
subplot(3, 1, 3); % Subplot 3
probabilities_boltzmann = calculate_boltzmann_probabilities(f_history, f_best, temperatures_all(:, 2), max_iter);

% Plotar o gráfico
plot(1:max_iter, probabilities_boltzmann, 'b-', 'LineWidth', 1.5); % Linha azul para probabilidade
xlabel('Iterações');
ylabel('Probabilidade');
title('Probabilidade de Boltzmann ao longo das Iterações');
grid on;

% Funções auxiliares para melhorar o código
function temperatures_all = calculate_temperatures(alphas, T_inicial, max_iter)
    temperatures_all = zeros(max_iter, length(alphas));
    for j = 1:length(alphas)
        T = T_inicial;
        for iter = 1:max_iter
            temperatures_all(iter, j) = T;
            T = T * alphas(j); % Atualizar a temperatura
        end
    end
end

function probabilities_boltzmann = calculate_boltzmann_probabilities(f_history, f_best, temperatures, max_iter)
    probabilities_boltzmann = zeros(1, max_iter);
    for k = 1:max_iter
        dE = abs(f_history(k) - f_best);  % Delta E = diferença de energia
        probabilities_boltzmann(k) = exp(-dE / temperatures(k)); % Probabilidade de Boltzmann
    end
end
