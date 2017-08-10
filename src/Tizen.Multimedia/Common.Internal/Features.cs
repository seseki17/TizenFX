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

using Tizen.System;

namespace Tizen.Multimedia
{
    internal static class Features
    {
        internal const string AudioEffect = "http://tizen.org/feature/multimedia.custom_audio_effect";
        internal const string RawVideo = "http://tizen.org/feature/multimedia.raw_video";

        internal static bool IsSupported(string featureKey)
        {
            bool supported = false;
            SystemInfo.TryGetValue(featureKey, out supported);
            return supported;
        }

    }
}