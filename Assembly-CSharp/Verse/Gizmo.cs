using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000610 RID: 1552
	public abstract class Gizmo
	{
		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x0600307D RID: 12413 RVA: 0x00136C13 File Offset: 0x00134E13
		// (set) Token: 0x0600307E RID: 12414 RVA: 0x00136C1B File Offset: 0x00134E1B
		public virtual bool Disabled
		{
			get
			{
				return this.disabled;
			}
			set
			{
				this.disabled = value;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x0600307F RID: 12415 RVA: 0x00136C24 File Offset: 0x00134E24
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x00136C27 File Offset: 0x00134E27
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x00136C2E File Offset: 0x00134E2E
		// (set) Token: 0x06003082 RID: 12418 RVA: 0x00136C36 File Offset: 0x00134E36
		public virtual float Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		// Token: 0x06003083 RID: 12419
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms);

		// Token: 0x06003084 RID: 12420 RVA: 0x00136C3F File Offset: 0x00134E3F
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x06003085 RID: 12421
		public abstract float GetWidth(float maxWidth);

		// Token: 0x06003086 RID: 12422 RVA: 0x00136C41 File Offset: 0x00134E41
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x00136C43 File Offset: 0x00134E43
		public virtual void ProcessGroupInput(Event ev, List<Gizmo> group)
		{
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x00136C45 File Offset: 0x00134E45
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x00136C48 File Offset: 0x00134E48
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x00136C4A File Offset: 0x00134E4A
		public virtual bool ShowPawnDetailsWith(Gizmo gizmo)
		{
			return false;
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x00136C4D File Offset: 0x00134E4D
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x00136C55 File Offset: 0x00134E55
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x00136C5E File Offset: 0x00134E5E
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x04001F22 RID: 7970
		protected bool disabled;

		// Token: 0x04001F23 RID: 7971
		public string disabledReason;

		// Token: 0x04001F24 RID: 7972
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x04001F25 RID: 7973
		private float order;

		// Token: 0x04001F26 RID: 7974
		public const float Height = 75f;
	}
}
