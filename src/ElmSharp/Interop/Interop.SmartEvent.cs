/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;

internal static partial class Interop
{
    internal class SmartEvent<TEventArgs> : IDisposable where TEventArgs : EventArgs
    {
        public delegate TEventArgs SmartEventInfoParser(IntPtr data, IntPtr obj, IntPtr info);

        private readonly object _sender;
        private readonly string _eventName;
        private readonly IntPtr _handle;
        private readonly SmartEventInfoParser _parser;
        private readonly List<NativeCallback> _nativeCallbacks = new List<NativeCallback>();

        public SmartEvent(object sender, IntPtr handle, string eventName, SmartEventInfoParser parser)
        {
            _sender = sender;
            _eventName = eventName;
            _handle = handle;
            _parser = parser;
        }
        public SmartEvent(object sender, IntPtr handle, string eventName) : this(sender, handle, eventName, null)
        {
        }

        ~SmartEvent()
        {
            Dispose(false);
        }

        private struct NativeCallback
        {
            public Interop.Evas.SmartCallback callback;
            public EventHandler<TEventArgs> eventHandler;
        }

        public event EventHandler<TEventArgs> On
        {
            add
            {
                EventHandler<TEventArgs> handler = value;
                var cb = new Interop.Evas.SmartCallback((d, o, e) =>
                {
                    TEventArgs ea = _parser == null ? (TEventArgs)EventArgs.Empty : _parser(d, o, e);
                    handler(_sender, ea);
                });
                _nativeCallbacks.Add(new NativeCallback { callback = cb, eventHandler = handler });
                int i = _nativeCallbacks.Count - 1;
                Interop.Evas.evas_object_smart_callback_add(_handle, _eventName, _nativeCallbacks[i].callback, IntPtr.Zero);
            }

            remove
            {
                EventHandler<TEventArgs> handler = value;
                var callbacks = _nativeCallbacks.Where(cb => cb.eventHandler == handler);
                foreach (var cb in callbacks)
                {
                    Interop.Evas.evas_object_smart_callback_del(_handle, _eventName, cb.callback);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Place holder to dispose managed state (managed objects).
            }
            foreach (var cb in _nativeCallbacks)
            {
                Evas.evas_object_smart_callback_del(_handle, _eventName, cb.callback);
            }
            _nativeCallbacks.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    internal class SmartEvent : IDisposable
    {
        private readonly object _sender;
        private readonly string _eventName;
        private readonly IntPtr _handle;
        private readonly List<NativeCallback> _nativeCallbacks = new List<NativeCallback>();

        public SmartEvent(object sender, IntPtr handle, string eventName)
        {
            _sender = sender;
            _eventName = eventName;
            _handle = handle;
        }

        ~SmartEvent()
        {
            Dispose(false);
        }

        private struct NativeCallback
        {
            public Interop.Evas.SmartCallback callback;
            public EventHandler eventHandler;
        }

        public event EventHandler On
        {
            add
            {
                EventHandler handler = value;
                var cb = new Interop.Evas.SmartCallback((d, o, e) =>
                {
                    handler(_sender, EventArgs.Empty);
                });
                _nativeCallbacks.Add(new NativeCallback { callback = cb, eventHandler = handler });
                int i = _nativeCallbacks.Count - 1;
                Interop.Evas.evas_object_smart_callback_add(_handle, _eventName, _nativeCallbacks[i].callback, IntPtr.Zero);
            }

            remove
            {
                EventHandler handler = value;
                var callbacks = _nativeCallbacks.Where(cb => cb.eventHandler == handler);
                foreach (var cb in callbacks)
                {
                    Interop.Evas.evas_object_smart_callback_del(_handle, _eventName, cb.callback);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Place holder to dispose managed state (managed objects).
            }
            foreach (var cb in _nativeCallbacks)
            {
                Evas.evas_object_smart_callback_del(_handle, _eventName, cb.callback);
            }
            _nativeCallbacks.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
