﻿using RegistrationApp.Domain.Interfaces.Repositories.Identity;
using RegistrationApp.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RegistrationApp.Domain.Core.Entities.Identity;

namespace RegistrationApp.Persistence.Repositories.Identity
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public override async Task<User> GetByIdAsync(Guid id)
        {
            return await FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate)
        {
            return await DbSet.Include(x => x.Role).FirstOrDefaultAsync(predicate);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await DbSet.Include(x => x.Role).ToListAsync();
        }
    }
}
