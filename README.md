# Estag.io – API de Gerenciamento de Estágios 🖥️

## Sobre o projeto 📋

A Estag.io é uma API desenvolvida em C# (.NET 8) com MySQL para auxiliar no gerenciamento de oportunidades de estágio.

Ela permite:

- Cadastrar empresas
- Cadastrar estudantes
- Cadastrar vagas de estágio
- Realizar candidaturas em vagas
- Consultar vagas e candidaturas

O projeto foi desenvolvido para a disciplina de Programação com Acesso a Banco de Dados, seguindo o padrão REST e utilizando Entity Framework Core.

## Tecnologias utilizadas 🔧

- C#
- .NET 8 (ASP.NET Web API)

- Entity Framework Core
- MySQL
- Postman / Swagger (para testes)

## Estrutura do projeto 📝

O projeto está organizado da seguinte forma:

- Controllers → rotas e endpoints da API
- Models → entidades (tabelas do banco de dados)
- Dtos → objetos de transferência e validações
- DataContext → conexão com o banco de dados

## Como executar o projeto ▶

1️⃣ Clone o repositório
git clone <https://github.com/karinydobis/projeto-pabd-Estag.io>

2️⃣ Crie o banco de dados

Execute o script .sql no MySQL criando o banco estagio_db. O script fica na pasta DataContexts

3️⃣ Configure a string de conexão

No arquivo appsettings.json, edite a conexão para seu MySQL local:

"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=estagio_db;user=root;password=sua_senha;"
}

4️⃣ Execute o projeto

Abra o projeto no Visual Studio e execute com: dotnet run

Ou apenas clique em Iniciar ▶.

5️⃣ Teste no navegador ou Postman
