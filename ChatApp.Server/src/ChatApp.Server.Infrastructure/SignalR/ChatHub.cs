using ChatApp.Server.Application.Messages;
using ChatApp.Server.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Infrastructure.SignalR;
public sealed class ChatHub(IChatHubService chatHubService) : Hub
{
    public async Task JoinServers(List<Guid> serverIds, string userId)
    {
        await chatHubService.JoinServersAsync(Context.ConnectionId, serverIds, userId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await chatHubService.OnDisconnectedAsync(Context.ConnectionId);
       
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinChannel(string channelId)
    {
        await chatHubService.JoinChannelAsync(Context.ConnectionId, channelId);
    }

    public async Task LeaveChannel(string channelId)
    {
        await chatHubService.LeaveChannelAsync(Context.ConnectionId, channelId);
    }

    //public async Task SendMessageToChannel(string channelId, MessagesGetAllQueryResponse message)
    //{
    //    await chatHubService.SendMessageToChannelAsync(channelId, message);
    //}
}
