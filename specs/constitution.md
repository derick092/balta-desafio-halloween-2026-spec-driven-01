# Constituição do projeto

## Stack
- A aplicaçao deve utilizar .Net 10 e C#.
- A API HTTP deve utilizar Minimal APIs.
- A persistência deve utilizar SQLite com Entity Framework Core.
- Os testes automatizados devem utilizar xUnit.
- Nenhuma dependência externa nova deve ser adicionada sem justificativa registrada no plano técnico.

## Arquitetura
- A aplicação deve manter uma estrutura simples, sem divisão obrigatória em múltiplos projetos.
- Os endpoints HTTP não devem acessar o banco de dados diretamente; devem utilizar um serviço de aplicação.
- As regras de negócio devem ficar fora dos endpoints HTTP.
- Interfaces e abstrações só devem ser criadas quando houver uma necessidade concreta.

## Qualidade
- A aplicação deve manter uma estrutura simples, sem divisão obrigatória em múltiplos projetos.
- Os endpoints HTTP não devem acessar o banco de dados diretamente; devem utilizar um serviço de aplicação.
- As regras de negócio devem ficar fora dos endpoints HTTP.
- Interfaces e abstrações só devem ser criadas quando houver uma necessidade concreta.

## Convenções
- Os nomes de classes, métodos, propriedades e variáveis devem seguir as convenções de nomenclatura do C#.
- Métodos que realizam operações de entrada e saída devem utilizar operações assíncronas quando suportadas pelas APIs utilizadas.
- O código deve seguir as convenções de formatação e análise disponibilizadas pelo .NET SDK.

## Governança
- Toda implementação deve atender aos requisitos e critérios de aceite definidos em spec.md.
- As decisões técnicas devem ser registradas em plan.md.
- As tarefas devem ser pequenas, ordenadas e verificáveis, com referência aos requisitos que as originaram.
- Cada implementação deve se limitar à tarefa solicitada em tasks.md.
- Alterações na constituição devem ser justificadas e registradas antes de serem aplicadas.