using System;
using System.Collections.Generic;
using LudeonTK;

namespace Verse
{
	// Token: 0x02000097 RID: 151
	public class LogMessageQueue
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x00026440 File Offset: 0x00024640
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00026448 File Offset: 0x00024648
		public void Enqueue(LogMessage msg)
		{
			bool flag;
			this.Enqueue(msg, out flag);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00026460 File Offset: 0x00024660
		public void Enqueue(LogMessage msg, out bool repeatsCapped)
		{
			repeatsCapped = false;
			if (this.lastMessage == null || !msg.CanCombineWith(this.lastMessage))
			{
				this.lastMessage = msg;
				this.messages.Enqueue(msg);
				if (this.messages.Count > this.maxMessages)
				{
					EditWindow_Log.Notify_MessageDequeued(this.messages.Dequeue());
				}
				return;
			}
			if (this.lastMessage.repeats >= 99)
			{
				repeatsCapped = true;
				return;
			}
			this.lastMessage.repeats++;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x000264E3 File Offset: 0x000246E3
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x04000248 RID: 584
		public int maxMessages = 1000;

		// Token: 0x04000249 RID: 585
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x0400024A RID: 586
		private LogMessage lastMessage;
	}
}
