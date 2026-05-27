# PhoneBook API (DDD + Clean Architecture) — .NET 8/9

این پروژه یک پیاده‌سازی سبک و قابل‌گسترش از **دفترچه تلفن (PhoneBook)** است که با رویکرد **Domain-Driven Design (DDD)** و **Clean Architecture** توسعه داده شده است. هدف پروژه نمایش طراحی صحیح لایه‌ها، مدل دامنه غنی، و مدیریت خطا به‌صورت استاندارد و قابل تست است.

> برای توضیحات کامل معماری، تصمیم‌های طراحی، جزئیات Result Pattern، نگاشت خطاها و استراتژی تست‌ها، فایل  
> **[`docs/CoverLetter.md`](docs/CoverLetter.md)** (یا نسخه PDF آن) را مطالعه کنید.

---

## Architecture Overview

پروژه از چهار لایه تشکیل شده است:

- **PhoneBook.Domain**
  - هسته دامنه و قوانین بیزینس (بدون وابستگی به API/DB)
  - `Contact` به عنوان **Aggregate Root**
  - `PhoneNumber` به عنوان **Value Object** (اعتبارسنجی در سطح دامنه)
  - کدهای خطا: `DomainErrorCode`

- **PhoneBook.Application**
  - Use Caseها و سرویس‌های برنامه (Orchestration)
  - DTOها برای ورودی/خروجی
  - **Result / Result<T>** برای خروجی قابل پیش‌بینی و بدون Exception به‌عنوان Flow Control
  - منبع پیام‌های فارسی: `ErrorMessages` (متمرکز)

- **PhoneBook.Infrastructure**
  - پیاده‌سازی Repository
  - In-Memory storage با ساختار Thread-safe (مثلاً `ConcurrentDictionary`)

- **PhoneBook.Api**
  - REST API + Swagger
  - کنترلرهای Thin
  - **ResultFilter** برای تبدیل خودکار `Result` به HTTP response استاندارد

---

## Key Features

- Result Pattern (`Result`, `Result<T>`) برای یکپارچگی خروجی‌ها
- Centralized Persian error messages (`ErrorMessages`)
- Thin Controllers + Global `ResultFilter`
- Unit Tests با `xUnit` و `FluentAssertions`
- Swagger/OpenAPI برای تست تعاملی API

---

## Tech Stack

- .NET 8 / 9
- xUnit, FluentAssertions (Tests)
- Swagger / OpenAPI
- (Optional) Moq در صورت نیاز به Mock

---

## Run & Test

### Prerequisites
- نصب .NET SDK

### Run API
```bash
git clone https://github.com/HNaeemaei8/Hasin-PhoneBook.git
cd PhoneBook