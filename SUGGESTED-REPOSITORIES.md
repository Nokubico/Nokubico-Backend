# Sugestão de Repositories — NOkubico Backend

Documento de design sobre **quais repositórios criar**, **o que cada um traz** (dados e responsabilidades) e **como cargar dados relacionados** (`Include`/`ThenInclude`) de forma segura e coerente, evitando repositórios, serviços ou endpoints que depois ninguém usa.

> **Base de dados:** todo o mapeamento fala com **um único PostgreSQL** (o configurado em `appsettings.Development.json`). O índice de busca (Algolia/Elasticsearch) e o object storage (S3/MinIO) previstos na ficha técnica **não são repositórios** — são `Nokubico.Infra.Services` (ver nota final).

---

## 1. Princípios que guiam esta proposta

| # | Princípio | Consequência |
|---|-----------|--------------|
| 1 | **Um repositório por agregado raíz** (DDD) | `User`, `Post`, `Product`, `Order`, `Wallet`, `Conversation`, `Company` = 1 repo cada. Tabelas-filhas **não** têm repo próprio. |
| 2 | **Tabelas de detalhe/associação vivem dentro do agregado** | `Like`, `Comment`, `Share`, `Bookmark` pertencem ao agregado `Post`; `OrderItem` ao `Order`; `ProductImage` ao `Product`; `ConversationParticipant`/`MessageAttachment` ao `Conversation`. |
| 3 | **Os repositórios não contêm lógica de negócio** | Regras (escrow, verificação de email, ranking) vivem nos **serviços de aplicação** (`Nokubico.Application/Services`), que orquestam repositórios e transações. |
| 4 | **Leitura vs. escrita** | Métodos de listagem usan `select` (projeção) + paginação; nunca devolvem campos sensíveis (hash de password, tokens). |
| 5 | **YAGNI** | Não criar repositórios para tabelas planeadas no `SQLnokubico.md` (jobs, notificações, etc.) até que a feature exista. |
| 6 | **Segurança por scoping** | Toda consulta filtra pelo utilizador autenticado (`userId`/claims) para evitar IDOR. |

---

## 2. Mapa agregado → repositório

```
Aggregate Root          Repository             Tabelas que toca (via Include)
─────────────────────────────────────────────────────────────
User        ──────►    UserRepository         user, account, session
Post        ──────►    PostRepository         post, like, comment, share, bookmark
Product     ──────►    ProductRepository      product, product_image, review
Order       ──────►    OrderRepository        order, order_item
Wallet      ──────►    WalletRepository       wallets, wallet_txs
Conversation──────►    ConversationRepository conversations, conversation_participants, messages, message_attachments
Company     ──────►    CompanyRepository      company, company_member, company_follow
```

> Nota: `Account` e `Session` aparecen como "tocadas" por `UserRepository`, mas recomendo **dois repositórios pequenos** dedicados (`AccountRepository`, `SessionRepository`) porque são usados intensamente pelo `AuthService` e pelo middleware de autenticação — ver secção 3.

---

## 3. Repositories sugeridos (detalhe por repositório)

### 3.1 UserRepository — `user`

**O que traz:** gestão de perfiles, registro e verificação de email (RF001). Devolve perfil público seguro.

| Método | Include / Trae | Segurança |
|--------|----------------|-----------|
| `findByEmail(email)` | — (solo `user`) | Para login; usado pelo AuthService; nunca devuelve en DTO |
| `findById(id)` | — (solo `user`) | Interno |
| `findPublicProfileById(id)` | `user.image`, `bio`, `location`, `profession` (projeção) | **Nunca** carrega `Accounts`/`Sessions` en respuestas públicas |
| `register(dados)` | — | Persiste user + crea `wallet` inicial en la misma transacción |
| `updateProfile(...)` | — | Scoping: el usuario solo edita su propio perfil |
| `verifyEmail(userId)` | — | Marca `emailVerified = true` |

**Índices:** `user(email)` (único), `user(role)`.

### 3.2 SessionRepository — `session`

**O que traz:** sesiones de login/logout, token único y expiración. Es el soporte del middleware de autenticación.

