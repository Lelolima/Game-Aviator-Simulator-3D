# Auditoria 2026-11-22 — Aviator Simulator 3D

## Diagnóstico (estado anterior)

Scripts C# para Unity soltos na raiz (sem `Assets/`, `ProjectSettings/`, cenas ou assets). Sem CI (`gh run list` vazio). Unity **não** está disponível nesta máquina; a análise foi estática + compilação contra stubs + testes de lógica pura com .NET 10.

| # | Problema | Severidade | Evidência |
|---|----------|-----------|-----------|
| 1 | Os tipos `Dano`, `Combustivel` e `Pontuacao` (e `ControleDeVoo`, `CameraSeguir`, `HUD`, `SomDeVoo`, `Clima`, `GerenciadorDeMissao`, `SistemaDeSalvamento`) estavam num arquivo **sem extensão** (`Game AViator Simulator 3D`), que o Unity não compila. Scripts dependentes não compilariam | P1 | Compilação do commit original contra stubs: `CS0246 "Dano"` (EfeitosVisuais.cs:18), `"Combustivel"` (HUDAvancado.cs:8, SistemaDeReabastecimento.cs:16), `"Pontuacao"` (SistemaDeMissoes.cs:64) |
| 2 | Campos privados acessados por outras classes: `Combustivel.combustivelAtual`, `Pontuacao.pontuacaoAtual` | P1 | `CS0122` em HUDAvancado.cs:112/131, SistemaDeReabastecimento.cs:103-117, SistemaDeSalvamento.cs:10/19 |
| 3 | Arquivo sem extensão definia um segundo `Checkpoint`, que colidiria com `Checkpoint.cs` | P1 | conteúdo do arquivo |
| 4 | `Tests/Tests.csproj` (net6 + NUnit) usa `UnityEngine` sem referência a ele, e o teste acessa membros privados (`rb`, `AplicarForcasAerodinamicasAvancadas`): nunca compilou | P2 | leitura do csproj e do teste |
| 5 | Artefatos de build (`Tests/obj/`) commitados; sem `.gitignore` | P2 | `git ls-files` |
| 6 | `NullReferenceException` se não houver `AudioSource` (`motorSom`) ou texto de UI em `Combustivel`/`Pontuacao` | P2 | FisicaDeVoo.cs, Combustivel.cs, Pontuacao.cs |
| 7 | README promete ESC (pausa), C (câmera) e "falha do motor sem combustível", que não existem; lista requisitos de hardware sem build; não diz que falta o projeto Unity; `teste_jogo.html` é uma simulação em JS, não o jogo | P2 | grep por `KeyCode` e leitura de `Combustivel` |

Segredos: busca por regex em todo o histórico — nada encontrado (gitleaks indisponível). O README tem um email de contato público, intencional. Dependências: só pacotes NuGet de teste; `dotnet list package --vulnerable` → nenhum vulnerável.

## Mudanças

- Arquivo sem extensão dividido em um `.cs` por classe (nome do arquivo = nome da classe, como o Unity exige). O `Checkpoint` duplicado (versão antiga e menor) foi descartado em favor de `Checkpoint.cs`.
- `combustivelAtual` e `pontuacaoAtual` viraram `[HideInInspector] public`.
- Guardas de nulo para `motorSom`, textos de UI e `curvaLift`; `rb` exposto (`[HideInInspector] public`) e `AplicarForcasAerodinamicasAvancadas` público, para o teste EditMode existente compilar.
- `Aerodinamica.cs`: fórmulas puras extraídas de `FisicaDeVoo` sem mudar o resultado (densidade `exp(-h/10000)`, força `0,5·ρ·v²·C`, ângulo limitado a ±45°).
- `Tests/Logica`: 9 testes NUnit reais da `Aerodinamica`. `Tests/FisicaDeVooTest.cs` movido para `Tests/EditMode/` (só roda no editor). `Tests.csproj` quebrado e `Tests/obj/` removidos; `.gitignore` adicionado.
- `ci/compile-check`: compila todos os scripts contra stubs mínimos da API do Unity.
- CI: compile-check + testes + verificação de pacotes vulneráveis; `permissions: contents: read`; actions fixadas por SHA; sem deploy.
- README: estado real, o que está implementado, como usar e testar.

## Testes executados (localmente, .NET SDK 10.0.404)

- Compile-check no commit original → 4 erros CS0246 (tipos ausentes). Na branch → `Compilação com êxito`, 0 erros.
- `dotnet test Tests/Logica` → Aprovado: 9, Com falha: 0.
- `dotnet list Tests/Logica package --vulnerable` → nenhum pacote vulnerável.

## O que NÃO pôde ser executado

- Compilar e jogar no editor Unity, e rodar `Tests/EditMode` no Test Runner: Unity não está instalado e o repositório não tem projeto Unity.
- Os stubs não validam o comportamento em runtime nem APIs obsoletas (ex.: `Rigidbody.velocity`/`drag` foram renomeados no Unity 6).

## Riscos residuais e decisões do dono

- Escolher a versão do Unity e versionar um projeto completo (`Assets/`, `ProjectSettings/`, cenas).
- Regras de negócio não alteradas: consumo de combustível com motor desligado e falta de combustível sem desligar o motor.
- Existem dois sistemas paralelos (ex.: `Dano` e `SistemaDano`, `Clima` e `SistemaClima`, `HUD` e `HUDAvancado`); decidir qual manter.
