﻿using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace UsefulSkips
{
    public class UsefulSkipsGameManager
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList(), codesToInsert = new List<CodeInstruction>();
            MethodInfo method = null;
            int index = 0, startIndex = 0, endIndex = 0;

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].operand is "SandBox")
                {
                    startIndex = i - 4;
                }
                else if (codes[i].opcode == OpCodes.Ldloc_0 || codes[i].opcode == OpCodes.Ldloc_2)
                {
                    endIndex = i + 2;
                    index = i + 3;
                }
                else if (codes[i].opcode == OpCodes.Ldftn)
                {
                    method = (MethodInfo)codes[i].operand;
                }
            }

            codesToInsert.Add(new CodeInstruction(OpCodes.Ldarg_0));
            codesToInsert.Add(new CodeInstruction(OpCodes.Call, method));
            codes.InsertRange(index, codesToInsert);
            // Skip the campaign intro.
            codes.RemoveRange(startIndex, endIndex - startIndex + 1);

            return codes;
        }
    }
}
