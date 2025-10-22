using BusinessLayer.Validation;
using DataAccessLayer.Common;
using DataAccessLayer.Repositories.Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class RoomTypeService
    {
        private readonly IRoomTypeRepository _repo;
        public RoomTypeService(IRoomTypeRepository repo) { _repo = repo; }


        public RepositoryResult<RoomType> Create(RoomType x)
        {
            var e = Validate(x);
            if (e != null) return RepositoryResult<RoomType>.Fail(e);
            return _repo.Add(x);
        }


        public RepositoryResult<RoomType> Update(RoomType x)
        {
            var e = Validate(x);
            if (e != null) return RepositoryResult<RoomType>.Fail(e);
            return _repo.Update(x);
        }


        private string? Validate(RoomType x)
        {
            if (!Validators.NotEmpty(x.RoomTypeName) || !Validators.MaxLength(x.RoomTypeName, 50))
                return "RoomTypeName required (<=50).";
            if (!Validators.MaxLength(x.TypeDescription, 250))
                return "TypeDescription max 250.";
            if (!Validators.MaxLength(x.TypenNote, 250))
                return "TypenNote max 250.";
            return null;
        }
    }
}
