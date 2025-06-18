//
// PrivMX Endpoint C#
// Copyright © 2024 Simplito sp. z o.o.
//
// This file is part of the PrivMX Platform (https://privmx.dev).
// This software is Licensed under the MIT License.
//
// See the License for the specific language governing permissions and
// limitations under the License.
//

using PrivMX.Endpoint.Core.Internal;
using PrivMX.Endpoint.Core.Models;
using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Core
{
    public class EventQueue : IEventQueue
    {
        static private EventQueue? instance = null;
        private readonly IntPtr ptr;
        private readonly Executor executor = new Executor(new EventQueueNative());
        
        /// <summary>
        /// Gets the event queue instance.
        /// </summary>
        /// <returns>Event queue instance.</returns>
        static public EventQueue GetInstance()
        {
            instance ??= new EventQueue();
            return instance;
        }

        private EventQueue()
        {
            EventQueueNative.privmx_endpoint_newEventQueue(out ptr);
        }

        ~EventQueue()
        {
            EventQueueNative.privmx_endpoint_freeEventQueue(ptr);
        }

        /// <summary>
        /// Puts the <see cref="LibBreakEvent"/> event into the event queue.
        /// 
        /// This method is useful for interrupting a blocking <see cref="WaitEvent()"/> call and breaking an event processing loop.
        /// </summary>
        public void EmitBreakEvent()
        {
            executor.ExecuteVoid(ptr, (int)EventQueueNative.Method.EmitBreakEvent, new List<object?>{});
        }

        /// <summary>
        /// Gets a new event from the queue.
        /// </summary>
        /// <returns>A new event, or <see langword="null"/> if no events in the queue.</returns>
        public Models.Event? GetEvent()
        {
            return executor.ExecuteOpt<Event>(ptr, (int)EventQueueNative.Method.GetEvent, new List<object?>{});
        }

        /// <summary>
        /// Gets or waits for a new event from the queue.
        /// 
        /// Waiting can be canceled by <see cref="EmitBreakEvent()"/>.
        /// </summary>
        /// <returns>A new event.</returns>
        public Models.Event WaitEvent()
        {
            return executor.Execute<Models.Event>(ptr, (int)EventQueueNative.Method.WaitEvent, new List<object?>{});
        }
    }
}
