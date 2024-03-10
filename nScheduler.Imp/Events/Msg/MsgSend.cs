using MediatR;
using nScheduler.Common.Extensions;
using nScheduler.Common.Models;
using nScheduler.Domain.Repositories.Configs;

namespace nScheduler.Imp.Events.Msg;

public class MsgSend : INotification
{
    public Guid MessageId { get; set; }

    public string Content { get; set; }
}

public class MsgSendHandler : INotificationHandler<MsgSend>
{
    private readonly IMessageCfgRepository repository;

    public MsgSendHandler(IMessageCfgRepository repository)
    {
        this.repository = repository;
    }

    public async Task Handle(MsgSend notification, CancellationToken cancellationToken)
    {
        var message = await repository.Find(notification.MessageId, cancellationToken);
        if (message == null || message.Url.IsEmpty())
        {
            throw new ArgumentNullException("无效地址");
        }

        switch (message.Type)
        {
            case MessageType.DingTalk:
                await RobotChatClient.SendTextMessage(message.Url, notification.Content);
                break;

            case MessageType.Wechat:
                await RobotChatClient.SendTextMessage(message.Url, notification.Content);
                break;

            default:
                throw new InvalidOperationException();
        }
    }
}