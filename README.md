# 🧩 SisONG - Sistema de Gestão para ONGs (Front-End)

Este é o projeto **front-end em ASP.NET Core MVC** do sistema **SisONG**, uma plataforma desenvolvida como Trabalho de Conclusão de Curso para apoiar a gestão de Organizações Não Governamentais (ONGs).

O sistema permite o gerenciamento de **voluntários**, **doações**, **eventos**, **produtos (insumos)** e **notificações**, com perfis distintos para administradores, voluntários e doadores.

---

## 🚀 Como Executar o Projeto

### ✅ Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ ou VS Code com extensão C#
- Projeto da API SisONG rodando em paralelo
- MySQL em funcionamento com o banco de dados da API configurado

---

### 🛠️ Passos para Rodar

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/SisONG-FrontEnd.git
   cd SisONG-FrontEnd
   
2. ## Abra o projeto no Visual Studio ou VS Code.

3. ## Verifique a URL da API:
Nos serviços que usam HttpClient, a URL base deve apontar corretamente para sua API:
_httpClient.BaseAddress = new Uri("https://localhost:7057/api/");

4. ## Execute o projeto:
dotnet run

🗂️ Estrutura do Projeto
SisONGFront/
├── Controllers/       # Lógica de controle das views (Home, Admin, Voluntário, etc)
├── Views/             # Páginas renderizadas com Razor
│   └── Shared/        # Layouts e componentes reutilizáveis
├── Dtos/              # Objetos de transferência de dados entre front-end e API
├── Models/            # ViewModels utilizados nas views
├── wwwroot/           # Arquivos estáticos (CSS, JS, imagens)
├── appsettings.json   # Configurações do projeto
└── Program.cs         # Inicialização da aplicação

👤 Perfis de Usuário
Administrador: gerencia produtos, voluntários, doadores, eventos e relatórios.
Voluntário: visualiza eventos disponíveis, histórico de participação e notificações.
Doador: realiza doações, consulta o histórico e recebe confirmações e alertas.

📦 Funcionalidades
🧑‍💼 Administrador
  Cadastro/edição/exclusão de produtos (insumos)
  Listagem de voluntários e doadores
  Gerenciamento de eventos
  Envio de notificações segmentadas
  Geração de relatórios por tipo e período

👥 Voluntário
  Visualização de eventos
  Cadastro de participação
  Histórico pessoal
  Recebimento de notificações

💝 Doador
  Doações financeiras e de insumos
  Histórico de doações
  Notificações de confirmação e alertas

🔄 Comunicação com a API
O front-end se comunica com a API do SisONG utilizando HttpClient. Os dados são enviados e recebidos em formato JSON, e as requisições seguem o padrão RESTful:
var resposta = await _httpClient.GetAsync($"/api/Notificacao/usuario/{usuarioId}");

📌 Observações
O sistema está em desenvolvimento contínuo.
Certifique-se de que a API esteja rodando antes de iniciar o front-end.
Em caso de erro, verifique o console de erros e a URL base da API.
