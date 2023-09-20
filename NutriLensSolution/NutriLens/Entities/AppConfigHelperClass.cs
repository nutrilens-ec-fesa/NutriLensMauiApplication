using NutriLens.Models;
using NutriLens.Services;
using ExceptionLibrary;

namespace NutriLens.Entities
{
    public static class AppConfigHelperClass
    {
        public enum ConfigItems
        {
            EnergeticUnit
        }

        private static EnergeticUnit? _energeticUnit;

        public static EnergeticUnit EnergeticUnit
        {
            get
            {
                if (_energeticUnit == null)
                {
                    try
                    {
                        _energeticUnit = (EnergeticUnit?)(ConfigItems)ViewServices.AppDataManager.GetItem<int>(ConfigItems.EnergeticUnit);
                    }
                    catch (NotFoundException)
                    {
                        _energeticUnit = EnergeticUnit.kcal;
                    }
                }

                return (EnergeticUnit)_energeticUnit;
            }
        }
    }
}
