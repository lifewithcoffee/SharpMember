using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Global
{
    public enum eDatabaseType
    {
        Sqlite,
        SqlServer,
        Postgres
    }

    static public class GlobalConfigs
    {
        //static public eDatabaseType DatabaseType { get; set; } = eDatabaseType.Sqlite;
        static public eDatabaseType DatabaseType { get; set; } = eDatabaseType.SqlServer;
    }
}
