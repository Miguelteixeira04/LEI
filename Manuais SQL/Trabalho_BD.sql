USE master
GO

CREATE DATABASE Trabalho_BD
GO

USE Trabalho_BD
GO

CREATE TABLE Endereco(
	ID_Endereco				CHAR(5)			NOT NULL,
	Endereco_Morada			VARCHAR(50)		NOT NULL,
	Endereco_CodigoPostal	CHAR(8)			NOT NULL,
	Endereco_Localidade		VARCHAR(50)		NOT NULL,
	
	CHECK	(ID_Endereco LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	CHECK	(Endereco_CodigoPostal LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]'),
	PRIMARY KEY (ID_Endereco)
)

CREATE TABLE Pessoas(
	CC						CHAR(13)		NOT NULL,
	Nome					VARCHAR(50)		NOT NULL,		
	Data_Nascimento			SMALLDATETIME	NOT NULL,
	ID_Endereco				CHAR(5)			NOT NULL,
	Telefone				BIGINT			NOT NULL,

	CHECK	(Telefone LIKE '[2-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	CHECK	(CC LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]-[0-9][A-Z][A-Z][0-9]'),
	PRIMARY KEY (CC),
	FOREIGN KEY (ID_Endereco) REFERENCES Endereco(ID_Endereco)
)

CREATE TABLE Formadores(
	CC						CHAR(13)		NOT NULL,
	Nivel					INT				NOT NULL,

	CHECK (Nivel>=1 AND Nivel<=10), 
	PRIMARY KEY (CC),
	FOREIGN KEY (CC) REFERENCES Pessoas(CC)
)

CREATE TABLE TiposdeFabricacao(
    Fabricacao_ID			CHAR(5)			NOT NULL,
    NomeFabricacao			VARCHAR(50)     NOT NULL,
    Descricao				VARCHAR(255),

	CHECK (Fabricacao_ID LIKE '[0-9][0-9][0-9][0-9][0-9]'),
    PRIMARY KEY (Fabricacao_ID)
)

CREATE TABLE ManuaisEscolares(
	ISBN					BIGINT			NOT NULL,
	Titulo					VARCHAR(50)		NOT NULL,
	AnoEscolar				INT				NOT NULL,
	Edicao					INT				NOT NULL,
	Fabricacao_ID			CHAR(5)			NOT NULL,					

	CHECK	(Edicao>0),
	CHECK	(AnoEscolar BETWEEN 1 AND 12),
	CHECK	(ISBN LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (ISBN),
	FOREIGN KEY (Fabricacao_ID) REFERENCES TiposdeFabricacao (Fabricacao_ID)
)

CREATE TABLE Erratas(
	Errata_ID				CHAR(5)			NOT NULL,
	Texto					VARCHAR(255)    NOT NULL,

	CHECK (Errata_ID LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (Errata_ID)
)

CREATE TABLE ManuaisEscolares_Erratas(
	ISBN					BIGINT			NOT NULL,
	Errata_ID				CHAR(5)			NOT NULL,

	PRIMARY KEY (Errata_ID),
	FOREIGN KEY (Errata_ID) REFERENCES Erratas(Errata_ID),
	FOREIGN KEY (ISBN) REFERENCES ManuaisEscolares(ISBN)
)

CREATE TABLE Editoras(
	Editora_ID				CHAR(5)			NOT NULL,
	Nome_Editora			VARCHAR(50)		NOT NULL,
	Cidade_Editora			VARCHAR(50)		NOT NULL,
	TotVendas				INT				NOT NULL,

	CHECK (Editora_ID LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	CHECK (TotVendas>=0),
	PRIMARY KEY(Editora_ID)
)

CREATE TABLE Paises(
    Pais_ID					CHAR(5)         NOT NULL,
    NomePais				VARCHAR(50)     NOT NULL,
    Criador					BIT             NOT NULL    DEFAULT 0, -- nao(0)/ sim(1)  

	CHECK (Pais_ID LIKE '[0-9][0-9][0-9][0-9][0-9]'),
    PRIMARY KEY (Pais_ID)
)

CREATE TABLE Viver(
	DataInicio				SMALLDATETIME	NOT NULL	DEFAULT GETDATE(),
	DataFim					SMALLDATETIME,
	Pais_ID					CHAR(5)			NOT NULL,
	CC						CHAR(13)		NOT NULL,

	CHECK	(DataFim>DataInicio), 
	PRIMARY KEY (DataInicio,Pais_ID,CC),
	FOREIGN KEY	(Pais_ID) REFERENCES Paises(Pais_ID),
	FOREIGN KEY	(CC) REFERENCES Pessoas(CC)
)

CREATE TABLE Classificar(
    CC						CHAR(13)		NOT NULL,
    ISBN					BIGINT			NOT NULL,
    Fabricacao_ID			CHAR(5)			NOT NULL,

    PRIMARY KEY (CC,ISBN,Fabricacao_ID),
    FOREIGN KEY (CC) REFERENCES Pessoas(CC),
    FOREIGN KEY (ISBN) REFERENCES ManuaisEscolares(ISBN),
    FOREIGN KEY (Fabricacao_ID) REFERENCES TiposdeFabricacao(Fabricacao_ID)
)

CREATE TABLE Formacao(
    DataFormacao        SMALLDATETIME   NOT NULL,
    PrecoFormacao       MONEY           NOT NULL,
    ISBN                BIGINT          NOT NULL,
    CC                  CHAR(13)        NOT NULL,
    ID_Formacao         CHAR(5)         NOT NULL,

    CHECK (PrecoFormacao >= 0),
    CHECK (ID_Formacao LIKE '[0-9][0-9][0-9][0-9][0-9]'),
    PRIMARY KEY (ID_Formacao),
    UNIQUE (ISBN, CC),
    FOREIGN KEY (ISBN) REFERENCES ManuaisEscolares(ISBN),
    FOREIGN KEY (CC) REFERENCES Pessoas(CC)
)

CREATE TABLE Formandos(
    CC              CHAR(13)    NOT NULL,
    Idade           INT,                            
    ID_Formacao     CHAR(5)     NOT NULL,

    CHECK(Idade > 0),
    PRIMARY KEY (CC, ID_Formacao),
    FOREIGN KEY (CC) REFERENCES Pessoas(CC),
    FOREIGN KEY (ID_Formacao) REFERENCES Formacao(ID_Formacao)
)

CREATE TABLE Produzir(
    DataProduzir			SMALLDATETIME	NOT NULL,
    CustoUnidade			MONEY		    NOT NULL,
    Quantidade_Produzir		INT             NOT NULL,
    Editora_ID				CHAR(5)         NOT NULL,
    ISBN					BIGINT          NOT NULL,

    CHECK (CustoUnidade>0),
    CHECK (Quantidade_Produzir>0),
    PRIMARY KEY (DataProduzir, Editora_ID, ISBN),
    FOREIGN KEY (Editora_ID) REFERENCES Editoras(Editora_ID),
    FOREIGN KEY (ISBN) REFERENCES ManuaisEscolares(ISBN)
)

CREATE TABLE Vender(
	DataVender				SMALLDATETIME	NOT NULL,
	Quantidade_Vender		INT				NOT NULL,
	Preco_Unidade			MONEY			NOT NULL,
	Pais_ID					CHAR(5)			NOT NULL,		
	ISBN					BIGINT          NOT NULL,
	Editora_ID				CHAR(5)         NOT NULL,

	CHECK (Preco_Unidade>0),
	CHECK (Quantidade_Vender>=0),
	PRIMARY KEY (Pais_ID,ISBN,Editora_ID,DataVender),
	FOREIGN KEY	(Pais_ID) REFERENCES Paises(Pais_ID),
	FOREIGN KEY (ISBN) REFERENCES ManuaisEscolares(ISBN),
	FOREIGN KEY (Editora_ID) REFERENCES Editoras(Editora_ID)
)


--Inserir registos na tabela Endereco
INSERT INTO Endereco (ID_Endereco, Endereco_Morada, Endereco_CodigoPostal, Endereco_Localidade)
VALUES
('10001', 'Rua A', '1234-567', 'Vila Real'),
('10002', 'Rua B', '2345-678', 'Porto'),
('10003', 'Rua C', '3456-789', 'Lisboa'),
('10004', 'Rua D', '4567-890', 'Braga'),
('10005', 'Rua E', '5678-901', 'Bragança'), --formadores
('10006', 'Rua F', '6789-012', 'Coimbra');	--formadores

-- Inserir registros na tabela Pessoas
INSERT INTO Pessoas (CC, Nome, Data_Nascimento, ID_Endereco, Telefone)
VALUES
('12345678-1AB2', 'Luis Martelo', '1985-01-07', '10001', 912345678),
('23456789-2BC3', 'Sergio Pereira', '1990-02-02', '10002', 923456789),
('34567890-3CD4', 'Carlos Sinais', '2000-03-03', '10003', 934567890),
('45678901-4DC5', 'Jorge Silva', '1998-04-10', '10004', 945678901),
('56789012-5EF6', 'Cristiano Corneta', '1973-11-16', '10005', 967362945), --formadores
('67890123-7FG8', 'Frederico Vassoura', '1968-05-28', '10006', 927302028); --formadores

-- Inserir registros na tabela TiposdeFabricacao
INSERT INTO TiposdeFabricacao (Fabricacao_ID, NomeFabricacao, Descricao)
VALUES
('20001', 'Tipo A', 'Descricao A'),
('20002', 'Tipo B', 'Descricao B'),
('20003', 'Tipo C', 'Descricao C');

-- Inserir registros na tabela ManuaisEscolares
INSERT INTO ManuaisEscolares (ISBN, Titulo, AnoEscolar, Edicao, Fabricacao_ID)
VALUES
(9781234567890, 'Matemática A', 5, 3, '20001'),
(9782345678901, 'História', 8, 2, '20002'),
(9783456789012, 'Inglês', 3, 1, '20003'),
(9784567890123, 'Português', 2, 4, '20003'),
(9785678901234, 'Ciências', 7, 1, '20001');  -- Novo livro que não será vendido

-- Inserir registros na tabela Erratas
INSERT INTO Erratas (Errata_ID, Texto)
VALUES
('30001', 'Erro na página 10, corrigido no capítulo 2.'),
('30002', 'Erro na página 25, corrigido no capítulo 5.'),
('30003', 'Erro na página 30'),
('30004', 'Erro na página 40');

-- Inserir registros na tabela ManuaisEscolares_Erratas
INSERT INTO ManuaisEscolares_Erratas (ISBN, Errata_ID)
VALUES
(9781234567890, '30001'),
(9782345678901, '30002'),
(9783456789012, '30003'),
(9784567890123, '30004');

-- Inserir registros na tabela Editoras
INSERT INTO Editoras (Editora_ID, Nome_Editora, Cidade_Editora, TotVendas)
VALUES
('40001', 'Arial', 'Porto', 1000),
('40002', 'Porto Editora', 'Lisboa', 2000),
('40003', 'LeYa', 'Faro', 1500),
('40004', 'Planeta', 'Lisboa', 0);

-- Inserir registros na tabela Paises
INSERT INTO Paises (Pais_ID, NomePais, Criador)
VALUES
('50001', 'Portugal', 1),
('50002', 'Espanha', 1),
('50003', 'França', 0),
('50004', 'Alemanha', 0),
('50005', 'Cuba', 1),
('50006', 'Inglaterra', 0);

-- Inserir registros na tabela Viver
INSERT INTO Viver (DataInicio, DataFim, Pais_ID, CC)
VALUES
('2020-01-01', '2021-01-01', '50001', '12345678-1AB2'),
('2021-02-01', '2022-02-01', '50002', '23456789-2BC3'),
('2021-01-01', '2021-12-31', '50003', '34567890-3CD4'),
('2021-01-02', '2021-12-30', '50004', '45678901-4DC5'),
('2020-03-27', '2022-12-31', '50005', '56789012-5EF6'), --formadores
('2020-12-01', '2021-10-20', '50006', '67890123-7FG8'); --formadores

-- Inserir registros na tabela Classificar
INSERT INTO Classificar (CC, ISBN, Fabricacao_ID)
VALUES
('12345678-1AB2', 9781234567890, '20001'),
('23456789-2BC3', 9782345678901, '20002'),
('34567890-3CD4', 9783456789012, '20003'),
('45678901-4DC5', 9784567890123, '20003');

-- Inserir registros de exemplo na tabela Formacao
-- Inserir registros na tabela Formacao para associar os formandos aos formadores
INSERT INTO Formacao (DataFormacao, PrecoFormacao, ISBN, CC, ID_Formacao)
VALUES
('2022-05-01', 150.00, 9781234567890, '56789012-5EF6', '60001'), -- formação dada por Cristiano Corneta
('2022-06-01', 200.00, 9782345678901, '56789012-5EF6', '60002'), -- formação dada por Cristiano Corneta
('2023-01-01', 100.00, 9783456789012, '56789012-5EF6', '60003'), -- formação dada por Cristiano Corneta
('2023-01-02', 300.00, 9784567890123, '56789012-5EF6', '60004'), -- formação dada por Cristiano Corneta
('2022-07-01', 250.00, 9781234567890, '67890123-7FG8', '60005'), -- formação dada por Frederico Vassoura
('2023-02-01', 300.00, 9782345678901, '67890123-7FG8', '60006'), -- formação dada por Frederico Vassoura
('2022-08-01', 200.00, 9783456789012, '67890123-7FG8', '60007'), -- formação dada por Frederico Vassoura
('2022-08-02', 200.00, 9784567890123, '67890123-7FG8', '60008'); -- formação dada por Frederico Vassoura

-- Inserir registros na tabela Formandos
INSERT INTO Formandos (CC, Idade, ID_Formacao)
VALUES
('12345678-1AB2', 44, '60001'),
('12345678-1AB2', 44, '60004'),
('12345678-1AB2', 44, '60003'),
('23456789-2BC3', 34, '60002'),
('23456789-2BC3', 34, '60006'),
('23456789-2BC3', 34, '60007'),
('34567890-3CD4', 24, '60001'),
('34567890-3CD4', 24, '60006'),
('34567890-3CD4', 24, '60002'),
('45678901-4DC5', 22, '60003'),
('45678901-4DC5', 22, '60005'),
('45678901-4DC5', 22, '60006');

-- Inserir registros na tabela Formadores
INSERT INTO Formadores (CC, Nivel)
VALUES
('56789012-5EF6', 6), --formadores
('67890123-7FG8', 8); --formadores

-- Inserir registros na tabela Produzir
INSERT INTO Produzir (DataProduzir, CustoUnidade, Quantidade_Produzir, Editora_ID, ISBN)
VALUES
('2023-01-01', 10.00, 100, '40001', 9781234567890),
('2023-02-01', 15.00, 150, '40002', 9782345678901),
('2023-03-01', 30.00, 3000, '40003', 9783456789012),
('2023-04-01', 30.00, 3000, '40004', 9784567890123),
('2023-05-01', 25.00, 500, '40001', 9785678901234);  -- Novo livro produzido mas não vendido

-- Inserir registros na tabela Vender
INSERT INTO Vender (DataVender, Quantidade_Vender, Preco_Unidade, Pais_ID, ISBN, Editora_ID)
VALUES
('2023-03-01', 50, 20.00, '50001', 9781234567890, '40001'),
('2023-04-01', 75, 25.00, '50002', 9782345678901, '40002'),
('2023-07-10', 1500, 35.00, '50003', 9783456789012, '40003'),
('2023-05-15', 2000, 40.00, '50004', 9784567890123, '40004');

-- 2.
-- 2.1  Qual foi o 1º manual vendido? [ISBN, Título, Data, País (nome)]
SELECT ME.ISBN, ME.Titulo, CONVERT(DATE, VE.DataVender) AS DataVender, P.NomePais
FROM ManuaisEscolares ME, Vender VE, Paises P, Viver VI
WHERE ME.ISBN = VE.ISBN AND
	  VE.Pais_ID = P.Pais_ID AND
	  P.Pais_ID = VI.Pais_ID AND
	  VE.DataVender = (SELECT MIN(DataVender) 
					   FROM Vender)

--2.2 Quantos manuais já foram produzidos por cada editora? [Editora (nome, cidade), N_Manuais] 
SELECT E.Nome_Editora, E.Cidade_Editora, 
       (SELECT SUM(Quantidade_Produzir) 
        FROM Produzir P 
        WHERE P.Editora_ID = E.Editora_ID) AS N_Manuais
FROM Editoras E;

--2.3 Que formadores vivem em países criadores? [Pessoas (nome, telefone)] 
SELECT DISTINCT P.Nome, P.Telefone
FROM Pessoas P, Formadores F, Viver V, Paises PA
WHERE P.CC = F.CC AND 
      P.CC = V.CC AND 
      V.Pais_ID = PA.Pais_ID AND 
      PA.Criador = 1;

-- 2.4. Que formandos participaram em mais formações nos últimos 2 anos? [nome, Total Formações] 
SELECT TOP 1
    P.Nome, (SELECT COUNT(DISTINCT F.ID_Formacao)
			 FROM Formandos F
			 WHERE F.CC = P.CC AND
				   F.ID_Formacao IN (SELECT ID_Formacao
									 FROM Formacao
									 WHERE DataFormacao >= DATEADD(YEAR, -2, GETDATE()))) AS TotalFormacoes
FROM
    Pessoas P
WHERE
    EXISTS (SELECT 1
			FROM Formandos F
			WHERE F.CC = P.CC)
ORDER BY
    TotalFormacoes DESC;

-- 2.5 Que manuais nunca foram vendidos? [título, editora (nome)]. Apresente-os ordenados pela editora e pelo título.
SELECT ME.Titulo, E.Nome_Editora
FROM ManuaisEscolares ME, Produzir P, Editoras E
WHERE ME.ISBN = P.ISBN AND
	  P.Editora_ID = E.Editora_ID AND
	  ME.ISBN NOT IN (SELECT ISBN FROM Vender)
ORDER BY E.Nome_Editora, ME.Titulo;


-- 3. Crie um procedimento que apresente quantos formandos cada formador formou no 
-- último ano. O procedimento deve retornar o valor total pago por esses clientes 
-- para essas formações.
GO

CREATE PROCEDURE ContFormandosUltimoAno
AS
BEGIN
    -- armazena o 1º dia do último ano (anoAtual - 1)
    DECLARE @primeiro_dia_ultimo_ano DATE = DATEADD(YEAR, -1, DATEADD(DAY, 1, CAST(FORMAT(GETDATE(), 'yyyy-01-01') AS DATE)))
    -- armazena o último dia do último ano (anoAtual - 1)
    DECLARE @ultimo_dia_ultimo_ano DATE = DATEADD(DAY, -1, CAST(FORMAT(GETDATE(), 'yyyy-01-01') AS DATE))

    -- seleção dos formadores que realizaram formações no último ano
    SELECT 
        F.CC AS Formador_CC,
        (SELECT Nome
		 FROM Pessoas
		 WHERE CC = F.CC) AS NomeFormador,

        -- contar nº de formandos em formações realizadas pelo formador no último ano
        (SELECT COUNT(DISTINCT CC) 
         FROM Formandos 
         WHERE ID_Formacao IN (SELECT ID_Formacao
							   FROM Formacao
							   WHERE CC = F.CC AND
									 DataFormacao BETWEEN @primeiro_dia_ultimo_ano AND @ultimo_dia_ultimo_ano)) AS TotalFormandosFormacao,
        
		-- somar preço total das formações realizadas pelo formador no último ano
        (SELECT SUM(PrecoFormacao) 
         FROM Formacao 
         WHERE CC = F.CC AND
			   DataFormacao BETWEEN @primeiro_dia_ultimo_ano AND @ultimo_dia_ultimo_ano) AS ValorTotalPago
    FROM 
        Formadores F
    WHERE 
        EXISTS (SELECT 1
				FROM Formacao
				WHERE CC = F.CC AND
					  DataFormacao BETWEEN @primeiro_dia_ultimo_ano AND @ultimo_dia_ultimo_ano)
	ORDER BY 
        TotalFormandosFormacao DESC, ValorTotalPago DESC;
END

-- comando para testar o Stored Procedure
EXECUTE ContFormandosUltimoAno


-- 4. Assumindo que uma formação dura 7 dias, crie um trigger que apenas deixe inserir um novo registo no relacionamento Formação se o formando não estiver noutra formação. 
GO

CREATE TRIGGER ImpedirFormacaoConcorrente
ON Formacao
INSTEAD OF INSERT
AS
BEGIN
    -- verifica se há dados para inserir
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- verifica se o formando já está em outra formação nos próximos 7 dias
        IF EXISTS (SELECT 1
				   FROM Formacao F, inserted I
				   WHERE F.CC = I.CC AND
						 ABS(DATEDIFF(DAY, F.DataFormacao, I.DataFormacao)) < 7)
        BEGIN
            -- se estiver noutra formação aciona o erro do trigger e aborta a inserção
            RAISERROR ('O formando já está em outra formação nos próximos 7 dias.', 16, 1)
            ROLLBACK TRANSACTION;
        END

        ELSE
        BEGIN
            -- se não estiver noutra formação permitir a inserção
            INSERT INTO Formacao (DataFormacao, PrecoFormacao, ISBN, CC, ID_Formacao)
            SELECT DataFormacao, PrecoFormacao, ISBN, CC, ID_Formacao
            FROM inserted;
        END
    END
END
-- executar a inserção de dados na tabela formação para testar o trigger!