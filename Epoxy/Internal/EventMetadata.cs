////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
// Copyright (c) 2019-2021 Kouji Matsui (@kozy_kekyo, @kekyo2)
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
using System.Reflection;
using System.Windows.Input;

namespace Epoxy.Internal
{
    internal static class EventMetadata
    {
        private struct EventKey
        {
            public readonly Type Type;
            public readonly string Name;

            public EventKey(Type type, string name)
            {
                this.Type = type;
                this.Name = name;
            }
        }

        private static readonly Dictionary<EventKey, EventInfo?> events =
            new Dictionary<EventKey, EventInfo?>();

        public static EventInfo? GetOrAddEventInfo(Type type, string name)
        {
            var key = new EventKey(type, name);
            if (!events.TryGetValue(key, out var ei))
            {
                ei = type.GetEvent(name);
                events.Add(key, ei);
            }
            if (ei == null)
            {
                throw new ArgumentException($"Couldn't bind event: Type={type.FullName}, Name={name}");
            }
            return ei;
        }

        private sealed class InvokingClosure
        {
            private readonly ICommand command;

            public InvokingClosure(ICommand command) =>
                this.command = command;

            public void Handler(object? sender, object? e)
            {
                if (this.command.CanExecute(e))
                {
                    this.command.Execute(e);
                }
            }
        }

        public static Delegate CreateHandler(EventInfo ei, ICommand command)
        {
            // Limitation:
            //   The closure handler signature valid only standard event style:
            //   `void (object? sender, object? e)`
            //   We can make perfect trampoline by opcode emitter or expression constructor.
            //   It's decline running on the AOT platform...
            var closure = new InvokingClosure(command);
            return new EventHandler(closure.Handler).   // valid with contravariance `e`
                GetMethodInfo()!.
                CreateDelegate(ei.EventHandlerType!, closure);
        }
    }
}
