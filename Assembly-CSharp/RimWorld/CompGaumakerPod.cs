using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001870 RID: 6256
	public class CompGaumakerPod : CompDryadHolder
	{
		// Token: 0x17001ABE RID: 6846
		// (get) Token: 0x06009C7F RID: 40063 RVA: 0x00385920 File Offset: 0x00383B20
		public bool Full
		{
			get
			{
				return this.innerContainer.Count >= 3;
			}
		}

		// Token: 0x06009C80 RID: 40064 RVA: 0x00385933 File Offset: 0x00383B33
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace && Find.TickManager.TicksGame < this.tickComplete)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			}
		}

		// Token: 0x06009C81 RID: 40065 RVA: 0x00385967 File Offset: 0x00383B67
		public override void TryAcceptPawn(Pawn p)
		{
			base.TryAcceptPawn(p);
			if (this.Full)
			{
				this.tickComplete = Find.TickManager.TicksGame + (int)(60000f * base.Props.daysToComplete);
			}
		}

		// Token: 0x06009C82 RID: 40066 RVA: 0x0038599C File Offset: 0x00383B9C
		protected override void Complete()
		{
			this.tickComplete = Find.TickManager.TicksGame;
			if (base.TreeComp != null)
			{
				for (int i = this.innerContainer.Count - 1; i >= 0; i--)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null)
					{
						base.TreeComp.RemoveDryad(pawn);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
				}
				base.TreeComp.gaumakerPod = null;
				((Plant)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_PodGauranlen, null), this.parent.Position, this.parent.Map, WipeMode.Vanish)).Growth = 1f;
			}
			this.parent.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06009C83 RID: 40067 RVA: 0x00385A58 File Offset: 0x00383C58
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.innerContainer.Count < 3)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = string.Concat(new string[]
				{
					text,
					GenLabel.BestKindLabel(PawnKindDefOf.Dryad_Gaumaker, Gender.Male, true, -1).CapitalizeFirst(),
					": ",
					this.innerContainer.Count.ToString(),
					"/",
					3.ToString()
				});
			}
			return text;
		}
	}
}
