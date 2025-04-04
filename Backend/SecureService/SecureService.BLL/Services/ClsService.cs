using SecureService.BLL.Repositories;
using SecureService.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureService.BLL.Services
{
    public class ClsService : IClsRepository
    {
        private readonly IDalClsRepository _cls;

        public ClsService(IDalClsRepository IDalClsRepository)
        {
            this._cls = IDalClsRepository;
        }
    }
}
