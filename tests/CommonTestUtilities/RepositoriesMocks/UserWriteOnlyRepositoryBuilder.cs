﻿using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserWriteOnlyRepositoryBuilder
{
	public static IUserWriteOnlyRepository BuildRepository()
	{
		var mock = new Mock<IUserWriteOnlyRepository>();

		return mock.Object;		
	}
}