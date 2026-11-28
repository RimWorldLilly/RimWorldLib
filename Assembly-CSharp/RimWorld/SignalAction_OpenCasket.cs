using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B09 RID: 6921
	public class SignalAction_OpenCasket : SignalAction_Delay
	{
		// Token: 0x17001FE5 RID: 8165
		// (get) Token: 0x0600B0ED RID: 45293 RVA: 0x003E55FD File Offset: 0x003E37FD
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null)
				{
					this.cachedAlert = new Alert_CasketOpening(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x17001FE6 RID: 8166
		// (get) Token: 0x0600B0EE RID: 45294 RVA: 0x003E561C File Offset: 0x003E381C
		public override bool ShouldRemoveNow
		{
			get
			{
				for (int i = 0; i < this.caskets.Count; i++)
				{
					if (this.caskets[i] != null && !this.caskets[i].Destroyed && ((Building_Casket)this.caskets[i]).HasAnyContents)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600B0EF RID: 45295 RVA: 0x003E567C File Offset: 0x003E387C
		protected override void Complete()
		{
			base.Complete();
			for (int i = 0; i < this.caskets.Count; i++)
			{
				Building_Casket building_Casket = (Building_Casket)this.caskets[i];
				if (building_Casket.CanOpen)
				{
					building_Casket.Open();
				}
			}
		}

		// Token: 0x0600B0F0 RID: 45296 RVA: 0x003E56C5 File Offset: 0x003E38C5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.caskets, "caskets", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0400662A RID: 26154
		public List<Thing> caskets = new List<Thing>();

		// Token: 0x0400662B RID: 26155
		private Alert_ActionDelay cachedAlert;
	}
}
