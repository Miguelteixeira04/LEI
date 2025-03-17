clc
clear
close all;

figure;
set(gcf, 'Units', 'normalized', 'OuterPosition', [0 0 1 1]);

% Limites e definição do intervalo da função
limits = [0, 1.6];
range = limits(2) - limits(1);
set(0, 'defaultlinelinewidth', 3);

% Gráfico da função no primeiro subplot
subplot(3, 1, 1); % 3 linhas, 1 coluna, primeira posição
ezplot('4 * (sin(5 * pi * x + 0.5).^6) .* exp(log2((x - 0.8).^2))', limits)
ylabel('f(x)')
title('Função f(x)')
axis([0, limits(2), -0.1, 2]);
hold on

% Inicialização do ponto inicial
x_current = limits(1) + rand * range;

% Avaliação da função objetivo
F_current = f1(x_current);

% Plot da solução inicial e do máximo global conhecido
plot(x_current, F_current, 'or', 'MarkerSize', 6);
plot(0.065182, 1.6333, 'ok', 'MarkerSize', 10, 'MarkerFaceColor', 'g');
legend('Função f(x)', 'Solução inicial', 'Solução do máximo','Location', [0.95, 0.65, 0.01, 0.01]);

% Vetores para guardar os valores de x e F
xplot(1) = x_current; 
Fplot(1) = F_current; 

% Parâmetros de iteração e contagem de estagnação
it = 1;
num_it = 300;
max_no_improvement = 25;  % Número máximo de iterações sem melhora antes da reinicialização
no_improvement_count = 0;  % Contador de iterações sem melhora
step_size = 0.02 * range;  % Passo máximo permitido

% Loop do Hill Climbing com reinicialização MRGC
while it <= num_it
    % Gerar um ponto de teste aleatório dentro de 0.02 da amplitude do espaço de pesquisa
    x_teste = x_current + (rand * 2 - 1) * step_size; % Intervalo [x_current - step_size, x_current + step_size]
    x_teste = max(min(x_teste, limits(2)), limits(1)); % Garantir que o ponto esteja nos limites

    % Avaliação da função objetivo no ponto de teste
    F_teste = f1(x_teste);

    % Verificação de melhora na função objetivo
    if F_teste > F_current
        x_current = x_teste;
        F_current = F_teste;
        plot(x_teste, F_teste, 'ok', 'MarkerSize', 6);
        no_improvement_count = 0;  % Reset ao contador se houver melhora
    else
        no_improvement_count = no_improvement_count + 1;
    end

    % Armazenamento dos valores da iteração
    xplot(it) = x_current;
    Fplot(it) = F_current;

    % Verificação para reinicialização MRHC
    if no_improvement_count >= max_no_improvement
        x_current = limits(1) + rand * range;  % Novo ponto aleatório
        F_current = f1(x_current);  % Reavaliação no novo ponto
        no_improvement_count = 0;  % Reset do contador de estagnação
        plot(x_current, F_current, 'or', 'MarkerSize', 6);  % Plot do novo ponto inicial
    end

    it = it + 1;
end

hold off

% Gráficos dos resultados
It_plot = 1:1:num_it;

% Gráfico do máximo de F no segundo subplot
subplot(3, 1, 2); % 3 linhas, 1 coluna, segunda posição
plot(It_plot, Fplot, 'b-', 'LineWidth', 1.5);
title('Gráfico do y máximo');
xlabel('Iterações');
ylabel('F');
axis([0 num_it 0 1.7]);

% Gráfico do máximo de x no terceiro subplot
subplot(3, 1, 3); % 3 linhas, 1 coluna, terceira posição
plot(It_plot, xplot, 'r-', 'LineWidth', 1.5);
title('Gráfico do x máximo');
xlabel('Iterações');
ylabel('x');
axis([0 num_it 0 1.7]);

hold off
