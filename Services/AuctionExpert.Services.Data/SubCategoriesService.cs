﻿namespace AuctionExpert.Services.Data
{
    using System.Linq;

    using AuctionExpert.Data.Common.Repositories;
    using AuctionExpert.Data.Models;
    using AuctionExpert.Services.Mapping;

    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly IDeletableEntityRepository<SubCategory> subCategoryRepository;

        public SubCategoriesService(IDeletableEntityRepository<SubCategory> subCategoryRepository)
        {
            this.subCategoryRepository = subCategoryRepository;
        }

        public IQueryable<T> GetAllSubCategories<T>()
        {
            return this.subCategoryRepository.AllAsNoTracking().To<T>();
        }

        public IQueryable<T> GetAllByCategoryId<T>(int categoryId)
        {
            return this.subCategoryRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .To<T>();
        }
    }
}