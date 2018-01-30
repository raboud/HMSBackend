using Microsoft.AspNetCore.Mvc.Rendering;
using HMSWeb.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSWeb.Services
{
    public interface ICatalogService
    {
        Task<Catalog> GetCatalogItems(int pageIndex, int itemsPage, int? brandID, int? typeId);
        Task<IEnumerable<SelectListItem>> GetBrands();
        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}
