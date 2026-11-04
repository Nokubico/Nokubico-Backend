using System.Globalization;
using System.Text;
using Nokubico.Application.DTOs;
using Nokubico.Application.Interfaces;
using Nokubico.Application.Mapping;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Enums;
using Nokubico.Domain.Interface.Auth;
using Nokubico.Domain.Interface.Marketplace;
using Nokubico.Domain.Pagination;
using Nokubico.Domain.Validation;

namespace Nokubico.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;

    public ProductService(
        IProductRepository productRepository,
        IUserRepository userRepository,
        IOrderRepository orderRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<PagedList<ProductDTO>> GetPublished(
        string? category,
        PaginationParams pagination,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(pagination);

        var products = await _productRepository.FindPublished(category, pagination, cancellationToken);
        return await MapPage(products, cancellationToken);
    }

    public async Task<ProductDTO> GetById(
        Guid id,
        Guid? currentUserId = null,
        CancellationToken cancellationToken = default)
    {
        EnsureId(id, nameof(id));

        var product = await _productRepository.FindPublishedById(id, cancellationToken);
        if (product is not null)
        {
            var publishedStatistics = await _productRepository.GetReviewStatistics([product.Id], cancellationToken);
            return MapProduct(product, publishedStatistics);
        }

        product = await _productRepository.FindById(id, cancellationToken)
            ?? throw new DomainException($"Produto com ID {id} não encontrado.");

        if (product.Status != ProductStatus.Published
            && !await CanManage(product, currentUserId, allowAdministrator: true, cancellationToken))
        {
            throw new DomainException("Não tem autorização para consultar este produto.");
        }

        var statistics = await _productRepository.GetReviewStatistics([product.Id], cancellationToken);
        return MapProduct(product, statistics);
    }

    public async Task<ProductDTO> Create(
        Guid creatorId,
        CreateProductDTO dto,
        CancellationToken cancellationToken = default)
    {
        EnsureId(creatorId, nameof(creatorId));
        ArgumentNullException.ThrowIfNull(dto);
        dto.Validate();

        var creator = await _userRepository.FindById(creatorId, cancellationToken)
            ?? throw new DomainException($"Criador com ID {creatorId} não encontrado.");

        var slug = NormalizeSlug(dto.Slug, dto.Title);
        if (await _productRepository.SlugExists(slug, cancellationToken: cancellationToken))
            throw new DomainException($"Já existe um produto com o slug '{slug}'.");

        var product = ProductMapper.ToEntity(dto with { Slug = slug }, creatorId);
        product.SetCreator(creator);

        var savedProduct = await _productRepository.Save(product, cancellationToken);
        return MapProduct(savedProduct, new Dictionary<Guid, ProductReviewStatistics>());
    }

    public async Task<ProductDTO> Update(
        Guid productId,
        Guid userId,
        UpdateProductDTO dto,
        CancellationToken cancellationToken = default)
    {
        EnsureId(productId, nameof(productId));
        EnsureId(userId, nameof(userId));
        ArgumentNullException.ThrowIfNull(dto);
        dto.Validate();

        var product = await GetProduct(productId, cancellationToken);
        EnsureOwner(product, userId);

        ProductMapper.ApplyUpdate(dto, product);
        var savedProduct = await _productRepository.Save(product, cancellationToken);
        var statistics = await _productRepository.GetReviewStatistics([savedProduct.Id], cancellationToken);
        return MapProduct(savedProduct, statistics);
    }

    public async Task Publish(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        EnsureId(productId, nameof(productId));
        EnsureId(userId, nameof(userId));

        var product = await GetProduct(productId, cancellationToken);
        EnsureOwner(product, userId);
        product.Publish();
        await _productRepository.Save(product, cancellationToken);
    }

    public async Task Disable(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        EnsureId(productId, nameof(productId));
        EnsureId(userId, nameof(userId));

        var product = await GetProduct(productId, cancellationToken);
        EnsureOwner(product, userId);
        product.Disable();
        await _productRepository.Save(product, cancellationToken);
    }

    public async Task Delete(
        Guid productId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        EnsureId(productId, nameof(productId));
        EnsureId(userId, nameof(userId));

        var product = await GetProduct(productId, cancellationToken);
        if (!await CanManage(product, userId, allowAdministrator: true, cancellationToken))
            throw new DomainException("Não tem autorização para eliminar este produto.");

        await _productRepository.Delete(product, cancellationToken);
    }

    public async Task<PagedList<ProductDTO>> GetMyProducts(
        Guid creatorId,
        PaginationParams pagination,
        CancellationToken cancellationToken = default)
    {
        EnsureId(creatorId, nameof(creatorId));
        ArgumentNullException.ThrowIfNull(pagination);

        var products = await _productRepository.FindByCreator(creatorId, pagination, cancellationToken);
        return await MapPage(products, cancellationToken);
    }

    public async Task<ReviewDTO> AddReview(
        Guid productId,
        Guid userId,
        CreateReviewDTO dto,
        CancellationToken cancellationToken = default)
    {
        EnsureId(productId, nameof(productId));
        EnsureId(userId, nameof(userId));
        ArgumentNullException.ThrowIfNull(dto);
        dto.Validate();

        var product = await _productRepository.FindPublishedById(productId, cancellationToken)
            ?? throw new DomainException("Só é possível avaliar um produto publicado.");

        var reviewer = await _userRepository.FindById(userId, cancellationToken)
            ?? throw new DomainException($"Utilizador com ID {userId} não encontrado.");

        if (!await _orderRepository.HasConfirmedPurchase(userId, productId, cancellationToken))
            throw new DomainException("Só é possível avaliar um produto comprado com pagamento confirmado.");

        if (await _productRepository.FindReview(productId, userId, cancellationToken) is not null)
            throw new DomainException("Já existe uma avaliação deste utilizador para o produto.");

        var review = ReviewMapper.ToEntity(dto, productId, userId);
        review.SetUser(reviewer);
        var savedReview = await _productRepository.SaveReview(review, cancellationToken);
        return ReviewMapper.ToDto(savedReview);
    }

    public async Task<double> GetAverageRating(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        EnsureId(productId, nameof(productId));

        if (await _productRepository.FindPublishedById(productId, cancellationToken) is null)
            throw new DomainException($"Produto publicado com ID {productId} não encontrado.");

        return await _productRepository.GetAverageRating(productId, cancellationToken);
    }

    private async Task<Product> GetProduct(Guid productId, CancellationToken cancellationToken)
    {
        return await _productRepository.FindById(productId, cancellationToken)
            ?? throw new DomainException($"Produto com ID {productId} não encontrado.");
    }

    private async Task<bool> CanManage(
        Product product,
        Guid? userId,
        bool allowAdministrator,
        CancellationToken cancellationToken)
    {
        if (!userId.HasValue || userId.Value == Guid.Empty)
            return false;

        if (product.CreatorId == userId.Value)
            return true;

        if (!allowAdministrator)
            return false;

        var user = await _userRepository.FindById(userId.Value, cancellationToken);
        return user?.Role == UserRole.Admin;
    }

    private static void EnsureOwner(Product product, Guid userId)
    {
        if (product.CreatorId != userId)
            throw new DomainException("Só o criador do produto pode alterar este produto.");
    }

    private async Task<PagedList<ProductDTO>> MapPage(
        PagedList<Product> products,
        CancellationToken cancellationToken)
    {
        if (products.Items.Count == 0)
        {
            return new PagedList<ProductDTO>(
                [],
                products.Page,
                products.PageSize,
                products.TotalItems);
        }

        var statistics = await _productRepository.GetReviewStatistics(
            products.Items.Select(product => product.Id).ToArray(),
            cancellationToken);

        var items = products.Items.Select(product => MapProduct(product, statistics)).ToList();

        return new PagedList<ProductDTO>(items, products.Page, products.PageSize, products.TotalItems);
    }

    private static ProductDTO MapProduct(
        Product product,
        IReadOnlyDictionary<Guid, ProductReviewStatistics> statistics)
    {
        statistics.TryGetValue(product.Id, out var reviewStatistics);
        return ProductMapper.ToDto(product, reviewStatistics.AverageRating, reviewStatistics.ReviewsCount);
    }

    private static void EnsureId(Guid id, string parameterName)
    {
        if (id == Guid.Empty)
            throw new DomainException($"O identificador '{parameterName}' não pode ser vazio.");
    }

    private static string NormalizeSlug(string? requestedSlug, string title)
    {
        var source = string.IsNullOrWhiteSpace(requestedSlug) ? title : requestedSlug;
        var decomposed = source.Normalize(NormalizationForm.FormD);
        var slug = new StringBuilder(decomposed.Length);
        var separatorPending = false;

        foreach (var character in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
                continue;

            if (char.IsLetterOrDigit(character))
            {
                if (separatorPending && slug.Length > 0)
                    slug.Append('-');

                slug.Append(char.ToLowerInvariant(character));
                separatorPending = false;
            }
            else
            {
                separatorPending = true;
            }
        }

        if (slug.Length == 0)
            throw new DomainException("Não foi possível gerar um slug válido para o produto.");

        return slug.ToString();
    }
}
