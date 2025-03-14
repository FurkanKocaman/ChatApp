using MediatR;
using TS.Result;

namespace ChatApp.Server.Application.Channels;

public sealed record ChannelCreateCommand(
    ): IRequest<Result<string>>;
