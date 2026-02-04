using Azure;
using Microsoft.AspNetCore.Http;
using Pustok.Business.Dtos;
using System.Net;

namespace Pustok.MVC.Handlers;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        var httpContext = _httpContextAccessor.HttpContext;

        httpContext!.Request.Cookies.TryGetValue("AccessToken", out var token);

    restartRequest:


        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);


        if (!response.IsSuccessStatusCode)
        {

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                httpContext!.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken);


                if (!string.IsNullOrEmpty(refreshToken))
                {
                    HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, $"https://localhost:44342/api/Auth/RefreshToken?token={refreshToken}");

                    var refreshTokenResponse = await base.SendAsync(httpRequestMessage, cancellationToken);


                    if (refreshTokenResponse.IsSuccessStatusCode)
                    {
                        var tokenResult = await refreshTokenResponse.Content.ReadFromJsonAsync<ResultDto<AccessTokenDto>>() ?? new();

                        httpContext.Response.Cookies.Append("AccessToken", tokenResult.Data!.Token, new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = tokenResult.Data!.ExpiredDate
                        });

                        httpContext.Response.Cookies.Append("RefreshToken", tokenResult.Data!.RefreshToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = tokenResult.Data!.RefreshTokenExpiredDate
                        });


                        token = tokenResult.Data!.Token;

                        goto restartRequest;

                    }
                }
            }

        }


        return response;
    }
}
