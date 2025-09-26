/*
============================================================
 Script: script_bd.sql
 Projeto: MottuBracelet
 Objetivo: Criação das tabelas compatíveis com AppDbContext
 Autor: Pedro Andrade (RM558186)
============================================================
*/

-- ===========================
-- Tabela: Patio
-- ===========================
CREATE TABLE Patio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    CapacidadeMaxima INT NOT NULL,
    AdministradorResponsavel NVARCHAR(100) NOT NULL,
    Logradouro NVARCHAR(200) NOT NULL,
    Numero INT NOT NULL,
    Cep NVARCHAR(20) NOT NULL,
    Complemento NVARCHAR(100) NULL,
    Cidade NVARCHAR(100) NOT NULL,
    Pais NVARCHAR(100) NOT NULL
);
GO

-- ===========================
-- Tabela: Moto
-- ===========================
CREATE TABLE Moto (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Imei NVARCHAR(50) NULL,
    Placa NVARCHAR(20) NULL,
    PatioId INT NULL,
    DispositivoId INT NULL,
    CONSTRAINT FK_Moto_Patio FOREIGN KEY (PatioId) REFERENCES Patio(Id)
);
GO

-- ===========================
-- Tabela: Dispositivo
-- ===========================
CREATE TABLE Dispositivo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StatusDispositivo NVARCHAR(50) NOT NULL,
    MotoId INT NULL,
    PatioId INT NULL,
    CONSTRAINT FK_Dispositivo_Moto FOREIGN KEY (MotoId) REFERENCES Moto(Id),
    CONSTRAINT FK_Dispositivo_Patio FOREIGN KEY (PatioId) REFERENCES Patio(Id)
);
GO

-- ===========================
-- Tabela: HistoricoPatio
-- ===========================
CREATE TABLE HistoricoPatio (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MotoId INT NULL,
    PatioId INT NULL,
    DataEntrada DATETIME NOT NULL DEFAULT GETDATE(),
    DataSaida DATETIME NULL,
    CONSTRAINT FK_Historico_Moto FOREIGN KEY (MotoId) REFERENCES Moto(Id),
    CONSTRAINT FK_Historico_Patio FOREIGN KEY (PatioId) REFERENCES Patio(Id)
);
GO
