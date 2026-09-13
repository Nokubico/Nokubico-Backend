# Esquema MVP normalizado para PostgreSQL

## Convenções PostgreSQL

- Todos os nomes físicos devem usar `snake_case`; os nomes camelCase abaixo representam o modelo lógico da aplicação.
- IDs internos e todas as FKs usam `uuid` com `gen_random_uuid()` como default. `text` fica reservado para conteúdo livre, URLs e referências externas.
- Datas usam `timestamptz` (UTC), não `timestamp` sem timezone.
- Valores monetários usam `bigint` em unidades mínimas da moeda (ex.: cêntimos), nunca `float`; `currency` usa `char(3)` ISO 4217.
- Toda FK deve apontar para uma PK/UK existente. Aplicar `ON DELETE CASCADE` em tabelas de ligação e `ON DELETE RESTRICT` em histórico financeiro/pedidos.
- Campos obrigatórios devem ser `NOT NULL`. Adicionar `CHECK` para valores monetários positivos e limites de rating.

## Integridade e índices obrigatórios

Aplicar estas restrições no DDL:

| Tabela | Restrição |
|--------|-----------|
| `account` | `UNIQUE (providerId, accountId)` |
| `like`, `share`, `bookmark` | `UNIQUE (userId, postId)` e FKs para `user(id)`/`post(id)` |
| `follow` | `UNIQUE (followerId, followingId)`, `CHECK (followerId <> followingId)` e duas FKs para `user(id)` |
| `user_skill`, `job_skill`, `creative_request_skill` | PK composta pelos dois IDs e FKs para as tabelas relacionadas |
| `company_member` | `UNIQUE (companyId, userId)` e FKs para `company(id)`/`user(id)` |
| `company_follow` | `UNIQUE (companyId, userId)` e FKs para `company(id)`/`user(id)` |
| `conversation_participants` | `UNIQUE (conversationId, userId)` e FKs para `conversations(id)`/`user(id)` |
| `job_application` | `UNIQUE (jobId, userId)` e FKs para `job(id)`/`user(id)` |
| `creative_request_claim` | `UNIQUE (requestId, creatorId)` e FKs para `creative_request(id)`/`user(id)` |
| `review` | `UNIQUE (productId, userId)`, `CHECK (rating BETWEEN 1 AND 5)` |
| `wallet_txs` | FK para `wallets(id)`; nunca permitir alteração ou exclusão de transações financeiras |
| `order_item` | FKs para `order(id)`/`product(id)` e `CHECK (price >= 0)` |
| `message_attachments` | FK para `messages(id)` com `ON DELETE CASCADE` |

Criar índices em todas as FKs e, no mínimo, em `post(authorId, createdAt)`, `product(status, category)`, `job(companyId, isActive)`, `messages(conversationId, createdAt)`, `notification(userId, read)` e `wallet_txs(walletId, createdAt)`. Índices não substituem FKs nem constraints `UNIQUE`.

## Table `user`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `uuid` | Primary, Default `gen_random_uuid()` |
| `email` | `varchar(255)` | Not null, Unique |
| `name` | `varchar(120)` | Not null |
| `image` | `text` |  Nullable |
| `bio` | `text` |  Nullable |
| `location` | `text` |  Nullable |
| `profession` | `text` |  Nullable |
| `createdAt` | `timestamptz` | Not null, Default `now()` |
| `updatedAt` | `timestamptz` | Not null, Default `now()` |
| `role` | `UserRole` |  |
| `emailVerified` | `bool` | Not null, Default `false` |

## Table `session`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `uuid` | Primary, Default `gen_random_uuid()` |
| `expiresAt` | `timestamptz` | Not null |
| `token` | `text` | Not null, Unique |
| `createdAt` | `timestamptz` | Not null, Default `now()` |
| `userId` | `uuid` | Not null, FK `user(id)` |

## Table `account`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `uuid` | Primary, Default `gen_random_uuid()` |
| `accountId` | `text` | Not null |
| `providerId` | `text` | Not null |
| `userId` | `uuid` | Not null, FK `user(id)` |
| `accessToken` | `text` |  Nullable |
| `refreshToken` | `text` |  Nullable |
| `password` | `text` |  Nullable |
| `createdAt` | `timestamptz` | Not null, Default `now()` |
| `updatedAt` | `timestamptz` | Not null, Default `now()` |

