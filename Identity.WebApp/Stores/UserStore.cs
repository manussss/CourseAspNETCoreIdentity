using System.Data.Common;
using Identity.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Identity.WebApp.Stores;

public class UserStore(IConfiguration configuration) : IUserStore<User>, IUserPasswordStore<User>
{
    public async Task<DbConnection> GetOpenConnectionAsync()
    {
        var connection = new SqlConnection(configuration.GetConnectionString("IdentityWebApp"));
        await connection.OpenAsync();

        return connection;
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        using var connection = await GetOpenConnectionAsync();

        await connection.ExecuteAsync(
            @"
                insert into Users([Id], [UserName], [NormalizedUserName],
                [PassWordHash]) values (@Id, @UserName, @NormalizedUserName, @PasswordHash)
            ",
            new
            {
                Id = user.Id,
                UserName = user.UserName,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash
            });

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        using var connection = await GetOpenConnectionAsync();

        await connection.ExecuteAsync(
            "delete from users where Id = @Id",
            new
            {
                Id = user.Id,
            });

        return IdentityResult.Success;
    }

    public void Dispose()
    {
    }

    public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        using var connection = await GetOpenConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "select * from users where Id = @Id",
            new { Id = userId });
    }

    public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        using var connection = await GetOpenConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "select * from users where NormalizedUserName = @NormalizedUserName",
            new { NormalizedUserName = normalizedUserName });
    }

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id);
    }

    public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        using var connection = await GetOpenConnectionAsync();

        await connection.ExecuteAsync(
            @"
                update Users set [Id] = @Id, [UserName] = @UserName,
                [NormalizedUserName] = @NormalizedUserName,
                [PasswordHash] = @PasswordHash where [Id] = @Id
            ",
            new
            {
                Id = user.Id,
                UserName = user.UserName,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash
            });

        return IdentityResult.Success;
    }

    public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;

        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash != null);
    }
}