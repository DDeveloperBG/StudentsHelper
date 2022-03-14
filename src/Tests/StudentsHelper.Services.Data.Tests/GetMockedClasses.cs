namespace StudentsHelper.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using StudentsHelper.Data.Common.Models;
    using StudentsHelper.Data.Common.Repositories;

    public static class GetMockedClasses
    {
        public static Mock<UserManager<TUser>> MockUserManager<TUser>()
            where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            return mgr;
        }

        public static Mock<IRepository<TEntity>> MockIRepository<TEntity>(
            ICollection<TEntity> entities)
            where TEntity : class
        {
            var repository = new Mock<IRepository<TEntity>>();
            repository
                .Setup(r => r.AddAsync(It.IsAny<TEntity>()))
                .Callback((TEntity r) => entities.Add(r));
            repository
                .Setup(r => r.Delete(It.IsAny<TEntity>()))
                .Callback((TEntity r) => entities.Remove(r));
            repository
                .Setup(r => r.All())
                .Returns(entities.AsQueryable());
            repository
                .Setup(r => r.AllAsNoTracking())
                .Returns(entities.AsQueryable());

            return repository;
        }

        public static Mock<IDeletableEntityRepository<TEntity>> MockIDeletableEntityRepository<TEntity>(
            ICollection<TEntity> entities)
            where TEntity : class, IDeletableEntity
        {
            var repository = new Mock<IDeletableEntityRepository<TEntity>>();
            repository
                .Setup(r => r.AddAsync(It.IsAny<TEntity>()))
                .Callback((TEntity r) => entities.Add(r));
            repository
                .Setup(r => r.Delete(It.IsAny<TEntity>()))
                .Callback((TEntity r) => entities.Remove(r));
            repository
                .Setup(r => r.All())
                .Returns(entities.AsQueryable());
            repository
                .Setup(r => r.AllAsNoTracking())
                .Returns(entities.AsQueryable());

            return repository;
        }
    }
}
