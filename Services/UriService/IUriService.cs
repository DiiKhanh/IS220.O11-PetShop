using PetShop.DTOs.Wrapper;

namespace PetShop.Services.UriService
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
