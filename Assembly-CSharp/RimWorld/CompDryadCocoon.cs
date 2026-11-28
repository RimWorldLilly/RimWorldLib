using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200186F RID: 6255
	public class CompDryadCocoon : CompDryadHolder
	{
		// Token: 0x06009C76 RID: 40054 RVA: 0x003855FB File Offset: 0x003837FB
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
				this.tickExpire = Find.TickManager.TicksGame + 600;
			}
		}

		// Token: 0x06009C77 RID: 40055 RVA: 0x00385628 File Offset: 0x00383828
		public override void TryAcceptPawn(Pawn p)
		{
			base.TryAcceptPawn(p);
			p.Rotation = Rot4.South;
			this.tickComplete = Find.TickManager.TicksGame + (int)(60000f * base.Props.daysToComplete);
			this.tickExpire = -1;
			this.dryadKind = base.TreeComp.DryadKind;
		}

		// Token: 0x06009C78 RID: 40056 RVA: 0x00385684 File Offset: 0x00383884
		protected override void Complete()
		{
			this.tickComplete = Find.TickManager.TicksGame;
			CompTreeConnection treeComp = base.TreeComp;
			if (treeComp != null && this.innerContainer.Count > 0)
			{
				Pawn pawn = (Pawn)this.innerContainer[0];
				long ageBiologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
				treeComp.RemoveDryad(pawn);
				Pawn pawn2 = treeComp.GenerateNewDryad(this.dryadKind);
				pawn2.ageTracker.AgeBiologicalTicks = ageBiologicalTicks;
				if (!pawn.Name.Numerical)
				{
					pawn2.Name = pawn.Name;
				}
				pawn.Destroy(DestroyMode.Vanish);
				this.innerContainer.TryAddOrTransfer(pawn2, 1, true);
				EffecterDefOf.DryadEmergeFromCocoon.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			}
			this.parent.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06009C79 RID: 40057 RVA: 0x00385762 File Offset: 0x00383962
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, delegate(Thing t, int c)
				{
					t.Rotation = Rot4.South;
					SoundDefOf.Pawn_Dryad_Spawn.PlayOneShot(this.parent);
				}, null, false);
			}
		}

		// Token: 0x06009C7A RID: 40058 RVA: 0x00385790 File Offset: 0x00383990
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Destroyed)
			{
				return;
			}
			if (this.dryadKind != null && this.dryadKind != base.TreeComp.DryadKind)
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.innerContainer.Count > 0 && this.tree != null && base.TreeComp.ShouldReturnToTree((Pawn)this.innerContainer[0]))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.tickExpire >= 0 && Find.TickManager.TicksGame >= this.tickExpire)
			{
				this.tickExpire = -1;
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06009C7B RID: 40059 RVA: 0x00385848 File Offset: 0x00383A48
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!this.innerContainer.NullOrEmpty<Thing>() && this.dryadKind != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "ChangingDryadIntoType".Translate(this.innerContainer[0].Named("DRYAD"), this.dryadKind.Named("TYPE")).Resolve();
			}
			return text;
		}

		// Token: 0x06009C7C RID: 40060 RVA: 0x003858C5 File Offset: 0x00383AC5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.tickExpire, "tickExpire", -1, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.dryadKind, "dryadKind");
		}

		// Token: 0x04005CEF RID: 23791
		private int tickExpire = -1;

		// Token: 0x04005CF0 RID: 23792
		private PawnKindDef dryadKind;
	}
}
