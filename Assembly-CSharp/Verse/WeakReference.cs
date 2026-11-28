using System;

namespace Verse
{
	// Token: 0x020006F9 RID: 1785
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x060038C3 RID: 14531 RVA: 0x0016C94C File Offset: 0x0016AB4C
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x060038C4 RID: 14532 RVA: 0x0016C95A File Offset: 0x0016AB5A
		// (set) Token: 0x060038C5 RID: 14533 RVA: 0x0016C967 File Offset: 0x0016AB67
		public new T Target
		{
			get
			{
				return (T)((object)base.Target);
			}
			set
			{
				base.Target = value;
			}
		}
	}
}
