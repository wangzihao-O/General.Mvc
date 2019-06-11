using System;
using System.Collections.Generic;
using System.Text;

namespace General.Services.Category
{
    public interface ICategoryService
    {
        List<Entities.Category> getAll();
    }
}
