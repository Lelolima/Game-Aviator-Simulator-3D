# Aviator Simulator 3D

> **Estado do repositório (auditoria de 2026-11-22):** este repositório contém apenas
> **scripts C# avulsos** para Unity. Não há projeto Unity (`Assets/`, `ProjectSettings/`,
> cenas, prefabs, modelos ou sons), portanto não é possível abrir e jogar diretamente.
> As descrições abaixo são o design pretendido; veja "O que está implementado" para o que existe no código.

## Visão Geral
Aviator Simulator 3D é um simulador de voo realista que oferece uma experiência imersiva de pilotagem de aeronaves. O jogo combina física de voo realista, gráficos detalhados e um sistema de missões envolvente para proporcionar uma experiência completa de simulação de voo.

## Características Principais

### Sistema de Voo Realista
- Física de voo detalhada com sustentação, resistência do ar e efeitos de estol
- Controles completos de aeronave (pitch, roll, yaw)
- Efeitos de clima dinâmicos, incluindo chuva e neblina
- Sistema de dano e combustível que afetam o desempenho da aeronave

### Sistema de Missões
- Missões variadas com objetivos específicos
- Sistema de checkpoints para guiar o jogador
- Recompensas baseadas em desempenho
- Limite de tempo para completar missões

### Recursos Visuais e Sonoros
- Efeitos visuais de dano e clima
- Efeitos sonoros realistas do motor
- Sistema de câmera que segue a aeronave
- HUD (Heads-Up Display) com informações importantes

### Outros Recursos
- Sistema de pontuação
- Sistema de salvamento de progresso
- Menu principal com opções de configuração
- Seleção de diferentes aeronaves

## Controles

### Controles Básicos
- **W/S ou Setas Cima/Baixo**: Controle de pitch (inclinar para cima/baixo)
- **A/D ou Setas Esquerda/Direita**: Controle de roll (girar no eixo) e yaw (virar para os lados)
- **Espaço**: Ligar/desligar motor
- **R**: Reabastecer (quando próximo a um ponto de reabastecimento)

### Controles Adicionais
- **M**: Abrir mapa (minimapa do `HUDAvancado`)
- **ESC** (pausar) e **C** (alternar câmera): descritos no design, **não implementados** nos scripts

## Mecânicas de Jogo

### Sistema de Dano
- A aeronave sofre dano ao colidir com objetos
- Danos afetam o desempenho da aeronave
- Efeitos visuais indicam o nível de dano
- Reparos podem ser realizados em pontos específicos

### Sistema de Combustível
- Consumo contínuo de combustível durante o voo
- Pontos de reabastecimento espalhados pelo mapa
- Interface visual mostra o nível atual de combustível
- Ficar sem combustível: hoje `Combustivel.cs` apenas registra "Sem combustível!" no log; o motor não é desligado
- Observação: o consumo atual é contínuo enquanto o componente está ativo, mesmo com o motor desligado

### Sistema de Missões
- Complete checkpoints em sequência
- Cumpra objetivos dentro do tempo limite
- Ganhe pontos ao completar missões
- Desbloqueie novas áreas e aeronaves

## Requisitos Técnicos

Não há build do jogo; requisitos de hardware não foram medidos. Para usar os scripts é preciso o editor Unity com o pacote **Post Processing** (v2), usado por `EfeitosVisuais.cs`.

## Desenvolvimento

### Tecnologias Utilizadas
- **Motor de Jogo**: Unity
- **Linguagem de Programação**: C#
- **Física**: Sistema de física integrado do Unity
- **Áudio**: Sistema de áudio do Unity

### O que está implementado (scripts na raiz)
- **FisicaDeVoo.cs**: física de voo (motor, sustentação, arrasto, estol, vento, turbulência)
- **Aerodinamica.cs**: fórmulas puras (densidade do ar, força aerodinâmica, ângulo de ataque), testáveis sem Unity
- **ControleDeVoo.cs**, **CameraSeguir.cs**, **HUD.cs**, **HUDAvancado.cs**, **SomDeVoo.cs**
- **Combustivel.cs**, **SistemaDeReabastecimento.cs**
- **Dano.cs**, **SistemaDano.cs**, **EfeitosVisuais.cs**
- **Checkpoint.cs**, **GerenciadorDeMissao.cs**, **SistemaDeMissoes.cs**, **Pontuacao.cs**, **SistemaDeSalvamento.cs**
- **Clima.cs**, **SistemaClima.cs**, **MenuPrincipal.cs**
- **teste_jogo.html**: protótipo/maquete em HTML+JS que **simula** a interface no navegador; não executa os scripts C#

## Como usar os scripts

1. Crie um projeto 3D no Unity e instale o pacote Post Processing (Package Manager).
2. Copie os arquivos `.cs` da raiz para `Assets/Scripts/`.
3. Monte a cena (aeronave com `Rigidbody`, `AudioSource`, `FisicaDeVoo`, `Combustivel`, `Dano` etc.) e ligue as referências no Inspector.

Esses passos não foram executados na auditoria (sem Unity disponível).

## Testes e verificação

```bash
# Requer .NET SDK 10
dotnet test Tests/Logica            # testes reais da lógica pura (Aerodinamica)
dotnet build ci/compile-check       # compila todos os scripts contra stubs mínimos da API do Unity
```

- `ci/compile-check` detecta tipos/membros inexistentes e erros de C#, mas **não substitui** compilar no editor: os stubs não implementam comportamento.
- `Tests/EditMode/FisicaDeVooTest.cs` é um teste do Unity Test Framework; só roda no Test Runner do editor.
- O CI (`.github/workflows/ci.yml`) executa os dois comandos acima.

## Variáveis de ambiente

Nenhuma.

## Contribuição

Contribuições são bem-vindas! Para contribuir com o projeto, siga estas etapas:

1. Faça um fork do repositório.
2. Crie um branch para sua feature ou correção de bug:
   ```bash
   git checkout -b minha-feature
   ```
3. Faça commit das suas alterações:
   ```bash
   git commit -m 'Adiciona minha feature'
   ```
4. Envie para o branch original:
   ```bash
   git push origin minha-feature
   ```
5. Crie um Pull Request.

## Créditos

- **Desenvolvimento**: Equipe Aviator Simulator
- **Arte e Design**: Equipe de Arte Aviator
- **Som**: Equipe de Áudio Aviator
- **Testes**: Equipe de QA Aviator

## Suporte

Para suporte técnico ou dúvidas sobre o jogo, entre em contato através do email: lelolima806@gmail.com

---

© 2023 Aviator Simulator Team. Todos os direitos reservados.
