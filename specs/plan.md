# Plano técnico

## Contexto
O projeto implementa um gerador de senhas fortes que permite recuperar senhas geradas anteriormente por meio de um identificador único.

A implementação deve atender aos requisitos definidos em spec.md e respeitar as regras estabelecidas em constitution.md.

O projeto terá uma estrutura simples, adequada ao objetivo de aprender Spec-Driven Development.

## Arquitetura
- A solução será composta por um projeto de API e um projeto de testes.
- A API utilizará Minimal APIs para expor as operações HTTP.
- Os endpoints serão responsáveis por receber requisições, validar entradas HTTP e produzir respostas.
- Um serviço de aplicação será responsável por coordenar a geração, persistência e consulta das senhas.
- A geração da senha será isolada em um componente responsável por aplicar as regras de composição.
- O acesso aos dados será realizado por meio do Entity Framework Core.
- A lógica de negócio não deve ficar nos endpoints.
- Não serão criadas camadas ou abstrações adicionais sem necessidade demonstrável.

## Decisões
D-01: Utilizar .NET 10 e Minimal APIs

Justificativa: atende à stack definida na constituição e permite implementar os dois endpoints com pouca estrutura adicional.

D-02: Utilizar SQLite com Entity Framework Core

Justificativa: o sistema precisa persistir as senhas para permitir consultas posteriores, e SQLite atende a essa necessidade sem exigir um servidor de banco de dados separado.

D-03: Utilizar xUnit para os testes

Justificativa: atende à constituição e permite testar as regras de negócio e o comportamento HTTP.

D-04: Separar a geração de senhas da camada HTTP

Justificativa: permite testar as regras de composição sem depender de requisições HTTP ou do banco de dados.

D-05: Utilizar um serviço de aplicação

Justificativa: centraliza o fluxo de geração, armazenamento e recuperação das senhas, evitando acesso direto aos dados pelos endpoints.

D-06: Utilizar um gerador de números aleatórios criptograficamente seguro

Justificativa: senhas fortes exigem imprevisibilidade. Será utilizada a funcionalidade disponível no .NET, sem adicionar uma dependência externa.

D-07: Definir os contratos HTTP

Justificativa: respostas consistentes facilitam o consumo da API e a validação dos critérios de aceite.

POST /passwords: gera e armazena uma senha. Retorna HTTP 201 e o GUID associado no corpo da resposta, sem incluir a senha.

GET /passwords/{id}: recupera a senha associada ao GUID informado. Retorna HTTP 200 e a senha no corpo da resposta.

GUID malformado: retorna HTTP 400 com ProblemDetails.

GUID válido sem registro correspondente: retorna HTTP 404 com ProblemDetails.

D-08: Utilizar identificadores GUID

Justificativa: atende à especificação e permite identificar cada registro independentemente do conteúdo da senha.

D-09: Manter o escopo reduzido

Justificativa: o objetivo é aprender SDD, sem introduzir complexidade desnecessária.

Não serão utilizados MediatR, CQRS, repositórios genéricos ou bibliotecas adicionais de validação.

## Modelo de dados
Senha armazenada

Cada registro deve conter:

- Id: GUID único do registro.
- Value: senha gerada e armazenada.

## Contratos
O identificador será a chave primária do registro.

Senhas iguais poderão existir em registros distintos, desde que seus identificadores sejam diferentes.

Gerar senha
- Método: POST.
- Caminho: /passwords.
- Entrada: nenhuma.
- Sucesso: HTTP 201 com o GUID no corpo da resposta.
- Efeito: gerar a senha, persistir o registro e retornar seu identificador.
- A senha não deve ser incluída na resposta de geração.

Consultar senha
- Método: GET.
- Caminho: /passwords/{id}.
- Entrada: GUID no caminho.
- Sucesso: HTTP 200 com a senha correspondente.
- GUID malformado: HTTP 400 com ProblemDetails.
- GUID inexistente: HTTP 404 com ProblemDetails.

## Riscos
- Uma senha pode ser gerada sem atender a todas as categorias obrigatórias se a composição não for validada por testes.
- Uma falha na persistência pode impedir a recuperação posterior da senha.
- A implementação pode retornar a senha na operação de geração por interpretar incorretamente o contrato.
- O formato inválido de GUID pode ser tratado como recurso inexistente se a validação não for implementada explicitamente.
- A solução não é adequada para produção, pois não contempla os controles de segurança e operação necessários para um serviço real.