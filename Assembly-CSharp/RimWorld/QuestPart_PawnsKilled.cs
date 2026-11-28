using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D7 RID: 4311
	public class QuestPart_PawnsKilled : QuestPartActivable
	{
		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06006BCE RID: 27598 RVA: 0x0025D940 File Offset: 0x0025BB40
		public override string DescriptionPart
		{
			get
			{
				return "PawnsKilled".Translate(GenLabel.BestKindLabel(this.race.race.AnyPawnKind, Gender.None, true, -1)).CapitalizeFirst() + ": " + this.killed.ToString() + " / " + this.count.ToString();
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06006BCF RID: 27599 RVA: 0x0025D9AB File Offset: 0x0025BBAB
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_PawnsKilled.<get_InvolvedFactions>d__9 <get_InvolvedFactions>d__ = new QuestPart_PawnsKilled.<get_InvolvedFactions>d__9(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x06006BD0 RID: 27600 RVA: 0x0025D9BB File Offset: 0x0025BBBB
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.killed = 0;
		}

		// Token: 0x06006BD1 RID: 27601 RVA: 0x0025D9CC File Offset: 0x0025BBCC
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (base.State == QuestPartState.Enabled && pawn.def == this.race && (this.requiredInstigatorFaction == null || (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == this.requiredInstigatorFaction))))
			{
				this.killed++;
				Find.SignalManager.SendSignal(new Signal(this.outSignalPawnKilled, false));
				if (this.killed >= this.count)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x06006BD2 RID: 27602 RVA: 0x0025DA74 File Offset: 0x0025BC74
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<Faction>(ref this.requiredInstigatorFaction, "requiredInstigatorFaction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.killed, "killed", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnKilled, "outSignalPawnKilled", null, false);
		}

		// Token: 0x06006BD3 RID: 27603 RVA: 0x0025DAEF File Offset: 0x0025BCEF
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.race = ThingDefOf.Muffalo;
			this.requiredInstigatorFaction = Faction.OfPlayer;
			this.count = 10;
		}

		// Token: 0x0400448F RID: 17551
		public ThingDef race;

		// Token: 0x04004490 RID: 17552
		public Faction requiredInstigatorFaction;

		// Token: 0x04004491 RID: 17553
		public int count;

		// Token: 0x04004492 RID: 17554
		public MapParent mapParent;

		// Token: 0x04004493 RID: 17555
		public string outSignalPawnKilled;

		// Token: 0x04004494 RID: 17556
		private int killed;
	}
}
