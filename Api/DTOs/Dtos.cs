namespace BookMaster.Api.DTOs;

public record UserDto(long Id, string Name, string Email);
public record CreateUserDto(string Name, string Email);

public record CategoryDto(long Id, string Name);
public record CreateCategoryDto(string Name);

public record BookDto(long Id, string Title, long OwnerId, long CategoryId, string Status);
public record CreateBookDto(string Title, long OwnerId, long CategoryId);
public record UpdateBookDto(string Title, long CategoryId, string Status);

public record NotificationDto(long Id, long UserId, bool IsRead);

public record ExchangeListingDto(long Id, long BookId, string WantedType);
public record CreateExchangeListingDto(long BookId, string WantedType);

public record ExchangeRequestDto(long Id, long ListingId, long RequesterId, long OfferedBookId, string Status);
public record CreateExchangeRequestDto(long ListingId, long RequesterId, long OfferedBookId);

public record HistoryDto(long Id, long RequestId, DateTime? CompletedAt);
