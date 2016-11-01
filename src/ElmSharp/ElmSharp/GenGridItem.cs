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

namespace ElmSharp
{
    public class GenGridItem : GenItem
    {
        internal GenGridItem(object data, GenItemClass itemClass) : base(data, itemClass)
        {
        }

        public override bool IsSelected
        {
            get
            {
                return Interop.Elementary.elm_gengrid_item_selected_get(Handle);
            }
            set
            {
                Interop.Elementary.elm_gengrid_item_selected_set(Handle, value);
            }
        }

        public override void Update()
        {
            Interop.Elementary.elm_gengrid_item_update(Handle);
        }
    }
}
