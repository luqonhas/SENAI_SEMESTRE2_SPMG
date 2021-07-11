-- DDL

CREATE DATABASE SPMG_DBFIRST;
GO

USE SPMG_DBFIRST;
GO


CREATE TABLE TipoUsuarios
(
	idTipoUsuario INT PRIMARY KEY IDENTITY,
	permissao VARCHAR(200) UNIQUE NOT NULL
);
GO



CREATE TABLE Usuarios
(
	idUsuario INT PRIMARY KEY IDENTITY,
	idTipoUsuario INT FOREIGN KEY REFERENCES TipoUsuarios(idTipoUsuario),
	email VARCHAR(200) UNIQUE NOT NULL,
	senha VARCHAR(200) NOT NULL,
);
GO

ALTER TABLE Usuarios
ADD foto VARCHAR(40) DEFAULT('user.png');
GO



CREATE TABLE Especialidades
(
	idEspecialidade INT PRIMARY KEY IDENTITY,
	especialidade VARCHAR(200) UNIQUE NOT NULL
);
GO



CREATE TABLE Clinicas
(
	idClinica INT PRIMARY KEY IDENTITY,
	cnpj CHAR(14) UNIQUE NOT NULL,
	nomeFantasia VARCHAR(255) NOT NULL,
	razaoSocial VARCHAR(255) UNIQUE NOT NULL,
	endereco VARCHAR(255) NOT NULL,
	horarioAbertura TIME NOT NULL,
	horarioFechamento TIME NOT NULL
);
GO



CREATE TABLE Pacientes
(
	idPaciente INT PRIMARY KEY IDENTITY,
	idUsuario INT FOREIGN KEY REFERENCES Usuarios(idUsuario),
	nomePaciente VARCHAR(200) NOT NULL,
	rg CHAR(9) UNIQUE NOT NULL,
	cpf CHAR(11) UNIQUE NOT NULL,
	dataNascimento DATE NOT NULL,
	telefonePaciente CHAR(11) UNIQUE NOT NULL,
	endereco VARCHAR(200) NOT NULL
);
GO



CREATE TABLE Medicos
(
	idMedico INT PRIMARY KEY IDENTITY,
	idUsuario INT FOREIGN KEY REFERENCES Usuarios(idUsuario),
	idClinica INT FOREIGN KEY REFERENCES Clinicas(idClinica),
	idEspecialidade INT FOREIGN KEY REFERENCES Especialidades(idEspecialidade),
	nomeMedico VARCHAR(200) NOT NULL,
	crm CHAR(8) UNIQUE NOT NULL
);
GO



CREATE TABLE Situacoes
(
	idSituacao INT PRIMARY KEY IDENTITY,
	situacao VARCHAR(100) UNIQUE NOT NULL
);
GO



CREATE TABLE Consultas
(
	idConsulta INT PRIMARY KEY IDENTITY,
	idMedico INT FOREIGN KEY REFERENCES Medicos(idMedico),
	idPaciente INT FOREIGN KEY REFERENCES Pacientes(idPaciente),
	idSituacao INT FOREIGN KEY REFERENCES Situacoes(idSituacao),
	dataConsulta DATE NOT NULL,
	horaConsulta TIME NOT NULL,
	descricao VARCHAR(200) DEFAULT ('Descrição não informada...') -- se não tiver nada preenchido na descrição, ficará como padrão o "descrição não informada..." para que o médico possa preencher depois
);
GO