# Tarefas

| ID | Tarefa | Origem | Depende de | Concluída quando |
|---|---|---|---|---|
| T-01 | Criar os projetos da API e de testes | Constituição, D-01, D-03 | — | A solução compila e os testes podem ser executados |
| T-02 | Configurar o SQLite e o Entity Framework Core | RF-02, RN-06, D-02 | T-01 | O contexto de dados pode persistir e consultar registros |
| T-03 | Criar o modelo de senha armazenada | RF-03, RN-04, RN-05, D-08 | T-02 | O modelo possui GUID único e valor da senha, e permite registros com senhas iguais |
| T-04 | Implementar e testar a geração de senhas | RF-01, RN-01, RN-02, RN-03, CB-03, D-06 | T-01 | Testes unitários verificam tamanho mínimo, categorias obrigatórias, ausência de espaços e aleatoriedade segura |
| T-05 | Implementar o serviço de aplicação para geração | RF-02, RF-03, RF-04, RN-04, RN-05, RN-06, D-05 | T-03, T-04 | A senha é gerada, armazenada e seu GUID é retornado pelo serviço |
| T-06 | Implementar o serviço de aplicação para consulta | RF-05, RF-06, RF-07, RN-06, D-05 | T-03, T-05 | O serviço recupera a senha correta ou informa que o registro não existe |
| T-07 | Implementar o endpoint de geração | RF-01, RF-04, CA-02, CA-03, D-07 | T-05 | O endpoint retorna HTTP 201 com somente o GUID |
| T-08 | Implementar o endpoint de consulta | RF-05, RF-06, CA-04, CA-05, CA-06, D-07 | T-06 | O endpoint retorna HTTP 200 para um GUID existente, HTTP 400 para formato inválido e HTTP 404 para GUID inexistente |
| T-09 | Criar testes de integração dos endpoints | CA-03, CA-04, CA-05, CA-06, CA-07, CA-08 | T-07, T-08 | Os caminhos felizes e os principais erros possuem testes de integração |
| T-10 | Executar a validação final do projeto | Constituição, todos os critérios de aceite | T-01 a T-09 | A aplicação compila sem erros ou warnings, todos os testes passam e a aplicação inicia sem erros |