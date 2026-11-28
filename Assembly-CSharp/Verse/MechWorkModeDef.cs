using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000190 RID: 400
	public class MechWorkModeDef : Def
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0003D5DF File Offset: 0x0003B7DF
		public WorkModeDrawer Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkModeDrawer)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0003D611 File Offset: 0x0003B811
		public override void PostLoad()
		{
			if (!string.IsNullOrEmpty(this.iconPath))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x040009A1 RID: 2465
		[NoTranslate]
		public string iconPath;

		// Token: 0x040009A2 RID: 2466
		public Texture2D uiIcon;

		// Token: 0x040009A3 RID: 2467
		public int uiOrder;

		// Token: 0x040009A4 RID: 2468
		public bool ignoreGroupChargeLimits;

		// Token: 0x040009A5 RID: 2469
		public Type workerClass = typeof(WorkModeDrawer);

		// Token: 0x040009A6 RID: 2470
		[Unsaved(false)]
		private WorkModeDrawer workerInt;
	}
}
