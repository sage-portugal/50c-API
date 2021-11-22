# Sage50c-API
## Sage 50c API e eXtensibilidade

## Utilizar o exemplo da API com Visual Studio 2019<br/>

### 1. Requisitos<br/>
É necessário ter a aplicação Sage 50c instalada no computador<br/>

### 2. Abrir e utilizar o exemplo<br/>
#### 2.1. Clonar o o código exemplo <br/>
![alt text](https://github.com/sage-portugal/50c-API/blob/master/doc/images/clone.png)

#### 2.2. Abrir a solução com o visual studio 2019<br/>
#### 2.3. Na primeira vez que se abre o projeto, é possivel que as referências à API não estejam a apontar para o lugar correto.<br/>
Se for o caso, é necessário removê-las para as voltar a adicionar. Exemplo com os Interops da versão 2021 ou anteriores (necessário substituir pelas da versão 2022):<br/>
![alt text](https://github.com/sage-portugal/50c-API/blob/master/doc/images/refsRemove.png)

#### 2.4. A localização tipica dos INTEROPS é:<br/>
C:\Program Files (x86)\Common Files\sage\2070\50c2022\Interops<br/>
Devem ser adicionadas todas as DLLs presentes nesta pasta.<br/>

#### 2.5. Depois de adicionados os INTEROPS como referências o VS2019 deve ficar assim:<br/>
![alt text](https://github.com/sage-portugal/50c-API/blob/master/doc/images/refs.png)

#### 2.5. Com todas as referências selecionadas, aceder a propriedades e definir o seguinte:<br/>
Embed Interop Types = FALSE<br/>
![alt text](https://github.com/sage-portugal/50c-API/blob/master/doc/images/embedInterops.png)

#### 2.6. Por último, indicar que pretendemos uma compilação x86:<br/>
![alt text](https://github.com/sage-portugal/50c-API/blob/master/doc/images/x86.png)

E é tudo. É só compilar e disfrutar<br/>
