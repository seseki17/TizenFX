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
    /// <summary>
    /// The Panes is a widget that adds a draggable bar between two contents.
    /// When dragged this bar resizes contents' size.
    /// </summary>
    public class Panes : Layout
    {
        SmartEvent _press;
        SmartEvent _unpressed;

        /// <summary>
        /// Creates and initializes a new instance of the Panes class.
        /// </summary>
        /// <param name="parent">The EvasObject to which the new Panes will be attached as a child.</param>
        public Panes(EvasObject parent) : base(parent)
        {
            _press = new SmartEvent(this, this.RealHandle, "press");
            _unpressed = new SmartEvent(this, this.RealHandle, "unpress");

            _press.On += (s, e) => Pressed?.Invoke(this, e);
            _unpressed.On += (s, e) => Unpressed?.Invoke(this, e);
        }

        /// <summary>
        /// Pressed will be triggered when panes have been pressed (button isn't released yet).
        /// </summary>
        public event EventHandler Pressed;

        /// <summary>
        /// Unpressed will be triggered when panes are released after being pressed.
        /// </summary>
        public event EventHandler Unpressed;

        /// <summary>
        /// Sets or gets resize mode of a given Panes widget.
        /// True means the left and right panes resize homogeneously.
        /// </summary>
        public bool IsFixed
        {
            get
            {
                return Interop.Elementary.elm_panes_fixed_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_panes_fixed_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or Gets the size proportion of the Panes widget's left side.
        /// </summary>
        /// <remarks>
        /// By default it's homogeneous, i.e., both sides have the same size.If something different is required,
        /// it can be set with this function. For example, if the left content should be displayed over 75% of the panes size,
        /// size should be passed as 0.75. This way, the right content is resized to 25% of the panes size.
        /// If displayed vertically, left content is displayed at the top, and right content at the bottom.
        /// This proportion changes when the user drags the panes bar.
        ///
        /// The value is float type and between 0.0 and 1.0 representing the size proportion of the left side.
        /// </remarks>
        public double Proportion
        {
            get
            {
                return Interop.Elementary.elm_panes_content_left_size_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_panes_content_left_size_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the orientation of a given Panes widget.
        /// </summary>
        /// <remarks>
        /// Uses this function to change how your panes are to be disposed: vertically or horizontally.
        /// By default it's displayed horizontally.
        /// </remarks>
        public bool IsHorizontal
        {
            get
            {
                return Interop.Elementary.elm_panes_horizontal_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_panes_horizontal_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the absolute minimum size of panes widget's left side.
        /// If displayed vertically, left content is displayed at top.
        /// value representing minimum size of left side in pixels.
        /// </summary>
        public int LeftMinimumSize
        {
            get
            {
                return Interop.Elementary.elm_panes_content_left_min_size_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_panes_content_left_min_size_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the relative minimum size of panes widget's left side.
        /// proportion of minimum size of left side.
        /// If displayed vertically, left content is displayed at top.
        /// value between 0.0 and 1.0 representing size proportion of minimum size of left side.
        /// </summary>
        public double LeftMinimumRelativeSize
        {
            get
            {
                return Interop.Elementary.elm_panes_content_left_min_relative_size_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_panes_content_left_min_relative_size_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the absolute minimum size of panes widget's right side.
        /// If displayed vertically, right content is displayed at top.
        /// value representing minimum size of right side in pixels.
        /// </summary>
        public int RightMinimumSize
        {
            get
            {
                return Interop.Elementary.elm_panes_content_right_min_size_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_panes_content_right_min_size_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the relative minimum size of panes widget's right side.
        /// proportion of minimum size of right side.
        /// If displayed vertically, right content is displayed at top.
        /// value between 0.0 and 1.0 representing size proportion of minimum size of right side.
        /// </summary>
        public double RightMinimumRelativeSize
        {
            get
            {
                return Interop.Elementary.elm_panes_content_right_min_relative_size_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_panes_content_right_min_relative_size_set(RealHandle, value);
            }
        }

        protected override IntPtr CreateHandle(EvasObject parent)
        {
            IntPtr handle = Interop.Elementary.elm_layout_add(parent.Handle);
            Interop.Elementary.elm_layout_theme_set(handle, "layout", "elm_widget", "default");

            RealHandle = Interop.Elementary.elm_panes_add(handle);
            Interop.Elementary.elm_object_part_content_set(handle, "elm.swallow.content", RealHandle);

            return handle;
        }
    }
}