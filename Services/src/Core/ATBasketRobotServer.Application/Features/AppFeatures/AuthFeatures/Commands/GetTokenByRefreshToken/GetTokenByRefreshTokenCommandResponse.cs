﻿using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Commands.GetTokenByRefreshToken;
public sealed record GetTokenByRefreshTokenCommandResponse(TokenRefreshTokenDto Token, string Email, string UserId, string NameLastName, IList<CompanyDto> Companies, int Year, CompanyDto Company);