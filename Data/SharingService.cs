using menu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menu.Data
{
    public class SharingService
    {
        private readonly AzureFunctionService azureFunctionService;

        public SharingService(AzureFunctionService azureFunctionService)
        {
            this.azureFunctionService = azureFunctionService;
        }

        public async Task<bool> ShareList(User user, string shareCode, UserList list)
        {

        }
    }
}
