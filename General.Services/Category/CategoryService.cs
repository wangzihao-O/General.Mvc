using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.Core.Data;
using General.Entities;

namespace General.Services.Category
{
    public class CategoryService : ICategoryService
    {

        private IRepository<Entities.Category> _categoryRepository;

        public CategoryService(IRepository<Entities.Category> categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        public List<Entities.Category> getAll()
        {
            return _categoryRepository.Table.ToList();
        }
    }
}
