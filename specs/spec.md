# Gerador de Senhas Fortes

## Problema
Usuários precisam gerar senhas fortes sem precisar criar manualmente combinações de caracteres que atendam aos requisitos mínimos.

Também precisam conseguir consultar senhas geradas anteriormente por meio de um identificador único.

## Objetivo
Disponibilizar uma solução que gere senhas fortes, armazene cada senha gerada e permita recuperá-la por meio de um identificador único.

## Usuários
- Consumidores da API que precisam gerar senhas fortes.
- Consumidores da API que precisam recuperar uma senha gerada anteriormente.

## Histórias
- Como consumidor, quero gerar uma senha forte para utilizar em outro sistema.
- Como consumidor, quero receber um identificador único associado à senha gerada para consultá-la posteriormente.
- Como consumidor, quero recuperar uma senha utilizando seu identificador.
- Como consumidor, quero receber uma resposta de erro quando tentar consultar uma senha inexistente.

## Requisitos funcionais
- RF-01: O sistema deve permitir a geração de uma senha forte.
- RF-02: O sistema deve armazenar cada senha gerada.
- RF-03: O sistema deve associar um identificador único a cada senha gerada.
- RF-04: a operação de geração retorna somente o identificador GUID, sem incluir a senha na resposta.
- RF-05: o consumidor pode recuperar a senha utilizando o GUID retornado.
- RF-06: O sistema deve informar um erro quando uma senha não puder ser encontrada pelo identificador fornecido.

## Regras de negócio
- RN-01: Toda senha gerada deve possuir pelo menos 16 caracteres.
- RN-02: Toda senha gerada deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.
- RN-03: Senhas geradas não podem conter espaços em branco.
- RN-04: Cada senha gerada deve possuir um identificador único.
- RN-05: Senhas iguais podem ser geradas mais de uma vez, desde que cada registro possua seu próprio identificador.
- RN-06: Uma senha armazenada deve continuar disponível para consulta após a conclusão da operação de geração.

## Casos de borda
- CB-01: O identificador informado possui formato inválido.
- CB-02: O identificador informado possui formato válido, mas não corresponde a nenhuma senha armazenada.
- CB-03: A senha gerada deve atender simultaneamente a todos os requisitos de composição.
- CB-04: Duas gerações produzem a mesma senha, mas devem continuar associadas a identificadores distintos.

## Fora de escopo
- Autenticação e autorização de usuários.
- Criptografia das senhas armazenadas.
- Histórico de senhas geradas.
- Exclusão de senhas armazenadas.
- Configuração do tamanho da senha pelo consumidor.
- Regras de expiração das senhas.
- Controle de frequência de requisições.

## Critérios de aceite
- CA-01: Ao solicitar uma geração, o sistema deve produzir uma senha que atenda a todas as regras de negócio.
- CA-02: A geração armazena a senha e retorna seu GUID.
- CA-03: Uma consulta com um GUID válido e existente retorna a senha correspondente.
- CA-04: A consulta de um identificador inexistente deve resultar em uma resposta de erro.
- CA-05: A consulta de um identificador malformado deve resultar em uma resposta de erro de validação.
- CA-06: Senhas idênticas geradas em operações distintas devem possuir identificadores distintos.
- CA-07: A geração não exige que o consumidor informe o tamanho da senha.