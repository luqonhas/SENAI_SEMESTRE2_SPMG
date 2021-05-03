-- DQL

USE SPMG_DBFIRST;
GO

SELECT * FROM TipoUsuarios;

SELECT * FROM Usuarios

SELECT * FROM Pacientes;

SELECT * FROM Medicos;

SELECT * FROM Especialidades;

SELECT * FROM Clinicas;

SELECT * FROM Situacoes;

SELECT * FROM Consultas;


-- INÍCIO FUNCIONALIDADES:

	-- aqui mostrará as contas(seus emails) com seus "cargos"(admistrador, médico e paciente);
	SELECT permissao AS TipoUsuario, email AS Email FROM tipoUsuarios
	INNER JOIN usuarios
	ON tipoUsuarios.idTipoUsuario = usuarios.idTipoUsuario;
	GO
	

	-- aqui será informado o paciente, data do agendamento e qual médico irá atender a consulta (o médico possuirá sua determinada especialidade);
	SELECT nomePaciente AS Paciente, nomeMedico AS Médico, especialidade AS Especialidade, dataConsulta AS DiaAgendamento, horaConsulta AS HoraAgendamento, situacao AS Situação FROM Consultas
	INNER JOIN Pacientes
	ON Consultas.idPaciente = Pacientes.idPaciente
	INNER JOIN Medicos
	ON Consultas.idMedico = Medicos.idMedico
	INNER JOIN Especialidades
	ON Especialidades.idEspecialidade = Medicos.idEspecialidade
	INNER JOIN Situacoes
	ON Consultas.idSituacao = Situacoes.idSituacao;
	GO


	-- aqui deverá informar os dados da clínica (como endereço, horário de funcionamento, CNPJ, nome fantasia e razão social);
	SELECT nomeFantasia, razaoSocial, cnpj, endereco, horarioAbertura, horarioFechamento FROM Clinicas;
	GO


	-- aqui o médico poderá ver os agendamentos (consultas) associados a ele;
	SELECT nomeMedico AS Médico, nomePaciente AS Paciente, descricao AS Descrição, dataConsulta AS DataAgendamento, horaConsulta AS HoraAgendamento, situacao AS Agendamento FROM Medicos
	INNER JOIN Consultas
	ON Medicos.idMedico = Consultas.idMedico
	INNER JOIN Pacientes
	ON Consultas.idPaciente = Pacientes.idPaciente
	INNER JOIN Situacoes
	ON Consultas.idSituacao = Situacoes.idSituacao
	WHERE Medicos.idMedico = 2; -- Roberto Possarle
	GO


	-- aqui o paciente poderá visualizar suas próprias consultas;
	SELECT nomePaciente AS Paciente, nomeMedico AS Médico, descricao AS Descrição, dataConsulta AS DataAgendamento, horaConsulta AS HoraAgendamento, situacao AS Agendamento FROM Medicos
	INNER JOIN Consultas
	ON Medicos.idMedico = Consultas.idMedico
	INNER JOIN Pacientes
	ON Consultas.idPaciente = Pacientes.idPaciente
	INNER JOIN Situacoes
	ON Consultas.idSituacao = Situacoes.idSituacao
	WHERE Pacientes.idPaciente = 7; -- Mariana
	GO


	-- aqui temos um simulador de login básico;
	SELECT permissao AS TipoUsuario, email AS Emails, senha AS Senhas FROM Usuarios
	INNER JOIN TipoUsuarios
	ON Usuarios.idTipoUsuario = TipoUsuarios.idTipoUsuario
	WHERE email = 'adm@adm.com' AND senha = 'adm123';
	GO


