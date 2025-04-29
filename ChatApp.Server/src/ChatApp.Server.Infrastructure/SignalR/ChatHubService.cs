using ChatApp.Server.Application.Messages;
using ChatApp.Server.Application.Services;
using ChatApp.Server.Domain.Messages;
using ChatApp.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Infrastructure.SignalR;
internal sealed class ChatHubService(IHubContext<ChatHub> hubContext, UserManager<AppUser> userManager) : IChatHubService
{
    public static Dictionary<string, Guid> AllClients = new Dictionary<string, Guid>();
    private static Dictionary<string, HashSet<string>> UserChannels = new Dictionary<string, HashSet<string>>();

    public async Task JoinChannelAsync(string connectionId, string channelId)
    {
        if (!UserChannels.ContainsKey(connectionId))
        {
            UserChannels[connectionId] = new HashSet<string>();
        }

        if (!UserChannels[connectionId].Contains(channelId))
        {
            await hubContext.Groups.AddToGroupAsync(connectionId, channelId);
            UserChannels[connectionId].Add(channelId);
        }
    }

    public async Task JoinServersAsync(string connectionId, List<Guid> serverIds, string userId)
    {
        AllClients.Add(connectionId, Guid.Parse(userId));
        var user = await userManager.FindByIdAsync(userId);
        if (user is not null)
            user.Status = UserStatus.Online;
        foreach (var serverId in serverIds)
        {
            await hubContext.Groups.AddToGroupAsync(connectionId, $"server-{serverId}");
            await hubContext.Clients.Group($"server-{serverId}").SendAsync("UserConnected", userId);
        }
    }

    public async Task LeaveChannelAsync(string connectionId, string channelId)
    {
        if (UserChannels.ContainsKey(connectionId) && UserChannels[connectionId].Contains(channelId))
        {
            await hubContext.Groups.RemoveFromGroupAsync(connectionId, channelId);
            UserChannels[connectionId].Remove(channelId);
        }
    }

    public async Task OnDisconnectedAsync(string connectionId)
    {
        if (AllClients.TryGetValue(connectionId, out var userId))
        {
            AllClients.Remove(connectionId);

            await hubContext.Clients.All.SendAsync("UserDisconnected", userId.ToString()); // Burada herkese gönderiyor ilerde sadece serverlara göndersin
        }

        // Kullanıcı kanallarından tümünü çıkarma
        if (UserChannels.ContainsKey(connectionId))
        {
            var channels = UserChannels[connectionId].ToList();
            foreach (var channel in channels)
            {
                await hubContext.Groups.RemoveFromGroupAsync(connectionId, channel);
            }
            UserChannels.Remove(connectionId);
        }
    }

    public Task SendMessageAsync(string userId, Message message)
    {
        throw new NotImplementedException();
    }

    public async Task SendMessageToChannelAsync(string channelId, MessagesGetAllQueryResponse message)
    {
        await hubContext.Clients.Group(channelId).SendAsync("ReceiveChannelMessage", message);
      
    }
}
