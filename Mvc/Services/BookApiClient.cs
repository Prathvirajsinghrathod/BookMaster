using System.Net.Http.Json;
using BookMaster.Mvc.Models;

namespace BookMaster.Mvc.Services;

public class BookApiClient
{
    private readonly HttpClient _http;
    public BookApiClient(HttpClient http) => _http = http;

    // Users
    public async Task<List<UserVm>> GetUsersAsync() =>
        await _http.GetFromJsonAsync<List<UserVm>>("users") ?? new();

    public async Task<UserVm?> GetUserAsync(long id) =>
        await _http.GetFromJsonAsync<UserVm>($"users/{id}");

    public async Task<List<BookVm>> GetUserLibraryAsync(long id) =>
        await _http.GetFromJsonAsync<List<BookVm>>($"users/{id}/library") ?? new();

    public async Task<HttpResponseMessage> CreateUserAsync(UserVm user) =>
        await _http.PostAsJsonAsync("users", new { user.Name, user.Email });

    // Categories
    public async Task<List<CategoryVm>> GetCategoriesAsync() =>
        await _http.GetFromJsonAsync<List<CategoryVm>>("categories") ?? new();

    public async Task<HttpResponseMessage> CreateCategoryAsync(string name) =>
        await _http.PostAsJsonAsync("categories", new { Name = name });

    // Books
    public async Task<List<BookVm>> GetBooksAsync(string? search = null, long? categoryId = null) =>
        await _http.GetFromJsonAsync<List<BookVm>>(
            $"books?search={search}&categoryId={categoryId}") ?? new();

    public async Task<BookVm?> GetBookAsync(long id) =>
        await _http.GetFromJsonAsync<BookVm>($"books/{id}");

    public async Task<HttpResponseMessage> CreateBookAsync(CreateBookVm book) =>
        await _http.PostAsJsonAsync("books", book);

    public async Task<HttpResponseMessage> DeleteBookAsync(long id) =>
        await _http.DeleteAsync($"books/{id}");

    // Exchange listings
    public async Task<List<ExchangeListingVm>> GetListingsAsync() =>
        await _http.GetFromJsonAsync<List<ExchangeListingVm>>("exchangelistings") ?? new();

    public async Task<HttpResponseMessage> CreateListingAsync(CreateExchangeListingVm listing) =>
        await _http.PostAsJsonAsync("exchangelistings", listing);

    public async Task<HttpResponseMessage> DeleteListingAsync(long id) =>
        await _http.DeleteAsync($"exchangelistings/{id}");

    // Exchange requests
    public async Task<List<ExchangeRequestVm>> GetRequestsAsync() =>
        await _http.GetFromJsonAsync<List<ExchangeRequestVm>>("exchangerequests") ?? new();

    public async Task<HttpResponseMessage> CreateRequestAsync(CreateExchangeRequestVm request) =>
        await _http.PostAsJsonAsync("exchangerequests", request);

    public async Task<HttpResponseMessage> AcceptRequestAsync(long id) =>
        await _http.PostAsync($"exchangerequests/{id}/accept", null);

    public async Task<HttpResponseMessage> RejectRequestAsync(long id) =>
        await _http.PostAsync($"exchangerequests/{id}/reject", null);

    // History
    public async Task<List<HistoryVm>> GetUserExchangeHistoryAsync(long userId) =>
        await _http.GetFromJsonAsync<List<HistoryVm>>($"users/{userId}/exchange-history") ?? new();
}
