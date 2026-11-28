using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F86 RID: 3974
	public class ComplexThreatDef : Def
	{
		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x060065A3 RID: 26019 RVA: 0x00242FE7 File Offset: 0x002411E7
		public ComplexThreatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (ComplexThreatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060065A4 RID: 26020 RVA: 0x00243019 File Offset: 0x00241219
		public override IEnumerable<string> ConfigErrors()
		{
			ComplexThreatDef.<ConfigErrors>d__16 <ConfigErrors>d__ = new ComplexThreatDef.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003C63 RID: 15459
		public Type workerClass = typeof(ComplexThreatWorker);

		// Token: 0x04003C64 RID: 15460
		public FactionDef faction;

		// Token: 0x04003C65 RID: 15461
		public float postSpawnPassiveThreatFactor = 1f;

		// Token: 0x04003C66 RID: 15462
		public int minPoints;

		// Token: 0x04003C67 RID: 15463
		public float spawnInOtherRoomChance;

		// Token: 0x04003C68 RID: 15464
		public bool allowPassive = true;

		// Token: 0x04003C69 RID: 15465
		public bool fallbackToRoomEnteredTrigger = true;

		// Token: 0x04003C6A RID: 15466
		public float delayChance;

		// Token: 0x04003C6B RID: 15467
		public List<int> delayTickOptions;

		// Token: 0x04003C6C RID: 15468
		public SimpleCurve threatFactorOverDelayTicksCurve;

		// Token: 0x04003C6D RID: 15469
		public SignalActionAmbushType signalActionAmbushType;

		// Token: 0x04003C6E RID: 15470
		public bool spawnAroundComplex;

		// Token: 0x04003C6F RID: 15471
		public bool useDropPods;

		// Token: 0x04003C70 RID: 15472
		[Unsaved(false)]
		private ComplexThreatWorker workerInt;
	}
}
