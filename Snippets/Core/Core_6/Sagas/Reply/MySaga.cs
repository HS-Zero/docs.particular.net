﻿namespace Core6.Sagas.Reply
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region saga-with-reply

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<StartMessage>
    {

        public Task Handle(StartMessage message, IMessageHandlerContext context)
        {
            var almostDoneMessage = new AlmostDoneMessage
            {
                SomeID = Data.SomeID
            };
            return ReplyToOriginator(context, almostDoneMessage);
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
        }

    }

}