| Método | Include / Trae | Seguridad |
|--------|----------------|-----------|
| `create(userId, expiresAt)` | — | Token aleatório seguro, `UNIQUE` |
| `findActiveByToken(token)` | **`Include(Session.User)`** | Solo trae user activo (no expirado); base del `Middleware` de auth |
| `revoke(token)` / `revokeAllForUser(userId)` | — | Logout único / logout de todas las sesiones |
| `deleteExpired()` | — | Job de limpieza |

**Índices:** `session(token)` (único), `session(userId)`.

### 3.3 AccountRepository — `account`

**O que traz:** vinculación de providers OAuth y credenciales email/senha (RF001 con OAuth).

| Método | Include / Trae | Seguridad |
|--------|----------------|-----------|
| `findByProviderAndAccountId(providerId, accountId)` | — | `UNIQUE(providerId, accountId)` — no duplica cuentas |
| `listByUser(userId)` | — | Para "mis cuentas vinculadas" |
| `link(account)` / `unlink(userId, providerId)` | — | Vincular/desvincular Google, GitHub, etc. |
| `updateTokens(...)` / `setPassword(...)` | — | Password siempre hasheado (bcrypt/Argon2, RNF003) |

### 3.4 PostRepository — `post`

**O que traz:** la comunidad (RF013/RF014). Feed, publicar, curtir, comentar, partilhar y guardar. Es el repositorio más usado en lecturas, por eso **los contadores se resuelven con subqueries** (no cargando todas las filas).

| Método | Include / Trae | Seguridad |
|--------|----------------|-----------|
| `findFeed(cursor, currentUserId)` | Autor (`name`, `image`) + contadores (likes, comments, shares) + flags del user actual (`liked`, `bookmarked`) | Público; **nunca** emails |
| `findById(id, currentUserId)` | `Include(Post.Author)` + `Include(Post.SharedPost)` (repost) + contadores | Detalle de un post |
| `findByAuthor(userId, page)` | Ídem feed | Paginado |
| `create` / `update` / `delete` | — | Scoping: solo el autor (o admin) |
| `like/unlike` , `addComment`, `share`, `bookmark` | — | Métodos que operan sobre el agregado; cada uno con su `UNIQUE(userId, postId)` |

> **Include recomendado (pseudocódigo EF):**
> ```csharp
> query.Include(p => p.Author)
>      .Include(p => p.SharedPost)
> // contadores: subqueries, NO Include(p.Likes) cargando todo
> ```

**Índices:** `post(authorId, createdAt)` — clave del feed; `like(userId, postId)`, `comment(postId)`, `share(postId)`, `bookmark(postId)`.

### 3.5 ProductRepository — `product`

**O que traz:** el marketplace (RF002, RF009, RF010). Catálogo público, galería, evaluaciones y el tablero del vendedor.

| Método | Include / Trae | Seguridad |
|--------|----------------|-----------|
| `findPublished(filtros, page)` | `Include(Product.Images)` + `Include(Product.Reviews)` + media de rating | Solo `Published`; filtros por categoría/precio |
| `findById(id, currentUserId)` | `Include(Product.Images)` + `Include(Product.Creator)` | Draft/Disabled solo owner o admin |
| `findByCreator(userId, page)` | Imágenes + estado | Dashboard vendedor; solo suyo |
| `create` (con imágenes) / `update` / `changeStatus` | — | Solo el `creatorId` |
| `addReview(userId, rating, comment)` | — | `UNIQUE(productId, userId)`; rating 1–5 |

> **Cuidado de seguridad:** `downloadUrl` **nunca** se devuelve en el catálogo; solo después de una compra confirmada (el `OrderService` lo expone vía `Download`).

**Índices:** `product(status, category)`, `product(creatorId)`, `product(slug)`.

### 3.6 OrderRepository — `order`

**O que traz:** compras con escrow (RF004, RF005, RF006, RF011). Histórico de pedidos para comprador y vendedor.

