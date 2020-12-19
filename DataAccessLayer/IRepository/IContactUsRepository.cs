using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IContactUsRepository
    {
        Task<int> PostAsync(ContactUs contact);
    }
}
