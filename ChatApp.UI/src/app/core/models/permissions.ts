export const Permissions = {
  // Server Permissions
  CreateServer: "server.create",
  EditServer: "server.edit",
  DeleteServer: "server.delete",
  ManageServerSettings: "server.manageSettings",

  // Channel Permissions
  CreateChannel: "channel.create",
  EditChannel: "channel.edit",
  DeleteChannel: "channel.delete",
  ViewChannel: "channel.view",
  ManageChannelPermissions: "channel.managePermissions",

  // Message Permissions
  SendMessage: "message.send",
  EditOwnMessage: "message.edit.own",
  EditAnyMessage: "message.edit.any",
  DeleteOwnMessage: "message.delete.own",
  DeleteAnyMessage: "message.delete.any",
  PinMessage: "message.pin",
  ViewMessageHistory: "message.viewHistory",

  // Role Permissions
  CreateRole: "role.create",
  EditRole: "role.edit",
  DeleteRole: "role.delete",
  AssignRoles: "role.assign",
  ManagePermissions: "role.managePermissions",

  // Member/User Permissions
  KickMember: "member.kick",
  BanMember: "member.ban",
  UnbanMember: "member.unban",
  ViewMembers: "member.view",
  ManageNicknames: "member.manageNicknames",

  // Voice/Audio Permissions
  ConnectVoice: "voice.connect",
  MuteOthers: "voice.mute.others",
  DeafenOthers: "voice.deafen.others",
  Speak: "voice.speak",
  StreamVideo: "voice.stream",

  // Invite & Links
  CreateInvite: "invite.create",
  DeleteInvite: "invite.delete",
  UseInvite: "invite.use",

  // Miscellaneous
  ManageEmojis: "emoji.manage",
  ChangeServerIcon: "server.changeIcon",
  ChangeChannelTopic: "channel.changeTopic",
} as const;

export function hasPermission(userPermissions: Permission[], required: Permission): boolean {
  return userPermissions.includes(required);
}

export type Permission = (typeof Permissions)[keyof typeof Permissions];
