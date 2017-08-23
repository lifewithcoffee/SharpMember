using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Definitions
{
    public enum eDatabaseType
    {
        Sqlite,
        SqlServer,
        Postgres
    }

    static public class GlobalConfigs
    {
        static public eDatabaseType DatabaseType { get; set; } = eDatabaseType.SqlServer;
    }
}
