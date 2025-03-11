USE master
GO 

CREATE DATABASE LAB2
GO

USE LAB2
GO

CREATE TABLE Autenticado(
	Username				VARCHAR(50)		NOT NULL,
	Password				VARCHAR(10)		NOT NULL,
	Contacto				BIGINT			NOT NULL,
	Contacto2				BIGINT,
	Contacto3				BIGINT,
	Nome					VARCHAR(50)		NOT NULL,
	
	CHECK (Contacto LIKE '9[1236][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	CHECK (Contacto2 LIKE '9[1236][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	CHECK (Contacto3 LIKE '9[1236][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (Username)
)

CREATE TABLE Bibliotecario(
	Username_Bib			VARCHAR(50)		NOT NULL,

	PRIMARY KEY (Username_Bib),
	FOREIGN KEY (Username_Bib) REFERENCES Autenticado(Username)
)

CREATE TABLE Leitores(
	Username_Lei			VARCHAR(50)		NOT NULL,
	Morada					VARCHAR(50)		NOT NULL,
	Email					VARCHAR(50)		NOT NULL,

	PRIMARY KEY (Username_Lei),
	FOREIGN KEY (Username_Lei) REFERENCES Autenticado(Username)
)

CREATE TABLE Administradores(
	Username_Admin			VARCHAR(50)		NOT NULL,
	Nome					VARCHAR(50)		NOT NULL,
	Email					VARCHAR(50)		NOT NULL,
	Password				VARCHAR(10)		NOT NULL,

	PRIMARY KEY (Username_Admin)
)

CREATE TABLE MotivoBloq(
	ID_MotivoBloq			INT				NOT NULL,
	Descricao				VARCHAR(100)	NOT NULL,

	CHECK (ID_MotivoBloq LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (ID_MotivoBloq)
)

CREATE TABLE Bloquear(
	ID_Bloqueio				INT				NOT NULL,
	ID_MotivoBloq			INT			    NOT NULL,
	Username_Admin			VARCHAR(50)		NOT NULL,
	Username				VARCHAR(50)		NOT NULL,
	Data_Bloqueio			SMALLDATETIME	NOT NULL	DEFAULT GETDATE(),
	Bloqueado				BIT				NOT NULL	DEFAULT 0, -- NAO BLOQUEADO

	CHECK (ID_Bloqueio LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (ID_Bloqueio),
	FOREIGN KEY (Username_Admin) REFERENCES Administradores(Username_Admin),
	FOREIGN KEY (Username)	REFERENCES Autenticado(Username),
	FOREIGN KEY (ID_MotivoBloq)	REFERENCES MotivoBloq(ID_MotivoBloq)
)

CREATE TABLE ValidarRegisto(
	ID_Valid				INT				NOT NULL,
	Username_Admin			VARCHAR(50)		NOT NULL,
	Username_Bib			VARCHAR(50)		NOT NULL,
	Data_Valid				SMALLDATETIME	NOT NULL,
	Validado				BIT				NOT NULL	DEFAULT 0, -- não validado

	CHECK (ID_Valid LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (ID_Valid),
	FOREIGN KEY (Username_Admin) REFERENCES Administradores(Username_Admin),
	FOREIGN KEY (Username_Bib) REFERENCES Bibliotecario(Username_Bib)
)

CREATE TABLE Autor(
	ID_Autor				INT				NOT NULL,
	Name_Autor				VARCHAR(50)		NOT NULL,

	CHECK (ID_Autor LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (ID_Autor)
)

CREATE TABLE Biblioteca(
	ID_Biblioteca			INT				NOT NULL,
	HorarioI				TIME			NOT NULL,
	HorarioF				TIME			NOT NULL,
	Localizacao				VARCHAR(500)	NOT NULL, 
	Nome					VARCHAR(50)		NOT NULL,
	Email					VARCHAR(50)		NOT NULL,
	Telefone				BIGINT			NOT NULL,
	
	CHECK (ID_Biblioteca LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	CHECK (Telefone LIKE '2[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	CHECK (HorarioF > HorarioI),
	PRIMARY KEY (ID_Biblioteca),
)

CREATE TABLE Livro(
	ISBN					BIGINT			NOT NULL,
	Preco					MONEY			NOT NULL,
	Titulo					VARCHAR(50)		NOT NULL,
	N_Exemplares			INT				NOT NULL,
	Genero					VARCHAR(50)		NOT NULL,
	Username_BibAdd			VARCHAR(50)		NOT NULL,
	ID_Autor				INT				NOT NULL,
	ID_Biblioteca			INT				NOT NULL,
	
	CHECK	(ISBN LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (ISBN),
	FOREIGN KEY (Username_BibAdd) REFERENCES Bibliotecario(Username_Bib),
	FOREIGN KEY (ID_Autor) REFERENCES Autor(ID_Autor),
	FOREIGN KEY (ID_Biblioteca) REFERENCES Biblioteca(ID_Biblioteca)
)

CREATE TABLE Requisicao(
	ID_Requisicao			INT				NOT NULL,
	Username_Bib			VARCHAR(50),
	Username_Lei			VARCHAR(50)		NOT NULL,
	ISBN					BIGINT			NOT NULL,
	Data_Requisicao			SMALLDATETIME	NOT NULL	DEFAULT GETDATE(),
	Data_Devolucao			SMALLDATETIME,
	Estado					BIT				NOT NULL	DEFAULT 0, --NAO ESTA REQUISITADO

	CHECK (ID_Requisicao LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	CHECK (Data_Devolucao > Data_Requisicao),
	PRIMARY KEY (ID_Requisicao),
	FOREIGN KEY (Username_Bib) REFERENCES Bibliotecario(Username_Bib),
	FOREIGN KEY (Username_Lei) REFERENCES Leitores(Username_Lei),
	FOREIGN KEY (ISBN) REFERENCES Livro(ISBN)
)

CREATE TABLE Cria_Admin(
	Username_Admin			VARCHAR(50)		NOT NULL,
	Username_NovoAdmin		VARCHAR(50)		NOT NULL,

	FOREIGN KEY (Username_Admin) REFERENCES Administradores(Username_Admin),
	FOREIGN KEY (Username_NovoAdmin) REFERENCES Administradores (Username_Admin)
)

CREATE TABLE EntregasAtraso(
	ID_Entregas				INT				NOT NULL,
	ID_Requisicao			INT				NOT NULL,

	CHECK (ID_Entregas LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (ID_Entregas),
	FOREIGN KEY (ID_Requisicao) REFERENCES Requisicao(ID_Requisicao)
)

CREATE TABLE HistoricoRequ(
	ID_Historico			INT				NOT NULL,
	ID_Requisicao			INT				NOT NULL,

	CHECK (ID_Historico LIKE '[0-9][0-9][0-9][0-9][0-9]'),
	PRIMARY KEY (ID_Historico), 
	FOREIGN KEY (ID_Requisicao) REFERENCES Requisicao(ID_Requisicao)
)