﻿namespace Survey.Shared.Common.Interfaces;

public interface ICurrentUserService
{
    string UserId { get; }
    string Token { get; }
}