-- CAPACIDADES E CRITÉRIOS:

	-- aqui mostra a quantidade de usuários cadastrados;
	SELECT COUNT (Usuarios.idUsuario) AS QtdUsuarios FROM Usuarios;
	GO


	-- aqui converte a data de nascimento dos usuários para o formato (mm-dd-yyyy) na exibição;
	SELECT nomePaciente AS Nomes, CONVERT (VARCHAR, dataNascimento, 101) AS DataNascimento FROM Pacientes; -- o 101 é meio que o "id" de conversão de data pra cada país, por exemplo, esse 101 é o padrão de data dos EUA
	GO


	-- aqui foi calculado a idade dos pacientes a partir da data de nascimento; || 8766 é o número de horas que tem um ano
	SELECT nomePaciente AS Nomes, DATEDIFF(HOUR, dataNascimento,GETDATE())/8766 AS Idades FROM Pacientes;
	GO


	-- aqui foi criado um evento para que a idade do usuário seja calculada todos os dias;
	SELECT Pacientes.nomePaciente, Pacientes.dataNascimento,
	CASE -- esse CASE funciona tipo um if no C#, caso a primeira condição for atendida, vai retornar tal "valor"
	WHEN DATEPART(MONTH, Pacientes.dataNascimento) <= DATEPART(MONTH, GETDATE()) -- o DATEPART retorna uma parte específica de uma data
	AND DATEPART(DAY, Pacientes.dataNascimento) <= DATEPART(DAY, GETDATE())
	THEN (DATEDIFF(YEAR, Pacientes.dataNascimento,GETDATE())) -- o DATEDIFF retorna a diferença entre duas datas
	ELSE (DATEDIFF(YEAR, Pacientes.dataNascimento,GETDATE())) - 1 -- caso nenhuma das outras condições forem verdadeiras, o ELSE retorna um valor
	END AS IdadeAtual
	FROM Pacientes
	WHERE Pacientes.idPaciente = 7
	GO


	-- aqui foi criado um evento para que a idade do usuário seja calculada todos os anos;
	SELECT Pacientes.nomePaciente, Pacientes.dataNascimento,
	DATEDIFF(YEAR, Pacientes.dataNascimento,GETDATE()) AS IdadeAtual
	FROM Pacientes;
	GO


	-- FUNÇÃO
		-- aqui foi criado uma função para retornar a quantidade de médicos de uma determinada especialidade;
		CREATE FUNCTION QntdMedicos(@idEspecialidade VARCHAR(200)) -- vai ser tipo um método, o @idEspecialidade vai ser um "atributo" vazio pra pegar outro valor depois
		RETURNS INT -- no final vai ser retornado um valor INT
		AS -- como
		BEGIN -- início
			DECLARE @qntd AS INT -- vai ser declarado um outro "atributo" INT vazio pra pegar o resultado final
			SET @qntd = -- vai "setar" dentro de @qntd
			(
			SELECT COUNT(nomeMedico) FROM Medicos -- vai contar os nomes dos médicos
			INNER JOIN Especialidades -- vai relacionar os nomes dos médicos com as especialidades
			ON Medicos.idEspecialidade = Especialidades.idEspecialidade
			WHERE Especialidades.especialidade = @idEspecialidade -- isso vai ocorrer onde o tituloEspecialidade for igual ao "atributo" @idEspecialidade
			)
			RETURN @qntd -- no fim vai retornar o valor com todas as informações
		END -- fim
		GO
		SELECT qntd = dbo.QntdMedicos('Psiquiatria'); -- vai exibir o valor do dbo.QntdMedicos('ESPECIALIDADE'), só que com o nome do atributo onde foi reunido todas os dados, que é o @qntd
		GO
		SELECT dbo.QntdMedicos('Psiquiatria') AS QuantidadeMedicos; -- mesma coisa do de cima, mas com outro nome na tabela
		GO


	-- STORED PROCEDURE

		-- aqui foi criado uma função para que retorne a idade do usuário a partir de uma determinada stored procedure;
		-- calcular a idade de todos os usuários
		CREATE PROCEDURE CalcularIdadeTodos
		AS
		SELECT Pacientes.nomePaciente, Pacientes.dataNascimento,
		DATEDIFF(YEAR, Pacientes.dataNascimento,GETDATE()) AS IdadeAtual
		FROM Pacientes;
		GO
		-- aqui executa o stored procedure "CalcularIdade"
		EXECUTE CalcularIdadeTodos;
		GO


		-- aqui foi criado uma função para que retorne a idade do usuário a partir de uma determinada stored procedure;
		-- calcular a idade de um usuário específico
		CREATE PROCEDURE CalcularIdadeEspecifica(@nomePaciente VARCHAR(100))
		AS
		SELECT Pacientes.nomePaciente, Pacientes.dataNascimento,
		DATEDIFF(YEAR, Pacientes.dataNascimento,GETDATE()) AS IdadeAtual
		FROM Pacientes
		WHERE Pacientes.nomePaciente = @nomePaciente;
		GO
		-- aqui executa o stored procedure "CalcularIdade" pelo nome do paciente:
		EXECUTE CalcularIdadeEspecifica @nomePaciente = 'Mariana';
		GO