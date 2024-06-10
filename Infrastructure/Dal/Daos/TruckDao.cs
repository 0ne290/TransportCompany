using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Daos;

public class TruckDao(TransportCompanyContext dbContext) : BaseDao(dbContext), ITruckDao
{
    public async Task<List<Truck>> GetRightTrucksForOrder(Order order)
    {
        var trucksRightForAvailabilityWeightAndVolume = dbContext.Trucks.AsNoTracking().Where(t =>
            t.IsAvailable && t.VolumeMax >= order.CargoVolume && t.WeightMax >= order.CargoWeight);

        List<Truck> rightTrucks;
        if (order.ClassAdr == ClassesAdr.None)
            rightTrucks = await trucksRightForAvailabilityWeightAndVolume.ToListAsync();
        else
        {
            var trucksRightForAvailabilityWeightVolumeAndDriver =
                trucksRightForAvailabilityWeightAndVolume.Where(t => t.Driver != null && t.Driver.CertificatAdr);
            
            switch (order.ClassAdr)
            {
                case ClassesAdr.ExplosiveSubstances:
                    rightTrucks = await trucksRightForAvailabilityWeightVolumeAndDriver.Where(t =>
                        t.TypeAdr == "EXII" || t.TypeAdr == "EXIII").ToListAsync();
                    break;
                case ClassesAdr.Gases:
                case ClassesAdr.FlammableLiquids:
                    rightTrucks = await trucksRightForAvailabilityWeightVolumeAndDriver.Where(t =>
                        t.TypeAdr == "FL").ToListAsync();
                    break;
                case ClassesAdr.ToxicGases:
                case ClassesAdr.SubstancesWhichInContactWithWaterEmitFlammableGases:
                case ClassesAdr.CorrosiveSubstances:
                    rightTrucks = await trucksRightForAvailabilityWeightVolumeAndDriver.Where(t =>
                        t.TypeAdr == "FL" || t.TypeAdr == "AT").ToListAsync();
                    break;
                case ClassesAdr.FlammableSolidsSelfReactiveSubstancesAndSolidDesensitizedExplosives:
                case ClassesAdr.SubstancesLiableToSpontaneousCombustion: 
                case ClassesAdr.OxidizingSubstances:
                case ClassesAdr.ToxicSubstances:
                case ClassesAdr.MiscellaneousDangerousSubstancesAndArticles:
                    rightTrucks = await trucksRightForAvailabilityWeightVolumeAndDriver.Where(t =>
                        t.TypeAdr == "AT").ToListAsync();
                    break;
                default:
                    throw new ArgumentException($"{order.ClassAdr} is not an class adr", nameof(order));
            }
        }

        return rightTrucks;
    }
    
    public async Task Update(Truck truck)
    {
        dbContext.Trucks.Update(truck);
        
        await dbContext.SaveChangesAsync();
    }
}