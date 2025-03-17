function [cidades, nomesCidades] = cidadesEscolhidas(opcao)
    switch opcao
        case 'a'
            % 14 cidades
            cidades = [
                41.8167, -6.7500;  % Bragança
                41.3000, -7.7500;  % Vila Real
                41.7333, -7.4667;  % Chaves
                41.7000, -8.8333;  % Viana do Castelo
                41.5500, -8.4333;  % Braga
                40.6333, -8.6500;  % Aveiro
                41.1833, -8.6000;  % Porto
                40.6500, -7.9167;  % Viseu
                41.1000, -7.8167;  % Lamego
                40.5667, -8.4500;  % Águeda
                41.1667, -7.7833;  % Peso da Régua
                41.4500, -8.3000;  % Guimarães
                42.0333, -8.6333;  % Valença do Minho
                41.5333, -8.6167;  % Barcelos
            ];
            nomesCidades = {
                'Bragança', 'Vila Real', 'Chaves', 'Viana do Castelo', 'Braga', ...
                'Aveiro', 'Porto', 'Viseu', 'Lamego', 'Águeda', 'Peso da Régua', ...
                'Guimarães', 'Valença do Minho', 'Barcelos'
            };

        case 'b'
            % 20 cidades
            cidades = [
                41.8167, -6.7500;  % Bragança
                41.3000, -7.7500;  % Vila Real
                41.7333, -7.4667;  % Chaves
                41.7000, -8.8333;  % Viana do Castelo
                41.5500, -8.4333;  % Braga
                40.6333, -8.6500;  % Aveiro
                41.1833, -8.6000;  % Porto
                40.6500, -7.9167;  % Viseu
                41.1000, -7.8167;  % Lamego
                41.4500, -8.3000;  % Guimarães
                40.2050, -8.4167;  % Coimbra
                37.0167, -7.9333;  % Faro
                38.5667, -7.9000;  % Évora
                38.7167, -9.1667;  % Lisboa
                39.2833, -7.4333;  % Portalegre
                37.1167, -7.6500;  % Tavira
                37.0000, -8.9333;  % Sagres
                38.5333, -8.9000;  % Setúbal
                40.5333, -7.2667;  % Guarda
                39.2333, -8.6833;  % Santarém
            ];
            nomesCidades = {
                'Bragança', 'Vila Real', 'Chaves', 'Viana do Castelo', 'Braga', ...
                'Aveiro', 'Porto', 'Viseu', 'Lamego', 'Guimarães', 'Coimbra', ...
                'Faro', 'Évora', 'Lisboa', 'Portalegre', 'Tavira', 'Sagres', ...
                'Setúbal', 'Guarda', 'Santarém'
            };
        
        case 'c'
            % 30 cidades
            cidades = [
                41.8167, -6.7500;  % Bragança
                41.3000, -7.7500;  % Vila Real
                41.7333, -7.4667;  % Chaves
                41.7000, -8.8333;  % Viana do Castelo
                41.5500, -8.4333;  % Braga
                40.6333, -8.6500;  % Aveiro
                41.1833, -8.6000;  % Porto
                40.6500, -7.9167;  % Viseu
                41.1000, -7.8167;  % Lamego
                41.4500, -8.3000;  % Guimarães
                40.2050, -8.4167;  % Coimbra
                37.0167, -7.9333;  % Faro
                38.5667, -7.9000;  % Évora
                38.7167, -9.1667;  % Lisboa
                39.2833, -7.4333;  % Portalegre
                37.1167, -7.6500;  % Tavira
                37.0000, -8.9333;  % Sagres
                38.5333, -8.9000;  % Setúbal
                40.5333, -7.2667;  % Guarda
                39.2333, -8.6833;  % Santarém
                38.0167, -7.8667;  % Beja
                37.9500, -8.8500;  % Sines
                40.2833, -7.5000;  % Covilhã
                39.6000, -8.4167;  % Tomar
                40.5667, -8.4500;  % Águeda
                39.7500, -8.8000;  % Leiria
                39.8167, -7.5000;  % Castelo Branco
                38.8833, -7.1667;  % Elvas
                41.5000, -6.2667;  % Miranda do Douro
                38.8000, -9.3833;  % Sintra
            ];
            nomesCidades = {
                'Bragança', 'Vila Real', 'Chaves', 'Viana do Castelo', 'Braga', ...
                'Aveiro', 'Porto', 'Viseu', 'Lamego', 'Guimarães', 'Coimbra', ...
                'Faro', 'Évora', 'Lisboa', 'Portalegre', 'Tavira', 'Sagres', ...
                'Setúbal', 'Guarda', 'Santarém', 'Beja', 'Sines', 'Covilhã', ...
                'Tomar', 'Águeda', 'Leiria', 'Castelo Branco', 'Elvas', 'Miranda do Douro', 'Sintra'
            };
        otherwise
            error('Opção inválida. Escolha entre a, b ou c.');
    end
end
