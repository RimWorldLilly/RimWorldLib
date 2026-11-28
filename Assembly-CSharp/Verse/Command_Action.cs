using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000602 RID: 1538
	public class Command_Action : Command
	{
		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003047 RID: 12359 RVA: 0x00135C28 File Offset: 0x00133E28
		public override Color IconDrawColor
		{
			get
			{
				Color? color = this.iconDrawColorOverride;
				if (color == null)
				{
					return base.IconDrawColor;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x00135C53 File Offset: 0x00133E53
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x00135C67 File Offset: 0x00133E67
		public override void GizmoUpdateOnMouseover()
		{
			if (this.onHover != null)
			{
				this.onHover();
			}
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x00135C7C File Offset: 0x00133E7C
		public void SetColorOverride(Color color)
		{
			this.iconDrawColorOverride = new Color?(color);
		}

		// Token: 0x04001EF4 RID: 7924
		public Action action;

		// Token: 0x04001EF5 RID: 7925
		public Action onHover;

		// Token: 0x04001EF6 RID: 7926
		private Color? iconDrawColorOverride;
	}
}
