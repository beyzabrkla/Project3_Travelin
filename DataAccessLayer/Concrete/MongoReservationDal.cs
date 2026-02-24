using DataAccessLayer.Abstract;
using EntityLayer;
using EntityLayer.Settings;

namespace DataAccessLayer.Concrete
{
    public class MongoReservationDal : GenericRepository<Reservation>, IReservationDal
    {
        public MongoReservationDal(IDatabaseSettings _databaseSettings)
            : base(_databaseSettings, _databaseSettings.ReservationCollectionName)
        {
        }
    }
}