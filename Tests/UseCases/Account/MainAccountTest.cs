using System;
using System.Collections.Generic;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using UseCases.Shared;

namespace Tests.UseCases.Account
{
    public class MainAccountTest
    {
        protected readonly Mock<IMediator> MockMediator = new();
        
        protected readonly IMapper TestMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfiles());
        }).CreateMapper();
        
        protected readonly List<User> Users = new()
        {
            new User { Id = Guid.NewGuid().ToString(), FirstName = "FirstName", 
                LastName = "LastName", Email = "test@email.com", UserName = "Test"},
            new User { Id = Guid.NewGuid().ToString(), FirstName = "FirstName2", 
                LastName = "LastName2", Email = "test2@email.com", UserName = "Test2"}
        };
                
        protected static Mock<UserManager<TUser>> ConfiguredMockUserManager<TUser>(ICollection<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }
        
        protected static Mock<UserManager<User>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}