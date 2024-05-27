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

using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Epoxy;

public enum ProduceCounts
{
    Zero,
    One,
    Two,
}

public abstract class Point
{
    public readonly Instruction Instruction;

    public ProduceCounts ProduceCount =>
        InstructionAnalyzer.GetProduceCount(this.Instruction);

    protected Point(Instruction instruction) =>
        this.Instruction = instruction;
}

public sealed class NonConsumePoint : Point
{
    public NonConsumePoint(Instruction instruction) :
        base(instruction)
    {
    }

    public void Deconstruct(
        out Instruction instruction,
        out ProduceCounts produceCount)
    {
        instruction = this.Instruction;
        produceCount = this.ProduceCount;
    }
}

public sealed class ConsumePoint : Point
{
    public readonly Point[] Inputs;

    public ConsumePoint(Instruction instruction, Point[] inputs) :
        base(instruction) =>
        this.Inputs = inputs;

    public void Deconstruct(
        out Instruction instruction,
        out Point[] inputs,
        out ProduceCounts produceCount)
    {
        instruction = this.Instruction;
        inputs = this.Inputs;
        produceCount = this.ProduceCount;
    }
}

public static class InstructionAnalyzer
{
    internal static bool IsCall(OpCode opCode) =>
        opCode == OpCodes.Call || opCode == OpCodes.Callvirt;
    internal static bool IsCalli(OpCode opCode) =>
        opCode == OpCodes.Calli;
    internal static bool IsRet(OpCode opCode) =>
        opCode == OpCodes.Ret;
    internal static bool IsVoid(TypeReference type) =>
        type.FullName == "System.Void";

    internal static ProduceCounts GetProduceCount(Instruction instruction) =>
        instruction.OpCode.StackBehaviourPush switch
        {
            StackBehaviour.Push1 => ProduceCounts.One,
            StackBehaviour.Pushi => ProduceCounts.One,
            StackBehaviour.Pushi8 => ProduceCounts.One,
            StackBehaviour.Pushr4 => ProduceCounts.One,
            StackBehaviour.Pushr8 => ProduceCounts.One,
            StackBehaviour.Pushref => ProduceCounts.One,
            StackBehaviour.Push1_push1 => ProduceCounts.Two,
            StackBehaviour.Varpush when IsCall(instruction.OpCode) && instruction.Operand is MethodReference m =>
                IsVoid(m.ReturnType) ? ProduceCounts.Zero : ProduceCounts.One,
            StackBehaviour.Varpush when IsCalli(instruction.OpCode) && instruction.Operand is CallSite c =>
                IsVoid(c.ReturnType) ? ProduceCounts.Zero : ProduceCounts.One,
            _ => ProduceCounts.Zero,
        };

    public static IEnumerable<ConsumePoint> AnalyzeToConsumePoints(
        this MethodBody body)
    {
        var es = new Stack<Point>();

        foreach (var instruction in body.Instructions)
        {
            var opCode = instruction.OpCode;

            Point point;
            switch (opCode.StackBehaviourPop)
            {
                case StackBehaviour.Pop0:
                    point = new NonConsumePoint(instruction);
                    break;

                case StackBehaviour.Pop1:
                case StackBehaviour.Popi:
                case StackBehaviour.Popref:
                    var popPoint1 = es.Pop();
                    var cp1 = new ConsumePoint(instruction, new[] { popPoint1 });
                    yield return cp1;
                    point = cp1;
                    break;

                case StackBehaviour.Pop1_pop1:
                case StackBehaviour.Popi_pop1:
                case StackBehaviour.Popi_popi:
                case StackBehaviour.Popi_popi8:
                case StackBehaviour.Popi_popr4:
                case StackBehaviour.Popi_popr8:
                case StackBehaviour.Popref_pop1:
                case StackBehaviour.Popref_popi:
                    var popPoint2 = es.Pop();
                    var popPoint3 = es.Pop();
                    var cp2 = new ConsumePoint(instruction, new[] { popPoint3, popPoint2 });
                    yield return cp2;
                    point = cp2;
                    break;

                case StackBehaviour.Popi_popi_popi:
                case StackBehaviour.Popref_popi_popi:
                case StackBehaviour.Popref_popi_popi8:
                case StackBehaviour.Popref_popi_popr4:
                case StackBehaviour.Popref_popi_popr8:
                case StackBehaviour.Popref_popi_popref:
                    var popPoint4 = es.Pop();
                    var popPoint5 = es.Pop();
                    var popPoint6 = es.Pop();
                    var cp3 = new ConsumePoint(instruction, new[] { popPoint6, popPoint5, popPoint4 });
                    yield return cp3;
                    point = cp3;
                    break;

                case StackBehaviour.Varpop:
                    if (IsRet(opCode))
                    {
                        if (IsVoid(body.Method.ReturnType))
                        {
                            point = new NonConsumePoint(instruction);
                        }
                        else
                        {
                            var popPoint7 = es.Pop();
                            var cp4 = new ConsumePoint(instruction, new[] { popPoint7 });
                            yield return cp4;
                            point = cp4;
                        }
                    }
                    else if (IsCall(opCode) && instruction.Operand is MethodReference m)
                    {
                        var popPoints = new Point[(m.HasThis ? 1 : 0) + m.Parameters.Count];
                        for (var index = 0; index < popPoints.Length; index++)
                        {
                            popPoints[index] = es.Pop();
                        }
                        var cp5 = new ConsumePoint(instruction, popPoints);
                        yield return cp5;
                        point = cp5;
                    }
                    else if (IsCalli(opCode) && instruction.Operand is CallSite c)
                    {
                        var popPoints = new Point[(c.HasThis ? 1 : 0) + c.Parameters.Count];
                        for (var index = 0; index < popPoints.Length; index++)
                        {
                            popPoints[index] = es.Pop();
                        }
                        var cp5 = new ConsumePoint(instruction, popPoints);
                        yield return cp5;
                        point = cp5;
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }

            switch (point.ProduceCount)
            {
                case ProduceCounts.One:
                    es.Push(point);
                    break;
                case ProduceCounts.Two:
                    es.Push(point);
                    es.Push(point);
                    break;
            }
        }
    }
}