| Método | Include / Trae | Seguridad |
|--------|----------------|-----------|
| `create(order, items)` | — | En transacción con `WalletRepository` (hold escrow) |
| `findByIdForUser(id, userId)` | `Include(Order.Items)` + producto snapshot | Solo comprador, vendedor o admin |
| `listByBuyer(userId, page)` | Items (títulos snapshot) | Paginado |
| `listBySeller(creatorId, page)` | Items + comprador | Via `OrderItem → Product.creatorId`; paginado |
| `updatePaymentStatus(...)` | — | Controlado por `EscrowService` |

**Índices:** `order(userId)`, `order_item(orderId)`, `order_item(productId)`.

### 3.7 WalletRepository — `wallet`

**O que traz:** saldo, transacciones y el histórico inmutable que sostiene el escrow (RF004, RF011, RNF006).

| Método | Include / Trae | Seguridad |
|--------|----------------|-----------|
| `findByUserId(userId)` | — (solo saldo/estado) | Solo el dueño o admin |
| `findByIdWithTransactions(walletId, page)` | `Include(Wallet.Transactions)` **paginado** | Histórico; nunca cargar todas las tx |
| `addTransaction(wallet, tx)` | — | Verifica `balanceBefore`/`balanceAfter`; `ON DELETE RESTRICT` |
| `updateBalance(...)` | — | Con lock optimista/versión para evitar carreras en pagos |

> **Concurrencia:** en pagos concurrentes, `updateBalance` debe usar `version` o `SELECT ... FOR UPDATE` para no sobrescribir saldos.

**Índices:** `wallet(userId)`, `wallet_txs(walletId, createdAt)`.

### 3.8 ConversationRepository — `conversation`

**O que traz:** chat de negociación y pruebas de entrega (RF003, RF010).

| Método | Include / Trae | Seguridad |
|--------|----------------|-----------|
| `create(participants)` | — | Crea conversa + participantes |
| `findById(id, userId)` | `Include(Participants)` + `Include(Messages)` + `ThenInclude(Messages.Attachments)` | **Solo participantes** pueden leer |
| `listForUser(userId, page)` | Último mensaje + no-leídos | Paginado; mensaje `SYSTEM`/`AI` con `senderId = NULL` |
| `addMessage(...)` / `markRead(...)` / `addAttachment(...)` | — | Solo participantes; `UNIQUE(conversationId, userId)` |

> **Include recomendado (pseudocódigo EF):**
> ```csharp
> query.Include(c => c.Participants)
>      .Include(c => c.Messages)
>      .ThenInclude(m => m.Attachments) // message_attachments
> ```

**Índices:** `messages(conversationId, createdAt)`, `conversation_participants(conversationId, userId)`.

### 3.9 CompanyRepository — `company`

**O que traz:** perfiles públicos de empresas/estudios y membresía (RF008).

| Método | Include / Trae | Seguridad |
|--------|----------------|-----------|
| `findById(id)` | `Include(Company.Members)` + `Include(Company.Follows)` | Público |
| `findByOwner(userId)` | Miembros | Solo OWNER |
| `create` / `update` | — | Solo OWNER/ADMIN (vía `CompanyMember`) |
| `addMember` / `removeMember` | — | `UNIQUE(companyId, userId)` |
| `follow` / `unfollow` | — | `UNIQUE(companyId, userId)` |

---

## 4. Servicios de aplicación (para no meter lógica en los repos)

| Servicio (`Nokubico.Application/Services`) | Orquesta | Trae |
|-------------------------------------------|----------|------|
| `AuthService` | `UserRepository` + `SessionRepository` + `AccountRepository` | Registro, login, logout, OAuth, verificación de email |
| `FeedService` | `PostRepository` | Feed paginado + proyección a DTO |
| `CheckoutService` / `EscrowService` | `OrderRepository` + `WalletRepository` (+ `ProductRepository`) | Crear pedido → **hold escrow** → confirmar entrega → **release** / disputa / refund |
| `MessagingService` | `ConversationRepository` | Enviar mensajes, adjuntos, marcar leído |
| `IIAService` (porta) → `IAService` (Infra.Services/AI) | — | Sugerencias de precio/descuento (RF012) |

> **Regla:** si un flujo toca dos agregados (ej.: compra = `Order` + `Wallet`), la transacción y la orquestación van en el **servicio**, no en un repo.

---

