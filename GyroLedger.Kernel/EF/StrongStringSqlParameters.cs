using GyroLedger.CodeInterface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GyroLedger.Kernel.EF;

public static class StrongStringSqlParameters
{
    public static SqlParameter AddStrongString(this SqlParameterCollection self,
        string paramName, StrongString strong, int? maxLength = null)
    {
        var _param = new SqlParameter(
            paramName,
            SqlDbType.VarChar,
            maxLength ?? strong.KeyVal.Length)
        {
            Value = strong.KeyVal,
            Direction = ParameterDirection.Input
        };
        self.Add(_param);
        return _param;
    }

    public static SqlParameter AddUnicodeStrongString(this SqlParameterCollection self,
        string paramName, StrongString strong, int? maxLength = null)
    {
        var _param = new SqlParameter(
            paramName,
            SqlDbType.NVarChar,
            maxLength ?? strong.KeyVal.Length)
        {
            Value = strong.KeyVal,
            Direction = ParameterDirection.Input
        };
        self.Add(_param);
        return _param;
    }
}
