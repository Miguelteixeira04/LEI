clc
clear
close all;

limits = [0, 1.6];
range = limits(2) - limits(1);
set(0, 'defaultlinelinewidth', 3);

% Criar figura única
figure

% Subgráfico 1: Função e Hill Climbing
subplot(3, 1, 1); % Dividindo a figura em 3 linhas e 1 coluna
ezplot('4 * (sin(5 * pi * x + 0.5).^6) .* exp(log2((x - 0.8).^2))', limits)
ylabel('f(x)')
title('Hill Climbing - Função e Soluções')
axis([0, limits(2), -0.1, 2]);
hold on

% Gerar ponto inicial
x_current = limits(1) + rand * range;

% Chama função objeto
F_current = f1(x_current);

% Plot da solução inicial
plot(x_current, F_current, 'or', 'MarkerSize', 6);

% Plot da solução do máximo
plot(0.066, 1.6332, 'ok', 'MarkerSize', 10, 'MarkerFaceColor', 'g');

% Legenda do gráfico
legend('Função f(x)', 'Solução inicial', 'Solução do máximo', 'Location', [0.85, 0.8, 0.2, 0.1]);

xplot(1) = x_current; % Vetor guarda os valores de x
Fplot(1) = F_current; % Vetor guarda os valores de F

% Ciclo de Hill Climbing
it = 1;
num_it = 300;
while it <= num_it
    % Gerar um ponto de teste no espaço todo
    x_teste = limits(1) + rand * range;

    % Chama função objetivo
    F_teste = f1(x_teste);

    % Critério de decisão sobre se aceitamos ou não a solução nova
    if F_teste > F_current
        x_current = x_teste;
        F_current = F_teste;
        % Plot da solução nova
        plot(x_teste, F_teste, 'ok', 'MarkerSize', 6);
    end

    xplot(it) = x_current;
    Fplot(it) = F_current;

    it = it + 1;
end
hold off

% Subgráfico 2: Gráfico do y máximo
subplot(3, 1, 2);
It_plot = 1:1:num_it;
plot(It_plot, Fplot)
title('Gráfico do y máximo')
xlabel('Iterações')
ylabel('F')
axis([0 num_it 0 1.7])

% Subgráfico 3: Gráfico do x máximo
subplot(3, 1, 3);
plot(It_plot, xplot)
title('Gráfico do x máximo')
xlabel('Iterações')
ylabel('x')
axis([0 num_it 0 1.7])

