// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using LabApi.Events.Arguments.PlayerEvents;

namespace XazeCustomRoles.Interfaces;

public interface ICustomVoiceModule
{
    public void ValidateReceive(PlayerReceivingVoiceMessageEventArgs args);
    public void ValidateSend(PlayerReceivingVoiceMessageEventArgs args);
}