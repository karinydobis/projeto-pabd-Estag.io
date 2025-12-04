CREATE DATABASE estagio_db;
USE estagio_db;

CREATE TABLE estudante (
    id_est INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    nome_est VARCHAR(150) NOT NULL,
    email_est VARCHAR(150) NOT NULL UNIQUE,
    curso_est VARCHAR(150) NOT NULL,
    stacks_est VARCHAR(255),
    github_est VARCHAR(255),
    portfolio_est VARCHAR(255),
    ativo_est TINYINT NOT NULL DEFAULT 1
);

CREATE TABLE empresa (
    id_emp INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    razao_emp VARCHAR(200) NOT NULL,
    cnpj_emp VARCHAR(18) NOT NULL UNIQUE,
    area_emp VARCHAR(150),
    email_emp VARCHAR(150) NOT NULL,
    logo_emp VARCHAR(255),
    ativo_emp TINYINT NOT NULL DEFAULT 1
);

CREATE TABLE vaga (
    id_vag INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    id_emp_fk INT NOT NULL,

    titulo_vag VARCHAR(150) NOT NULL,
    descricao_vag TEXT NOT NULL,
    requisitos_vag TEXT NOT NULL,
    modalidade_vag VARCHAR(50) NOT NULL,
    beneficios_vag TEXT,
    data_vag DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),

    FOREIGN KEY (id_emp_fk) REFERENCES empresa(id_emp)
);

CREATE TABLE candidatura (
    id_cand INT NOT NULL AUTO_INCREMENT PRIMARY KEY,

    id_est_fk INT NOT NULL,
    id_vag_fk INT NOT NULL,

    data_cand DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),

    FOREIGN KEY (id_est_fk) REFERENCES estudante(id_est),
    FOREIGN KEY (id_vag_fk) REFERENCES vaga(id_vag)
);


INSERT INTO empresa (razao_emp, cnpj_emp, area_emp, email_emp, logo_emp)
VALUES
('Tech Solutions LTDA', '12.345.678/0001-90', 'Desenvolvimento de Software', 'contato@techsolutions.com', NULL),
('AgroTech Brasil', '98.765.432/0001-55', 'Tecnologia para o Agronegócio', 'contato@agrotech.com', NULL),
('Inova Educações', '45.987.123/0001-77', 'Educação e Tecnologia', 'suporte@inovaedu.com', NULL);

INSERT INTO estudante (nome_est, email_est, curso_est, stacks_est, github_est, portfolio_est)
VALUES
('Ana Paula Silva', 'ana.silva@example.com', 'Análise e Desenvolvimento de Sistemas', 'C#, SQL, JavaScript', 'https://github.com/ana', NULL),
('Carlos Eduardo Souza', 'carlos.souza@example.com', 'Sistemas de Informação', 'Java, Spring, MySQL', 'https://github.com/carlosedu', NULL),
('Mariana Ferreira', 'mariana.ferreira@example.com', 'Ciência da Computação', 'Python, Django, Machine Learning', 'https://github.com/marifer', NULL),
('João Henrique Ramos', 'joao.ramos@example.com', 'Engenharia de Software', 'React, Node.js, MongoDB', 'https://github.com/joaoh', NULL);



INSERT INTO vaga (id_emp_fk, titulo_vag, descricao_vag, requisitos_vag, modalidade_vag, beneficios_vag)
VALUES
(1, 'Desenvolvedor Backend Júnior', 'Atuar no desenvolvimento de APIs em C# e .NET.', 'C#, .NET, SQL', 'Remoto', 'Vale refeição, plano de saúde'),
(1, 'Estágio em QA', 'Auxiliar na criação de testes automatizados.', 'Noções de testes, Git', 'Presencial', 'Bolsa + VT'),
(2, 'Analista de Dados Jr', 'Trabalhar com análise e modelagem de dados.', 'SQL, Python, Power BI', 'Híbrido', 'Plano de saúde + VR'),
(3, 'Dev Front-End', 'Atuar com React e integração com APIs.', 'JavaScript, React, HTML/CSS', 'Remoto', 'Horário flexível');

INSERT INTO candidatura (id_est_fk, id_vag_fk)
VALUES
(1, 1), -- Ana -> Backend Jr
(2, 1), -- Carlos -> Backend Jr
(3, 3), -- Mariana -> Analista de Dados
(4, 4), -- João -> Dev Front-end
(1, 2); -- Ana -> Estágio em QA



