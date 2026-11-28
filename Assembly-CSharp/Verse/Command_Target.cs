using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200060A RID: 1546
	public class Command_Target : Command
	{
		// Token: 0x06003067 RID: 12391 RVA: 0x001367A0 File Offset: 0x001349A0
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.DesignatorManager.Deselect();
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target);
			}, null, null, null, null, null, true, null, new Action<LocalTargetInfo>(this.Update));
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x001367F8 File Offset: 0x001349F8
		public override void GizmoUpdateOnMouseover()
		{
			this.Update(null);
			base.GizmoUpdateOnMouseover();
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x0013680C File Offset: 0x00134A0C
		private void Update(LocalTargetInfo target)
		{
			if (this.lastUpdate == Time.frameCount)
			{
				return;
			}
			this.lastUpdate = Time.frameCount;
			Action<LocalTargetInfo> action = this.onUpdate;
			if (action == null)
			{
				return;
			}
			action(target);
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x00136838 File Offset: 0x00134A38
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x04001F07 RID: 7943
		public Action<LocalTargetInfo> action;

		// Token: 0x04001F08 RID: 7944
		public Action<LocalTargetInfo> onUpdate;

		// Token: 0x04001F09 RID: 7945
		public TargetingParameters targetingParams;

		// Token: 0x04001F0A RID: 7946
		private int lastUpdate;
	}
}