## Table `verification`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `identifier` | `text` |  |
| `value` | `text` |  |
| `expiresAt` | `timestamp` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `post`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `content` | `text` |  |
| `image` | `text` |  Nullable |
| `video` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |
| `authorId` | `text` |  |
| `sharedPostId` | `text` |  Nullable |

## Table `like`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `createdAt` | `timestamp` |  |
| `userId` | `text` |  |
| `postId` | `text` |  |

## Table `comment`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `content` | `text` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |
| `authorId` | `text` |  |
| `postId` | `text` |  |

## Table `share`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `createdAt` | `timestamp` |  |
| `userId` | `text` |  |
| `postId` | `text` |  |

## Table `bookmark`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `createdAt` | `timestamp` |  |
| `userId` | `text` |  |
| `postId` | `text` |  |

## Table `product`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `uuid` | Primary, Default `gen_random_uuid()` |
| `title` | `text` |  |
| `slug` | `text` |  |
| `description` | `text` |  Nullable |
| `category` | `text` |  |
| `price` | `bigint` | Not null, `CHECK (price >= 0)` |
| `currency` | `char(3)` | Not null |
| `thumbnail` | `text` |  |
| `license` | `text` |  |
| `downloadUrl` | `text` |  Nullable |
| `creatorId` | `text` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |
| `status` | `ProductStatus` |  |

## Table `product_image`

| Name | Type | Constraints |
|------|------|-------------|
| `productId` | `uuid` | Primary, Foreign key `product(id)` |
| `imageUrl` | `text` | Primary |
| `position` | `smallint` | Not null, `CHECK (position >= 0)` |

`UNIQUE (productId, position)` evita duas imagens na mesma posição. Esta tabela substitui o array `images` para manter a primeira forma normal.

## Table `wallets`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `userId` | `text` |  |
| `balance` | `bigint` | Not null, `CHECK (balance >= 0)` |
| `currency` | `char(3)` | Not null |
| `status` | `WalletStatus` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `wallet_txs`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `walletId` | `text` |  |
| `type` | `TxType` |  |
| `amount` | `bigint` | Not null, `CHECK (amount > 0)` |
| `balanceBefore` | `bigint` | Not null |
| `balanceAfter` | `bigint` | Not null |
| `description` | `text` |  |
| `ref` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |

## Table `deposits`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `walletId` | `text` |  |
| `amount` | `bigint` | Not null, `CHECK (amount > 0)` |
| `method` | `DepositMethod` |  |
| `status` | `DepositStatus` |  |
| `providerReference` | `text` |  Nullable |
| `confirmedAt` | `timestamp` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `withdrawals`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `walletId` | `text` |  |
| `amount` | `bigint` | Not null, `CHECK (amount > 0)` |
| `fee` | `bigint` | Not null, `CHECK (fee >= 0)` |
| `netAmount` | `bigint` | Not null, `CHECK (netAmount >= 0)` |
| `method` | `WithdrawalMethod` |  |
| `status` | `WithdrawalStatus` |  |
| `accountReference` | `text` |  |
| `processedAt` | `timestamp` |  Nullable |
| `rejectReason` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `escrow_txs`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `buyerWalletId` | `text` |  |
| `sellerWalletId` | `text` |  |
| `orderId` | `text` |  |
| `productId` | `text` |  Nullable |
| `amount` | `bigint` | Not null, `CHECK (amount > 0)` |
| `platformFee` | `bigint` | Not null, `CHECK (platformFee >= 0)` |
| `sellerReceives` | `bigint` | Not null, `CHECK (sellerReceives >= 0)` |
| `status` | `EscrowStatus` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `Download`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `userId` | `text` |  |
| `productId` | `text` |  |
| `orderId` | `text` |  |
| `createdAt` | `timestamp` |  |

## Table `follow`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `followerId` | `text` |  |
| `followingId` | `text` |  |
| `createdAt` | `timestamp` |  |

## Table `notification`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `userId` | `text` |  |
| `type` | `text` |  |
| `title` | `text` |  |
| `content` | `text` |  |
| `read` | `bool` |  |
| `link` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |

## Table `skill`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `name` | `text` |  |
| `category` | `text` |  Nullable |

