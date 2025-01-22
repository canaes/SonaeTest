using SonaeTestSol.Domain.Entities;
using SonaeTestSol.Domain.Interfaces.Service;
using SonaeTestSol.Domain.Models;
using SonaeTestSol.Services.Base;
using SonaeTestSol.Services.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonaeTestSol.Services
{
    public class StockService : BaseService, IStockService
    {
        private int productsQuantity = 100;

        public StockService(IErrorService errorService) : base(errorService)
        {
        }

        public async Task<int> Get()
        {
            return productsQuantity;
        }

        public async Task Set(int value)
        {
            productsQuantity = value;
        }

    }
}
