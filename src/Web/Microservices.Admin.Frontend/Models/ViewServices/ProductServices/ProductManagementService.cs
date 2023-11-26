using System.Text.Json;
using Microservices.Admin.Frontend.Models.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestSharp;

namespace Microservices.Admin.Frontend.Models.ViewServices.ProductServices;

public class ProductManagementService : IProductManagementService
{
    private readonly RestClient restClient;
    private IHttpContextAccessor _contextAccessor;
    private string _accessToken = null;
    public ProductManagementService(RestClient restClient, IHttpContextAccessor contextAccessor)
    {
        this.restClient = restClient;
        _contextAccessor = contextAccessor;
    }

    private async Task<string> GetAccessToken()
    {
        if (!string.IsNullOrWhiteSpace(_accessToken))
            return _accessToken;

        _accessToken = await _contextAccessor?.HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, "access_token");

        return _accessToken;
    }

    public List<ProductDto>? GetProducts()
    {
        var request = new RestRequest("/api/ProductManagement", Method.GET);
        request.AddHeader("Authorization", $"Bearer {GetAccessToken().Result}");
        IRestResponse response = restClient.Execute(request);
        var products = JsonSerializer.Deserialize<List<ProductDto>>(response.Content);
        return products;
    }

    public ResultDto UpdateName(UpdateProductDto updateProduct)
    {
        var request = new RestRequest($"/api/ProductManagement", Method.PUT);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", $"Bearer {GetAccessToken().Result}");
        string serializeModel = JsonSerializer.Serialize(updateProduct);
        request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
        IRestResponse response = restClient.Execute(request);
        return GetResponseStatusCode(response);
    }

    private static ResultDto GetResponseStatusCode(IRestResponse response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return new ResultDto(false);
        }
        else
        {
            return new ResultDto(false, response.ErrorMessage);
        }
    }
}