## 5. O que NÃO criar (para no inflar el proyecto)

**Nunca** repositórios sueltos para tablas hijas o de ligación:

| ✗ No crear | Por qué |
|------------|---------|
| `LikeRepository`, `CommentRepository`, `ShareRepository`, `BookmarkRepository` | Hijas del agregado `Post`; se operan via `PostRepository` |
| `ReviewRepository` | Se resuelve como método de `ProductRepository` |
| `OrderItemRepository` | Detalle de `Order` |
| `ProductImageRepository` | Detalle de `Product` |
| `ConversationParticipantRepository`, `MessageAttachmentRepository` | Detalle de `Conversation` |
| `CompanyMemberRepository`, `CompanyFollowRepository` | Detalle de `Company` |
| `JobRepository`, `NotificationRepository`, `SkillRepository`, … | Tablas planeadas sin feature aún (YAGNI) |
| `EscrowRepository` | El escrow es **flujo de servicio**, no una tabla de repos (hasta que exista `escrow_txs`, y aún así se opera via `WalletRepository`/`CheckoutService`) |

**Endpoints que no mapean los RF del README tampoco se crean** (el README ya lista RF001–RF017; el API expone solo esos casos de uso).

---

## 6. Seguridad transversal (aplicar a todos)

- **Scoping por usuario:** cada método recibe `userId`/claims y filtra; nunca listar datos de otro usuario (anti-IDOR).
- **Proyección en lecturas:** `select(...)` para devolver solo columnas necesarias; jamás `password`, `accessToken`, `refreshToken` ni `token` de sesión.
- **Paginación obligatoria** en todas las listas (headers HTTP, patrón del HeathIA).
- **Transacciones** en writes multi-agregado (`CheckoutService`).
- **Índices en toda FK** y en las columnas de filtro/orden de los feeds.
- **N+1:** contadores de feed con subqueries; adjuntos y mensajes con `ThenInclude` (una sola query).
- **Datos sensibles por rol:** `downloadUrl` solo tras compra; `wallet` solo dueño/admin; mensajes solo participantes.

---

## 7. Nota: la "segunda base" (busca y ficheros)

El sistema prevé dos almacenes *externos* adicionales, pero **no son repositorios**:

| Almacén | Dónde vive | Cómo se integra |
|---------|-----------|-----------------|
| Índice de búsqueda (Algolia / Elasticsearch) | `Nokubico.Infra.Services/Search` | Sincronizado desde los casos de uso (indexar al publicar producto/post), nunca consultado por repos |
| Object storage (AWS S3 / MinIO) | `Nokubico.Infra.Services/Storage` | Subir/bajar ficheros; el repositorio guarda solo la **URL** (`image`, `video`, `fileUrl`) |

Los repositorios de esta propuesta hablan **solo con el PostgreSQL configurado**; cualquier otro almacén se conecta en `Infra.Services` y se orquesta desde Application.

---

## 8. Resumen en una tabla

| Repositorio | Tabelas | Trae (dependencias) | Seguridad clave |
|-------------|---------|---------------------|-----------------|
| `UserRepository` | user | perfil seguro, registro, verificación | sin tokens en público |
| `SessionRepository` | session | sesión + user (auth) | token único/expiración |
| `AccountRepository` | account | providers OAuth | UNIQUE provider+account |
| `PostRepository` | post, like, comment, share, bookmark | feed, autor, contadores, flags | público; writes scoped |
| `ProductRepository` | product, product_image, review | galería, reviews, creador | downloadUrl tras compra |
| `OrderRepository` | order, order_item | items snapshot, comprador | scoping comprador/vendedor |
| `WalletRepository` | wallets, wallet_txs | saldo, histórico | inmutable, concurrencia |
| `ConversationRepository` | conversations, participants, messages, attachments | chat, adjuntos, no-leídos | solo participantes |
| `CompanyRepository` | company, member, follow | miembros, seguidores | edición OWNER/ADMIN |

Total: **9 repositórios** — ni uno más ni uno menos para el MVP. Todo lo demás se resuelve en servicios de aplicación y `Infra.Services`.

---

<p align="center">
  <em>NOkubico — Backend · Design Notes</em>
</p>