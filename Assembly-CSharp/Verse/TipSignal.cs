using System;

namespace Verse
{
	// Token: 0x0200063F RID: 1599
	public struct TipSignal
	{
		// Token: 0x0600321E RID: 12830 RVA: 0x0013F8C7 File Offset: 0x0013DAC7
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x0013F8F0 File Offset: 0x0013DAF0
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x0013F919 File Offset: 0x0013DB19
		public TipSignal(string text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x0013F951 File Offset: 0x0013DB51
		public TipSignal(string text, float delay)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = delay;
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x0013F988 File Offset: 0x0013DB88
		public TipSignal(TaggedString text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text.Resolve();
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x0013F9E2 File Offset: 0x0013DBE2
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x0013FA0F File Offset: 0x0013DC0F
		public TipSignal(Func<string> textGetter, int uniqueId, TooltipPriority priority)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x0013FA3C File Offset: 0x0013DC3C
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
			this.delay = 0.45f;
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x0013FA74 File Offset: 0x0013DC74
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x0013FA7C File Offset: 0x0013DC7C
		public static implicit operator TipSignal(TaggedString str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x0013FA84 File Offset: 0x0013DC84
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Tip(",
				this.text,
				", ",
				this.uniqueId.ToString(),
				")"
			});
		}

		// Token: 0x04002010 RID: 8208
		public const float DefaultDelay = 0.45f;

		// Token: 0x04002011 RID: 8209
		public string text;

		// Token: 0x04002012 RID: 8210
		public Func<string> textGetter;

		// Token: 0x04002013 RID: 8211
		public int uniqueId;

		// Token: 0x04002014 RID: 8212
		public TooltipPriority priority;

		// Token: 0x04002015 RID: 8213
		public float delay;
	}
}
