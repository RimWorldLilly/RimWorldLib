using System;

namespace Verse.AI.Group
{
	// Token: 0x02000917 RID: 2327
	public class PsychicRitualLordToilData : LordToilData
	{
		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x060046F7 RID: 18167 RVA: 0x001BDDC0 File Offset: 0x001BBFC0
		public PsychicRitualToil CurPsychicRitualToil
		{
			get
			{
				PsychicRitualGraph psychicRitualGraph = this.psychicRitualToil as PsychicRitualGraph;
				if (psychicRitualGraph != null)
				{
					return psychicRitualGraph.CurrentToil;
				}
				return this.psychicRitualToil;
			}
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x001BDDEC File Offset: 0x001BBFEC
		public override void ExposeData()
		{
			Scribe_Deep.Look<PsychicRitualToil>(ref this.psychicRitualToil, "psychicRitualToil", Array.Empty<object>());
			Scribe_Deep.Look<PsychicRitual>(ref this.psychicRitual, "psychicRitual", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.done, "done", false, false);
			Scribe_Values.Look<int>(ref this.iteration, "iteration", 0, false);
			Scribe_Values.Look<bool>(ref this.playerRitual, "playerRitual", false, false);
			Scribe_Values.Look<bool>(ref this.removeLordOnCancel, "removeLordOnCancel", false, false);
		}

		// Token: 0x04002E21 RID: 11809
		public PsychicRitual psychicRitual;

		// Token: 0x04002E22 RID: 11810
		public PsychicRitualToil psychicRitualToil;

		// Token: 0x04002E23 RID: 11811
		public bool done;

		// Token: 0x04002E24 RID: 11812
		public int iteration;

		// Token: 0x04002E25 RID: 11813
		public bool playerRitual;

		// Token: 0x04002E26 RID: 11814
		public bool removeLordOnCancel;
	}
}
