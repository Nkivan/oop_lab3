using Lab3.BLL.Mapping;
using Lab3.BLL.Models;
using Lab3.DAL;
using Lab3.DAL.Entities;

namespace Lab3.BLL.Services;

public class TaxiDriverService : EntityService<TaxiDriver, TaxiDriverEntity>
{
    public TaxiDriverService(EntityContext<TaxiDriverEntity> context) : base(context, new TaxiDriverMapper()) { }
}

public class AcrobatService : EntityService<Acrobat, AcrobatEntity>
{
    public AcrobatService(EntityContext<AcrobatEntity> context) : base(context, new AcrobatMapper()) { }
}
