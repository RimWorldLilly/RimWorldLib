using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000096 RID: 150
	public class LogMessage
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x000262E4 File Offset: 0x000244E4
		public Color Color
		{
			get
			{
				switch (this.type)
				{
				case LogMessageType.Message:
					return Color.white;
				case LogMessageType.Warning:
					return Color.yellow;
				case LogMessageType.Error:
					return ColorLibrary.LogError;
				default:
					return Color.white;
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00026323 File Offset: 0x00024523
		public string StackTrace
		{
			get
			{
				if (this.stackTrace != null)
				{
					return this.stackTrace;
				}
				return "No stack trace.";
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0002633C File Offset: 0x0002453C
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
			this.timestamp = DateTime.Now.ToString("HH:mm:ss");
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00026384 File Offset: 0x00024584
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
			this.timestamp = DateTime.Now.ToString("HH:mm:ss");
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000263CC File Offset: 0x000245CC
		public override string ToString()
		{
			string text = "[" + this.timestamp + "] " + this.text;
			if (this.repeats > 1)
			{
				return "(" + this.repeats.ToString() + ") " + text;
			}
			return text;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002641B File Offset: 0x0002461B
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		// Token: 0x04000243 RID: 579
		public string text;

		// Token: 0x04000244 RID: 580
		public LogMessageType type;

		// Token: 0x04000245 RID: 581
		public int repeats = 1;

		// Token: 0x04000246 RID: 582
		private string stackTrace;

		// Token: 0x04000247 RID: 583
		private string timestamp;
	}
}