## Table `user_skill`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `userId` | `text` | Primary |
| `skillId` | `text` | Primary |

## Table `company`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `name` | `text` |  |
| `logo` | `text` |  Nullable |
| `description` | `text` |  Nullable |
| `website` | `text` |  Nullable |
| `location` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |
| `ownerId` | `text` |  Nullable |
| `isVerified` | `bool` |  |

## Table `job`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `title` | `text` |  |
| `description` | `text` |  |
| `location` | `text` |  Nullable |
| `type` | `text` |  |
| `salaryMin` | `bigint` | Nullable, `CHECK (salaryMin >= 0)` |
| `salaryMax` | `bigint` | Nullable, `CHECK (salaryMax >= salaryMin)` |
| `currency` | `char(3)` | Nullable |
| `companyId` | `text` |  |
| `isActive` | `bool` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |
| `postedById` | `text` |  Nullable |

## Table `job_skill`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `jobId` | `text` | Primary |
| `skillId` | `text` | Primary |

## Table `user_block`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `blockerId` | `text` |  |
| `blockedId` | `text` |  |
| `createdAt` | `timestamp` |  |

## Table `user_report`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `reporterId` | `text` |  |
| `reportedId` | `text` |  |
| `reason` | `text` |  |
| `details` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |

## Table `conversations`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `title` | `text` |  Nullable |
| `isGroup` | `bool` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `conversation_participants`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `conversationId` | `text` |  |
| `userId` | `text` |  |
| `joinedAt` | `timestamp` |  |
| `lastReadAt` | `timestamp` |  Nullable |

## Table `messages`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `conversationId` | `text` |  |
| `senderId` | `text` |  Nullable |
| `content` | `text` |  |
| `messageType` | `MessageType` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |
| `replyToId` | `text` |  Nullable |

## Table `creative_request`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `clientId` | `text` |  |
| `companyId` | `text` |  Nullable |
| `title` | `text` |  |
| `description` | `text` |  |
| `budget` | `bigint` | Nullable, `CHECK (budget >= 0)` |
| `status` | `CreativeRequestStatus` |  |
| `acceptedById` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `creative_request_skill`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `requestId` | `text` | Primary |
| `skillId` | `text` | Primary |

## Table `creative_request_claim`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `requestId` | `text` | Foreign key |
| `creatorId` | `text` | Foreign key |
| `createdAt` | `timestamp` |  |

## Table `job_application`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `jobId` | `text` | Foreign key |
| `userId` | `text` | Foreign key |
| `status` | `JobApplicationStatus` |  |
| `coverLetter` | `text` | Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `budgets`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `conversationId` | `text` |  |
| `creatorId` | `text` |  |
| `clientId` | `text` |  |
| `title` | `text` |  |
| `description` | `text` |  Nullable |
| `estimatedValue` | `bigint` | Not null, `CHECK (estimatedValue >= 0)` |
| `currency` | `text` |  |
| `status` | `BudgetStatus` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `contracts`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `conversationId` | `text` |  |
| `creatorId` | `text` |  |
| `clientId` | `text` |  |
| `title` | `text` |  |
| `content` | `text` |  |
| `status` | `ContractStatus` |  |
| `signedAt` | `timestamp` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `deliverables`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `conversationId` | `text` |  |
| `creatorId` | `text` |  |
| `clientId` | `text` |  |
| `title` | `text` |  |
| `fileUrl` | `text` |  |
| `fileName` | `text` |  |
| `status` | `DeliverableStatus` |  |
| `sentAt` | `timestamp` |  Nullable |
| `confirmedAt` | `timestamp` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `order`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `userId` | `text` |  |
| `total` | `bigint` | Not null, `CHECK (total >= 0)` |
| `currency` | `char(3)` | Not null |
| `status` | `text` |  |
| `paymentStatus` | `text` |  |
| `paymentMethod` | `text` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `order_item`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `orderId` | `text` |  |
| `productId` | `text` |  |
| `price` | `bigint` | Not null, `CHECK (price >= 0)` |
| `title` | `text` |  |
| `license` | `text` |  |

## Table `review`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `productId` | `text` |  |
| `userId` | `text` |  |
| `rating` | `int4` |  |
| `comment` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `message_attachments`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `messageId` | `text` |  |
| `fileUrl` | `text` |  |
| `fileName` | `text` |  |
| `mimeType` | `text` |  |
| `createdAt` | `timestamp` |  |

