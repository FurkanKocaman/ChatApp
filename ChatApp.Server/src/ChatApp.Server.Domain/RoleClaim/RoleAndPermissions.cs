namespace PersonelYonetim.Server.Domain.RoleClaim;

public static class Permissions
{  // Server Permissions
    public const string CreateServer = "server.create";
    public const string EditServer = "server.edit";
    public const string DeleteServer = "server.delete";
    public const string ManageServerSettings = "server.manageSettings";

    // Channel Permissions
    public const string CreateChannel = "channel.create";
    public const string EditChannel = "channel.edit";
    public const string DeleteChannel = "channel.delete";
    public const string ViewChannel = "channel.view";
    public const string ManageChannelPermissions = "channel.managePermissions";

    // Message Permissions
    public const string SendMessage = "message.send";
    public const string EditOwnMessage = "message.edit.own";
    public const string EditAnyMessage = "message.edit.any";
    public const string DeleteOwnMessage = "message.delete.own";
    public const string DeleteAnyMessage = "message.delete.any";
    public const string PinMessage = "message.pin";
    public const string ViewMessageHistory = "message.viewHistory";

    // Role Permissions
    public const string CreateRole = "role.create";
    public const string EditRole = "role.edit";
    public const string DeleteRole = "role.delete";
    public const string AssignRoles = "role.assign";
    public const string ManagePermissions = "role.managePermissions";

    // Member/User Permissions
    public const string KickMember = "member.kick";
    public const string BanMember = "member.ban";
    public const string UnbanMember = "member.unban";
    public const string ViewMembers = "member.view";
    public const string ManageNicknames = "member.manageNicknames";

    // Voice/Audio Permissions
    public const string ConnectVoice = "voice.connect";
    public const string MuteOthers = "voice.mute.others";
    public const string DeafenOthers = "voice.deafen.others";
    public const string Speak = "voice.speak";
    public const string StreamVideo = "voice.stream";

    // Invite & Links
    public const string CreateInvite = "invite.create";
    public const string DeleteInvite = "invite.delete";
    public const string UseInvite = "invite.use";

    // Miscellaneous / Diğer
    public const string ManageEmojis = "emoji.manage";
    public const string ChangeServerIcon = "server.changeIcon";
    public const string ChangeChannelTopic = "channel.changeTopic";
}

