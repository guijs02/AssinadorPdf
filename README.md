# 🖋️ Assinador de PDF com Blazor, .NET, MassTransit e SignalR

Este projeto tem como objetivo permitir que usuários façam o upload de documentos PDF e imagens de assinaturas, processando-os de forma assíncrona para gerar um arquivo PDF assinado, pronto para download.

---

## 🚀 Tecnologias Utilizadas

- [ASP.NET Core](https://learn.microsoft.com/aspnet/core) — Web API
- [Blazor Server](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) — Frontend
- [MassTransit](https://masstransit.io/) + [RabbitMQ](https://www.rabbitmq.com/) — Mensageria assíncrona
- [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction) — Comunicação em tempo real
- [iText7](https://itextpdf.com/en/products/itext-7) — Manipulação e assinatura de arquivos PDF

---

## 💻 Funcionalidades

- 📂 Upload de arquivos PDF e imagem da assinatura (PNG/JPEG)
- ✍️ Inserção automática da assinatura no rodapé da primeira página do PDF
- ⚡ Processamento assíncrono com feedback em tempo real usando SignalR
- ⬇️ Link para download do PDF assinado

---

## 📁 Estrutura do Projeto

```bash
/AssinadorPDF
│
├── WebApi/                      # ASP.NET Core com processamento do PDF
│   ├── Controllers/
│   ├── Consumers/               # Consumer do RabbitMQ com MassTransit
│   ├── SignalR/
│   └── wwwroot/pdfs/           # PDFs processados
│
├── Frontend/                    # Projeto Blazor Server
│   ├── Pages/
│   └── Shared/
│
└── Assets/                      # Assinaturas temporárias
