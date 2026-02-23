using DataAccessLayer.Abstract;
using EntityLayer;
using EntityLayer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class MongoGuideDal : GenericRepository<Guide>, IGuideDal
    {
        public MongoGuideDal(IDatabaseSettings _databaseSettings) : base(_databaseSettings, _databaseSettings.GuideCollectionName)
        {
        }
    }
}
