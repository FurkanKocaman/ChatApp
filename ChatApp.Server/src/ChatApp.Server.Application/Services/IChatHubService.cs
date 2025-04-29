using ChatApp.Server.Application.Messages;
using ChatApp.Server.Domain.Messages;

namespace ChatApp.Server.Application.Services;
public interface IChatHubService
{
    Task SendMessageAsync(string user, Message message);
    Task JoinServersAsync(string connectionId,List<Guid> serverIds, string userId);
    Task OnDisconnectedAsync(string connectionId);
    Task JoinChannelAsync(string connectionId, string channelId);
    Task LeaveChannelAsync(string connectionId, string channelId);
    public Task SendMessageToChannelAsync(string channelId, MessagesGetAllQueryResponse message);
}
