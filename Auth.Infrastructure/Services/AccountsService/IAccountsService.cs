﻿using Auth.Domain.Shared.Models;

namespace Auth.Infrastructure.Services.AccountsService;

public interface IAccountsIS
{
    Task<Password?> UpdatePasswordAsync(int accountId, string password);
    Task<Account?> GetByIdAsync(int id);
    Task<Account?> GetByEmailAsync(string email);
}
