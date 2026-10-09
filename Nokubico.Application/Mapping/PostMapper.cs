using Nokubico.Application.DTOs;
using Nokubico.Domain.Entities;

namespace Nokubico.Application.Mapping;

/// <summary>
/// Mapper para conversão entre entidades de Post e seus DTOs.
/// </summary>
public static class PostMapper
{
    /// <summary>
    /// Converte uma entidade Post para PostDTO com contadores e flags do utilizador.
    /// </summary>
    /// <param name="post">A entidade Post a converter</param>
    /// <param name="currentUserId">ID do utilizador autenticado (opcional) para calcular LikedByMe e BookmarkedByMe</param>
    /// <returns>PostDTO com dados completos</returns>
    public static PostDTO ToDto(Post post, Guid? currentUserId = null)
    {
        ArgumentNullException.ThrowIfNull(post);

        var author = post.Author != null 
            ? ToUserSummary(post.Author)
            : throw new InvalidOperationException("O post não tem autor associado.");

        var likedByMe = false;
        var bookmarkedByMe = false;

        if (currentUserId.HasValue && currentUserId != Guid.Empty)
        {
            likedByMe = post.Likes?.Any(l => l.UserId == currentUserId.Value) ?? false;
            // Nota: Bookmarks não estão carregados diretamente em Post, seria obtido via repositório
            // Esta é uma simplificação - em produção, seria passado como parâmetro
        }

        return new PostDTO(
            Id: post.Id,
            Content: post.Content,
            ImageUrl: post.Image,
            VideoUrl: post.Video,
            CreatedAt: post.CreatedAt,
            Author: author,
            LikesCount: post.Likes?.Count ?? 0,
            CommentsCount: post.Comments?.Count ?? 0,
            SharesCount: 0, // Nota: Shares não estão carregados em Post collection, seria obtido via repositório
            LikedByMe: likedByMe,
            BookmarkedByMe: bookmarkedByMe);
    }

    /// <summary>
    /// Converte um utilizador para UserSummaryDTO (resumo seguro).
    /// </summary>
    public static UserSummaryDTO ToUserSummary(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserSummaryDTO(
            Id: user.Id,
            Name: user.Name,
            Image: user.Image,
            Profession: user.Profession);
    }

    /// <summary>
    /// Converte uma entidade Comment para CommentDTO.
    /// </summary>
    public static CommentDTO ToDto(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);

        var author = comment.Author != null
            ? ToUserSummary(comment.Author)
            : throw new InvalidOperationException("O comentário não tem autor associado.");

        return new CommentDTO(
            Id: comment.Id,
            Content: comment.Content ?? string.Empty,
            CreatedAt: comment.CreatedAt,
            Author: author);
    }

    /// <summary>
    /// Converte uma entidade Like para LikeDTO.
    /// </summary>
    public static LikeDTO ToDto(Like like)
    {
        ArgumentNullException.ThrowIfNull(like);

        return new LikeDTO(
            UserId: like.UserId,
            PostId: like.PostId,
            CreatedAt: like.CreatedAt);
    }

    /// <summary>
    /// Converte uma entidade Share para ShareDTO.
    /// </summary>
    public static ShareDTO ToDto(Share share)
    {
        ArgumentNullException.ThrowIfNull(share);

        return new ShareDTO(
            UserId: share.UserId,
            PostId: share.PostId,
            CreatedAt: share.CreatedAt);
    }

    /// <summary>
    /// Converte uma entidade Bookmark para BookmarkDTO.
    /// </summary>
    public static BookmarkDTO ToDto(Bookmark bookmark)
    {
        ArgumentNullException.ThrowIfNull(bookmark);

        return new BookmarkDTO(
            UserId: bookmark.UserId,
            PostId: bookmark.PostId,
            CreatedAt: bookmark.CreatedAt);
    }

    /// <summary>
    /// Aplica dados de CreatePostDTO a uma nova entidade Post.
    /// </summary>
    public static Post ToEntity(CreatePostDTO dto, Guid authorId)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (authorId == Guid.Empty)
            throw new ArgumentException("O ID do autor não pode ser vazio.", nameof(authorId));

        dto.Validate();

        return new Post(authorId, dto.Content, dto.ImageUrl, dto.VideoUrl);
    }

    /// <summary>
    /// Aplica dados de UpdatePostDTO a uma entidade Post existente.
    /// </summary>
    public static void ApplyUpdate(UpdatePostDTO dto, Post post)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(post);

        if (!string.IsNullOrEmpty(dto.Content))
            post.SetContent(dto.Content);

        if (!string.IsNullOrEmpty(dto.ImageUrl))
            post.SetImage(dto.ImageUrl);

        if (!string.IsNullOrEmpty(dto.VideoUrl))
            post.SetVideo(dto.VideoUrl);
    }

    /// <summary>
    /// Cria uma entidade Comment a partir de CreateCommentDTO.
    /// </summary>
    public static Comment ToEntity(CreateCommentDTO dto, Guid authorId, Guid postId)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (authorId == Guid.Empty)
            throw new ArgumentException("O ID do autor não pode ser vazio.", nameof(authorId));

        if (postId == Guid.Empty)
            throw new ArgumentException("O ID do post não pode ser vazio.", nameof(postId));

        if (string.IsNullOrWhiteSpace(dto.Content))
            throw new ArgumentException("O conteúdo do comentário não pode estar vazio.", nameof(dto.Content));

        return new Comment(authorId, postId, dto.Content);
    }
}
