using EntityLayer.Settings;
using DataAccessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class MongoCommentDal : GenericRepository<Comment>, ICommentDal
    {
        public MongoCommentDal(IDatabaseSettings _databaseSettings) : base(_databaseSettings, _databaseSettings.CommentCollectionName)
        {
        }
    }
}
