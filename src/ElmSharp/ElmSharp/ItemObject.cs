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

namespace ElmSharp
{
    public class ItemObject
    {
        private static Dictionary<int, ItemObject> s_IdToItemTable = new Dictionary<int, ItemObject>();
        private static Dictionary<IntPtr, ItemObject> s_HandleToItemTable = new Dictionary<IntPtr, ItemObject>();
        private static int s_globalId = 0;

        readonly Dictionary<string, EvasObject> _partContents = new Dictionary<string, EvasObject>();
        Interop.Evas.SmartCallback _deleteCallback;
        IntPtr _handle = IntPtr.Zero;

        protected ItemObject(IntPtr handle)
        {
            _deleteCallback = DeleteCallbackHandler;
            Id = GetNextId();
            s_IdToItemTable[Id] = this;
            Handle = handle;
        }

        // C# Finalizer was called on GC thread
        // So, We can't access to EFL object
        // And When Finalizer was called, Field can be already released.
        //~ItemObject()
        //{
        //    if (Handle != IntPtr.Zero)
        //        Interop.Elementary.elm_object_item_del(Handle);
        //}

        public int Id { get; private set; }

        public bool IsEnabled
        {
            get { return !Interop.Elementary.elm_object_item_disabled_get(Handle); }
            set { Interop.Elementary.elm_object_item_disabled_set(Handle, !value); }
        }

        internal IntPtr Handle
        {
            get
            {
                return _handle;
            }
            set
            {
                if (_handle == value)
                    return;

                if (_handle != IntPtr.Zero)
                {
                    UnsetDeleteCallback();
                }
                _handle = value;
                SetDeleteCallback();
                s_HandleToItemTable[Handle] = this;
            }
        }

        public event EventHandler Deleted;

        public void Delete()
        {
            Interop.Elementary.elm_object_item_del(Handle);
            _handle = IntPtr.Zero;
        }

        public void SetPartContent(string part, EvasObject content)
        {
            SetPartContent(part, content, false);
        }

        public void SetPartContent(string part, EvasObject content, bool preserveOldContent)
        {
            IntPtr oldContent = Interop.Elementary.elm_object_item_part_content_unset(Handle, part);
            if (oldContent != IntPtr.Zero && !preserveOldContent)
            {
                Interop.Evas.evas_object_del(oldContent);
            }
            Interop.Elementary.elm_object_item_part_content_set(Handle, part, content);
            _partContents[part ?? "__default__"] = content;
        }

        public void SetPartText(string part, string text)
        {
            Interop.Elementary.elm_object_item_part_text_set(Handle, part, text);
        }

        public string GetPartText(string part)
        {
            return Interop.Elementary.elm_object_item_part_text_get(Handle, part);
        }

        public static implicit operator IntPtr(ItemObject obj)
        {
            if (obj == null)
                return IntPtr.Zero;
            return obj.Handle;
        }

        protected virtual void OnInvalidate() { }

        internal static ItemObject GetItemById(int id)
        {
            ItemObject value;
            s_IdToItemTable.TryGetValue(id, out value);
            return value;
        }

        internal static ItemObject GetItemByHandle(IntPtr handle)
        {
            ItemObject value;
            s_HandleToItemTable.TryGetValue(handle, out value);
            return value;
        }

        void DeleteCallbackHandler(IntPtr data, IntPtr obj, IntPtr info)
        {
            Deleted?.Invoke(this, EventArgs.Empty);
            OnInvalidate();
            if (s_IdToItemTable.ContainsKey(Id))
            {
                s_IdToItemTable.Remove(Id);
            }
            if (s_HandleToItemTable.ContainsKey(_handle))
            {
                s_HandleToItemTable.Remove(_handle);
            }
            _partContents.Clear();
            _handle = IntPtr.Zero;
        }

        void UnsetDeleteCallback()
        {
            Interop.Elementary.elm_object_item_del_cb_set(Handle, null);
        }

        void SetDeleteCallback()
        {
            if (Handle != IntPtr.Zero)
                Interop.Elementary.elm_object_item_del_cb_set(Handle, _deleteCallback);
        }

        static int GetNextId()
        {
            return s_globalId++;
        }
    }
}
