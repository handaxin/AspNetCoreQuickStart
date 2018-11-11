﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyCompany.MyProject.Application.Demo.Models;
using MyCompany.MyProject.Persistence;

namespace MyCompany.MyProject.Application.Demo.Queries
{
    public class GetDemoByIdQueryHandler : IRequestHandler<GetDemoByIdQuery, DemoModel>
    {
        private readonly AppDbContext _appDbContext;

        public GetDemoByIdQueryHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<DemoModel> Handle(GetDemoByIdQuery request, CancellationToken cancellationToken)
        {
            var resylt = _appDbContext.Demo
                .Select(s => new DemoModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).FirstOrDefault();

            return Task.FromResult(resylt);
        }
    }
}
