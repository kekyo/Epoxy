////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

#nullable enable

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Epoxy;

internal static class Hoe
{
    public static void AddRange(
        this Collection<Instruction> instructions, params Instruction[] insts)
    {
        foreach (Instruction inst in insts)
        {
            instructions.Add(inst);
        }
    }
}

[TestFixture]
[Parallelizable(ParallelScope.All)]
public sealed class InstructionAnalyzerTest
{
    [Test]
    public static void T()
    {
        var module = ModuleDefinition.CreateModule("test", ModuleKind.Dll);
        var method = new MethodDefinition(
            "test",
            MethodAttributes.Public | MethodAttributes.Static,
            new TypeReference("System", "Int32", module, module));

        method.Body.Instructions.AddRange(
            Instruction.Create(OpCodes.Ldc_I4_1),
            Instruction.Create(OpCodes.Ldc_I4_2),
            Instruction.Create(OpCodes.Add),
            Instruction.Create(OpCodes.Ret));

        var points = InstructionAnalyzer.AnalyzeToConsumePoints(method.Body).
            ToArray();


        var c = System.Reflection.Emit.OpCodes.Call;
    }
}
