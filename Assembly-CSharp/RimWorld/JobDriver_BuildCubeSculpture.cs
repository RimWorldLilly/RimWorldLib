using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F1 RID: 2545
	public class JobDriver_BuildCubeSculpture : JobDriver
	{
		// Token: 0x06004D1E RID: 19742 RVA: 0x001D979A File Offset: 0x001D799A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x001D97BD File Offset: 0x001D79BD
		public override string GetReport()
		{
			if (base.CurToilIndex == 0)
			{
				return "ReportBuildingCubeEnroute".Translate();
			}
			return this.job.def.reportString.Formatted(this.GetMaterialString());
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x001D97FC File Offset: 0x001D79FC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BuildCubeSculpture.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_BuildCubeSculpture.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x001D980C File Offset: 0x001D7A0C
		private void CacheGroundType()
		{
			TerrainDef terrainDef = this.pawn.MapHeld.terrainGrid.TerrainAt(base.TargetA.Cell);
			if (terrainDef == null)
			{
				this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Scrap;
				return;
			}
			if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Soil)
			{
				this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Dirt;
				return;
			}
			if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Stone)
			{
				this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Stone;
				return;
			}
			if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Sand)
			{
				this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Sand;
				return;
			}
			this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Scrap;
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x001D9884 File Offset: 0x001D7A84
		private void PlaceAndFinish()
		{
			Thing thing = GenSpawn.Spawn(this.GetSculptureThing(), base.TargetA.Cell, this.pawn.MapHeld, WipeMode.Vanish);
			thing.SetFaction(Faction.OfPlayer, null);
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(QualityUtility.GenerateQualityCreatedByPawn(this.pawn, SkillDefOf.Construction, false), new ArtGenerationContext?(ArtGenerationContext.Colony));
				QualityUtility.SendCraftNotification(thing, this.pawn);
			}
			CompArt compArt = thing.TryGetComp<CompArt>();
			if (compArt != null)
			{
				if (compQuality == null)
				{
					compArt.InitializeArt(ArtGenerationContext.Colony);
				}
				compArt.JustCreatedBy(this.pawn);
			}
			Hediff_CubeInterest hediff_CubeInterest;
			if (this.pawn.health.hediffSet.TryGetHediff<Hediff_CubeInterest>(out hediff_CubeInterest))
			{
				hediff_CubeInterest.Notify_BuiltSculpture(thing);
			}
			if (this.pawn.InMentalState && this.pawn.MentalStateDef == MentalStateDefOf.CubeSculpting)
			{
				this.pawn.MentalState.RecoverFromState();
			}
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x001D9968 File Offset: 0x001D7B68
		private Thing GetSculptureThing()
		{
			ThingDef def = null;
			if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Dirt)
			{
				def = ThingDefOf.DirtCubeSculpture;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Stone)
			{
				def = ThingDefOf.StoneCubeSculpture;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Sand)
			{
				def = ThingDefOf.SandCubeSculpture;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Scrap)
			{
				def = ThingDefOf.ScrapCubeSculpture;
			}
			return ThingMaker.MakeThing(def, null);
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x001D99C0 File Offset: 0x001D7BC0
		private EffecterDef GetSculptureEffecter()
		{
			EffecterDef result = null;
			if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Dirt)
			{
				result = EffecterDefOf.ConstructDirt;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Stone)
			{
				result = EffecterDefOf.ConstructDirt;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Sand)
			{
				result = EffecterDefOf.ConstructDirt;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Scrap)
			{
				result = EffecterDefOf.ConstructMetal;
			}
			return result;
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x001D9A14 File Offset: 0x001D7C14
		private string GetMaterialString()
		{
			switch (this.cubeMaterial)
			{
			case JobDriver_BuildCubeSculpture.CubeMaterial.Dirt:
				return "CubeMaterialDirt".Translate();
			case JobDriver_BuildCubeSculpture.CubeMaterial.Stone:
				return "CubeMaterialStone".Translate();
			case JobDriver_BuildCubeSculpture.CubeMaterial.Sand:
				return "CubeMaterialSand".Translate();
			case JobDriver_BuildCubeSculpture.CubeMaterial.Scrap:
				return "CubeMaterialScrap".Translate();
			default:
				return "CubeMaterialScrap".Translate();
			}
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x001D9A8F File Offset: 0x001D7C8F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<JobDriver_BuildCubeSculpture.CubeMaterial>(ref this.cubeMaterial, "cubeMaterial", JobDriver_BuildCubeSculpture.CubeMaterial.Dirt, false);
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x0400304D RID: 12365
		private JobDriver_BuildCubeSculpture.CubeMaterial cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Scrap;

		// Token: 0x0400304E RID: 12366
		private float workDone;

		// Token: 0x0400304F RID: 12367
		private const TargetIndex Target = TargetIndex.A;

		// Token: 0x04003050 RID: 12368
		private const int BuildTimeTicks = 5000;

		// Token: 0x02002C66 RID: 11366
		private enum CubeMaterial
		{
			// Token: 0x0400B53F RID: 46399
			Dirt,
			// Token: 0x0400B540 RID: 46400
			Stone,
			// Token: 0x0400B541 RID: 46401
			Sand,
			// Token: 0x0400B542 RID: 46402
			Scrap
		}
	}
}