## Table `erasure_requests`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `userId` | `text` |  |
| `status` | `ErasureStatus` |  |
| `requestedAt` | `timestamp` |  |
| `processedAt` | `timestamp` |  Nullable |

## Table `company_member`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `companyId` | `text` |  |
| `userId` | `text` |  |
| `role` | `CompanyMemberRole` |  |
| `joinedAt` | `timestamp` |  |

## Table `company_follow`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `companyId` | `text` |  |
| `userId` | `text` |  |
| `createdAt` | `timestamp` |  |

## Table `company_post`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `companyId` | `text` |  |
| `authorId` | `text` |  |
| `content` | `text` |  |
| `image` | `text` |  Nullable |
| `video` | `text` |  Nullable |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `company_ad`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `companyId` | `text` |  |
| `title` | `text` |  |
| `body` | `text` |  Nullable |
| `imageUrl` | `text` |  Nullable |
| `linkUrl` | `text` |  Nullable |
| `status` | `AdStatus` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Table `company_event`

### Columns

| Name | Type | Constraints |
|------|------|-------------|
| `id` | `text` | Primary |
| `companyId` | `text` |  |
| `authorId` | `text` |  |
| `title` | `text` |  |
| `description` | `text` |  Nullable |
| `location` | `text` |  Nullable |
| `isOnline` | `bool` |  |
| `eventUrl` | `text` |  Nullable |
| `startsAt` | `timestamp` |  |
| `endsAt` | `timestamp` |  |
| `isPublished` | `bool` |  |
| `createdAt` | `timestamp` |  |
| `updatedAt` | `timestamp` |  |

## Custom Types / Enums

### `MessageType`

`TEXT` | `IMAGE` | `FILE` | `VIDEO` | `AUDIO` | `DOCUMENT` | `SYSTEM` | `AI` | `PROPOSAL` | `CONTRACT` | `PAYMENT_REQUEST`

### `WalletStatus`

`ACTIVE` | `SUSPENDED` | `FROZEN` | `CLOSED`

### `TxType`

`DEPOSIT` | `WITHDRAWAL` | `ESCROW_HOLD` | `ESCROW_RELEASE` | `ESCROW_REFUND` | `FEE` | `ADJUSTMENT` | `STORE_SALE` | `STORE_PURCHASE`

### `DepositMethod`

`MULTICAIXA` | `STRIPE` | `BANK_TRANSFER`

### `DepositStatus`

`PENDING` | `CONFIRMED` | `FAILED` | `EXPIRED`

### `WithdrawalMethod`

`MULTICAIXA` | `BANK_TRANSFER` | `TPA`

### `WithdrawalStatus`

`PENDING` | `PROCESSING` | `COMPLETED` | `REJECTED` | `CANCELLED`

### `EscrowStatus`

`PENDING` | `HELD` | `DELIVERED` | `CONFIRMED` | `RELEASED` | `REFUNDED` | `DISPUTED` | `DISPUTE_RESOLVED` | `EXPIRED`

### `UserRole`

`USER` | `ADMIN` | `COMPANY`

### `CreativeRequestStatus`

`OPEN` | `CLAIMED` | `CLOSED` | `CANCELLED` | `DISPUTED`

### `JobApplicationStatus`

`PENDING` | `INTERVIEWING` | `HIRED` | `REJECTED`

### `BudgetStatus`

`DRAFT` | `SENT` | `APPROVED` | `REJECTED` | `REVISED` | `EXPIRED`

### `ContractStatus`

`DRAFT` | `SENT` | `SIGNED` | `REJECTED` | `EXPIRED`

### `DeliverableStatus`

`PENDING` | `SENT` | `DOWNLOADED` | `CONFIRMED` | `EXPIRED`

### `ErasureStatus`

`PENDING` | `PROCESSING` | `COMPLETED` | `REJECTED`

### `CompanyMemberRole`

`OWNER` | `ADMIN` | `MEMBER`

### `AdStatus`

`PENDING` | `ACTIVE` | `PAUSED` | `ENDED` | `REJECTED`

### `ProductStatus`

`PENDING` | `APPROVED` | `REJECTED`

