# SonaeTest

O projeto foi dividido em 2 pastas, frontend e backend.
Em um único repositorio, conforme o solicitado na especificação.


Foram criadas 2 branchs para desenvolvimento separados, frontend e backend.
A branch consolide possui o código consolidado de ambos os ambientes.


O Layout do front está simplificado, sem uso de bibliotecas:
![image](https://github.com/user-attachments/assets/2fdde6c9-55cc-41b4-afc6-207e2cffd942)

Ao clicar em Criar nova encomenda, pode-se solicitar nova encomenda de produtos ao informar a quantidade.
![image](https://github.com/user-attachments/assets/effdb252-ad15-46eb-bae3-2a5d843b43a4)

Na coluna ações, é possível finalizar uma encomenda:
![image](https://github.com/user-attachments/assets/e08605a9-f3c3-468d-b0f9-60742ac27c01)



Nas tela principal são entregues todos os requisitos (user stories), sendo:
- Gestão das encomendas com quantidade de produto.
- Criação de nova encomenda, com reserva automática por x tempo
- Após X tempo, expira-se automaticamente a reserva de encomenda, e passa para o estado expirado. Em seguida, devolve a quantidade expirada para estoque disponível.
- Finalização (completed) de uma encomenda que ainda esteja ativa



# Front-End
Em ReactJs com typescript, criado com vitejs. 
Foi utilizado yarn para criá-lo.
Para inicar em desenvolvimento: yarn vite .
Foi adicionada uma camada de proxy e configurações no backend para fugir do impacto de CORS, por rodar ambos em localhost.



# Back-End
Em .NET Core 8 (.NET 8)
O projeto foi elaborado seguindo elementos de DDD, TDD, camadas e foco em Clean Architecture, com o objetivo da segregação das responsabilidades e, por consequência, melhor testabilidade do software.
Como não foi criada base de dados, foram iniciadas as dependências com escopo Singleton (de propósito) para se manter a memória das entidades entre os requests;
A camada de Repositório imita persistência, porém os elementos estão somente na memória principal (RAM) (Singleton);
Foi adicionado na camada de configuração (startup), se em ambiente desenvolvimento, tratamento para CORS e header para requests de mesma origem.
O projeto conta com 5 camadas, sendo:
- Aplicação
- Dominio
- Infraestrutura
- Serviços
- Testes

Nesse sentido, e pelo tempo de teste, a camada de Teste se focou principalmente nas regras de negócio principais da camada Services, sendo:
- Adicionar Encomenda e variações
- Encomendas com quantidades além do estoque
- Completar encomenda e variações
- Processar expiração de encomendas que devem vender após x tempo

Não foi criada nenhuma exceção personalizada (de domínio), pois não houve necessidade.
Qualquer exceção não tratada, irá ser encaminhada para o filtro de exceção padrão (HttpResponseExceptionFilter) que foi adicionado na camada de configuração (startup).


Optou-se por não enfatizar complexidade extra, como criar classes base para services ou repositório, pois se trata de um projeto com 2 serviços e sem base de dados.
Foi criado classes de validação em encomendas para auxiliar nos testes, com uso de FluentValidation.

Seguindo os conceitos do TDD,
Não se achou necessário criar uma classe para produtos, pois isso é irrelevante para o contexto do user stories, já que ele se limite a gestão de encomendas e quantidade de itens.

Optou-se por fazer uso de um HostedService, que irá avaliar o estoque a cada tempo determinado para realizar a expiração (status) das encomendas vencidas.
