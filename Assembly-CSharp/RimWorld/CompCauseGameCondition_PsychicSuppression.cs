using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FD RID: 6141
	public class CompCauseGameCondition_PsychicSuppression : CompCauseGameCondition
	{
		// Token: 0x06009947 RID: 39239 RVA: 0x003774E9 File Offset: 0x003756E9
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		// Token: 0x06009948 RID: 39240 RVA: 0x00377503 File Offset: 0x00375703
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x06009949 RID: 39241 RVA: 0x0037751D File Offset: 0x0037571D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompCauseGameCondition_PsychicSuppression.<CompGetGizmosExtra>d__3 <CompGetGizmosExtra>d__ = new CompCauseGameCondition_PsychicSuppression.<CompGetGizmosExtra>d__3(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600994A RID: 39242 RVA: 0x00377530 File Offset: 0x00375730
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Active || !base.MyTile.Valid)
			{
				return;
			}
			foreach (Caravan caravan in Find.World.worldObjects.Caravans)
			{
				if (Find.WorldGrid.ApproxDistanceInTiles(caravan.Tile, base.MyTile) < (float)base.Props.worldRange)
				{
					foreach (Pawn pawn in caravan.pawns)
					{
						GameCondition_PsychicSuppression.CheckPawn(pawn, this.gender);
					}
				}
			}
		}

		// Token: 0x0600994B RID: 39243 RVA: 0x00377610 File Offset: 0x00375810
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_PsychicSuppression)condition).gender = this.gender;
		}

		// Token: 0x0600994C RID: 39244 RVA: 0x0037762C File Offset: 0x0037582C
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst());
		}

		// Token: 0x0600994D RID: 39245 RVA: 0x0037768B File Offset: 0x0037588B
		public override void RandomizeSettings(Site site)
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		// Token: 0x04005B84 RID: 23428
		public Gender gender;
	}
}
