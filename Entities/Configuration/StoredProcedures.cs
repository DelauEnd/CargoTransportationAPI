using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class StoredProcedures
    {
        public static string AssignCargoToRoute =>
            "create procedure AssignCargoToRoute " +
            "@cargoId int, " +
            "@routeId nvarchar(10) " +
            "as " +
            "begin " +
            "update Cargoes " +
            "set RouteId = @routeId " +
            "where CargoId = @cargoId " +
            "end";
    }
}
