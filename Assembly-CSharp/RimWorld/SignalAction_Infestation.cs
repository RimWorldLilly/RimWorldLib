using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B06 RID: 6918
	public class SignalAction_Infestation : SignalAction_Delay
	{
		// Token: 0x17001FE4 RID: 8164
		// (get) Token: 0x0600B0E1 RID: 45281 RVA: 0x003E50C6 File Offset: 0x003E32C6
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null)
				{
					this.cachedAlert = new Alert_InfestationDelay(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x0600B0E2 RID: 45282 RVA: 0x003E50E4 File Offset: 0x003E32E4
		protected override void Complete()
		{
			base.Complete();
			Thing thing = InfestationUtility.SpawnTunnels(this.hivesCount, base.Map, this.spawnAnywhereIfNoGoodCell, this.ignoreRoofedRequirement, null, this.overrideLoc, this.insectsPoints);
			if (thing != null && this.sendStandardLetter)
			{
				IntVec3 cell = this.overrideLoc ?? thing.Position;
				Find.LetterStack.ReceiveLetter(IncidentDefOf.Infestation.letterLabel, IncidentDefOf.Infestation.letterText, IncidentDefOf.Infestation.letterDef, new TargetInfo(cell, base.Map, false), null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600B0E3 RID: 45283 RVA: 0x003E5198 File Offset: 0x003E3398
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hivesCount, "hivesCount", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnAnywhereIfNoGoodCell, "spawnAnywhereIfNoGoodCell", false, false);
			Scribe_Values.Look<bool>(ref this.ignoreRoofedRequirement, "ignoreRoofedRequirement", false, false);
			Scribe_Values.Look<IntVec3?>(ref this.overrideLoc, "overrideLoc", null, false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", false, false);
			Scribe_Values.Look<float?>(ref this.insectsPoints, "insectsPoints", null, false);
		}

		// Token: 0x04006615 RID: 26133
		public int hivesCount = 1;

		// Token: 0x04006616 RID: 26134
		public float? insectsPoints;

		// Token: 0x04006617 RID: 26135
		public bool spawnAnywhereIfNoGoodCell;

		// Token: 0x04006618 RID: 26136
		public bool ignoreRoofedRequirement;

		// Token: 0x04006619 RID: 26137
		public IntVec3? overrideLoc;

		// Token: 0x0400661A RID: 26138
		public bool sendStandardLetter;

		// Token: 0x0400661B RID: 26139
		private Alert_ActionDelay cachedAlert;
	}
}
