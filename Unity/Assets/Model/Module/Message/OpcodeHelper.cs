using System.Collections.Generic;

namespace ET
{
    public static class OpcodeHelper
    {
        public static readonly HashSet<ushort> DebugLogMessageSet = new HashSet<ushort>
        {
            // OuterOpcode.C2R_Ping,
            // OuterOpcode.R2C_Ping,
        };

        public static bool IsNeedDebugLogMessage(ushort opcode)
        {
            if (DebugLogMessageSet.Contains(opcode))
            {
                return true;
            }

            return false;
        }

        public static bool IsClientHotfixMessage(ushort opcode)
        {
            return opcode > 10000;
        }
    }
}