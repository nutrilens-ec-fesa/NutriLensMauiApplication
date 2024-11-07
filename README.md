# NutriLens - Solução para aprimorar o controle alimentar por meio da exploração nutricional

<br/>
<div align="center">
	<img src="https://github.com/user-attachments/assets/df3033bf-d7c9-4c21-85da-ff47be641348">
</div>
<br/>

## UMA NOVA MANEIRA DE CONTROLAR A SUA ALIMENTAÇÃO

Pensando nesses problemas, desenvolvemos essa solução que vai
ajudar a controlar melhor a sua alimentação, tendo consciência de
boa parte dos nutrientes presentes em suas refeições a partir de
diversas formas de registro.

<br/>
<div align="center">
	<img src="https://github.com/user-attachments/assets/6eef63f6-086d-474d-9d5d-1980ad6585dd">
</div>
<br/>

## ARQUITETURA DA SOLUÇÃO

A arquitetura da solução NutriLens, se utiliza de recursos disponíveis no
dispositivo móvel e computação em nuvem, levando em consideração
conceitos de modularidade e segurança.

<br/>
<div align="center">
	<img src="https://github.com/user-attachments/assets/ef1243b5-c34e-43db-9bdd-aeedcfdb7e97">
</div>
<br/>

1. Como o ponto de entrada para a nossa solução, temos o usuário, que faz toda a
interação com o sistema a partir de um smartphone.

2. A câmera do dispositivo móvel é usada para captura das fotos dos alimentos e
detecção de códigos de barras.

3. Utilizamos um recurso embarcado para detecção de código de barras, a partir
das imagens da câmera.

4. O microfone é utilizado para a forma de registro de refeição por voz.
   
5. Os dados locais são usados para poder sincronizar localmente os recursos de
nuvem, principalmente para possibilitar menos tráfego de rede e utilização da
aplicação offline.

6. Para acessar os recursos de nuvem, utilizamos um canal com autenticação e
requisições utilizando HTTPS, tudo isso para proteger suas informações na
internet.

7. Utilizamos uma API em ASP.NET hospedada no serviço de WebApp da Azure,
aumentando consideravelmente a disponibilidade e escalabilidade da solução.

8. A API faz interface com os recursos de Inteligência Artificial: GPT-4 Turbo e
Gemini 1.0 Pro.

9. Além de também acessar nosso banco de dados em MongoDB.
  
10. Na nossa base de dados armazenamos a base de dados dos alimentos
industrializados, a tabela TACO e os dados dos usuários da solução.
