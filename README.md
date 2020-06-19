# libvw
Código dos sites visaolibertaria.com, visionlibertaria.com, libvw.com e ancap.su

# como compilar
Você vai precisar do Visual Studio 2017, com os módulos: .Net Core 2.1, C#, React. Embora versões mais recentes do Visual Studio possam funcionar, a experiência mostra que portar, mesmo para versões mais recentes pode ser trabalhoso. Melhor ter o VS2017. Você pode baixar a versão community, que é gratuita, aqui https://visualstudio.microsoft.com/pt-br/vs/older-downloads/

É recomendado ter um SQL Server 2012, pode ser o Developer ou o Express, que são gratuitos. Não é necessário, mas usar o banco de dados de homologação na nuvem torna tudo bem demorado.

Você pode baixar o SQL Server, versão Developer ou Express, aqui https://www.microsoft.com/pt-br/sql-server/sql-server-downloads

Se tiver SQL Server 2012 ou posterior, basta criar um banco de dados vazio e importar o backup que está em Resources/a_v001_Lksj88s7jwjshanx.bak

Depois você vai precisar alterar o arquivo LibVisWeb/LibVisWeb/appsetings.json alterando a linha da string de conexão para seu banco de dados local.

# o que precisa saber
O sistema está em C# e React e Redux com Typescript. Então, precisa conhecer essas linguagens e bibliotecas.

C# é o backend do sistema, que tem a biblioteca de acesso ao banco de dados LibVisLib, cujo código faz parte do projeto e todos os Controllers no diretório... Controllers, modelos de dados no diretório Models. O diretório View é um resquicio do VS que praticamente não é usado.

A parte frontend é React com typescript. Portanto é diferente dos Reacts normais por aí. O typescript é um javascript fortemente tipado. Ou seja, tudo tem tipo e se você errar o tipo, ele dá erro. Isso é positivo, porque evita erros difíceis de identificar que fatalmente acontecem em aplicações javascript muito grandes. Você pega o erro mais cedo, mas, para quem trabalha com javascript, é um chute no saco passar para typescript. Porque tem que ficar controlando tipo de tudo. Para quem vem do C# ou Java é menos traumático um pouco.

O frontend tem uma parte no diretório wwwroot/dist, o resto do diretório wwwroot é montado dinamicamente. O grosso do frontend está no diretório ClientApp. Ali você tem o css/style.css que é onde está a parte customizada dos estilos. Copie de um padrão que achei na internet chamado newspaper, então o resto dos css está em ClientApp/theme/newspaper/css as imagens estão em ClientApp/theme/newspaper/img. Outros diretórios no theme são só coisas estáticas que eu uso uma coisa ou outra. Nem sei se uso, de fato, mas como ele só leva para produção o que é usado, não faz diferença.

O diretorio messages tem os arquivos que cuidam da tradução. Tem todas as mensagens do site aqui.

O react é usado com redux, então todas as informações de estado são mantidas em stores. Se você já usou react com redux, sabe o esquema. É a mesma coisa, só 10x mais complicado por causa do typescript, que exige tipo de tudo. Evidentemente, todas as stores estão no diretório stores.

O diretório Models contém os exatos mesmos modelos que o diretório Models do C#. Tem que bater tudo ali, mesmo nome, mesmo campo, infelizmente. Isso poderia ser automatizado, mas não é. Se precisar incluir um campo em qualquer estrutura da Models C#, precisa fazer a mesma mudança em Models JS.

A arquitetura usada é monolítica porque permite fazer o server rendering das páginas javascript. Daí o boot-client e o boot-server que fazem a mesma coisa no cliente e no servidor para permitir indexação. 


