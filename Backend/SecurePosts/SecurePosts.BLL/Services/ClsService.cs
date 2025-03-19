using SecurePosts.BLL.Repositories;
using SecurePosts.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurePosts.BLL.Services
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
