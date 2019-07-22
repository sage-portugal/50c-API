# Sage50c-API
Sage 50c API e eXtensibilidade

Utilizar o exemplo da API com Visual Studio 2019

1. Requisitos
É necessário ter a aplicação Sage 50c instalada no computador

2. Abrir e utilizar o exemplo
2.1. Clonar o o código exemplo:
![alt text](https://github.com/sage-portugal/50c-API/blob/master/doc/images/clone.png)

2.2. Abrir a solução com o visual studio 2019
2.3. Na primeira vez que se abre o projeto, é possivel que todas as referências à API não estejam a apontar para o lugar correto.
Por isso, é necessário removê-las para as voltar a adicionar:
![alt text](https://github.com/sage-portugal/50c-API/doc/images/refsRemove.png)

2.4. A localização tipica dos INTEROPS é:
C:\Program Files (x86)\Common Files\sage\2070\50c2018\Interops
Devem ser adicionadas todas as DLLs presentes nesta pasta.

2.5. Depois de adicionados os INTEROPS como referências o VS2019 deve ficar assim:
![alt text](https://github.com/sage-portugal/50c-API/doc/images/refs.png)

2.5. Com todas as referências selecionadas, aceder a propriedades e definir o seguinte:
Embed Interop Types = FALSE
![alt text](https://github.com/sage-portugal/50c-API/doc/images/embedinterops.png)

2.6. Por último, indicar que pretendemos uma compilação x86:
![alt text](https://github.com/sage-portugal/50c-API/doc/images/x86.png)

E é tudo. É só compiplar e disfrutar
