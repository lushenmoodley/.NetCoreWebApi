﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : IVillaRepository
    {

        private readonly ApplicationDBContext _db;

        public VillaRepository(ApplicationDBContext db) {  _db = db; }

        public async Task UpdateAsync(Villa entity)
        {
            entity.UpdateDate = DateTime.Now;
            _db.Villas.Update(entity);
        }
    }
}
