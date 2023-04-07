using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace UsefulSkips
{
    public class UsefulSkipsGameManager
    {
        // Skip the campaign intro.
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            List<CodeInstruction> codesToInsert = new List<CodeInstruction>();
            MethodInfo method = null;
            int index = 0;
            int startIndex = 0;
            int endIndex = 0;
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Stloc_0)
                {
                    startIndex = i - 3;
                }
                else if (codes[i].opcode == OpCodes.Ldloc_0)
                {
                    endIndex = i + 2;
                    index = i + 3;
                    method = (MethodInfo)codes[i - 5].operand;
                }
            }
            codesToInsert.Add(new CodeInstruction(OpCodes.Ldarg_0));
            codesToInsert.Add(new CodeInstruction(OpCodes.Call, method));
            codes.InsertRange(index, codesToInsert);
            codes.RemoveRange(startIndex, endIndex - startIndex + 1);
            return codes;
        }
    }
}
