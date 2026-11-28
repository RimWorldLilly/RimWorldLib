using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004CA RID: 1226
	public class Hediff_ShardHolder : HediffWithComps
	{
		// Token: 0x0600222F RID: 8751 RVA: 0x000CB082 File Offset: 0x000C9282
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Shard holder"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000CB0AC File Offset: 0x000C92AC
		public override void Notify_PawnKilled()
		{
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			Thing thing;
			if (!GenDrop.TryDropSpawn(ThingMaker.MakeThing(ThingDefOf.Shard, null), this.pawn.PositionHeld, this.pawn.MapHeld, ThingPlaceMode.Near, out thing, null, null, true))
			{
				return;
			}
			thing.SetForbidden(!thing.MapHeld.areaManager.Home[thing.PositionHeld], true);
			string text = this.pawn.LabelShort;
			if (this.pawn.IsMutant)
			{
				text = Find.ActiveLanguageWorker.WithDefiniteArticle(this.pawn.mutant.Def.label, false, false);
			}
			else if (this.pawn.Name == null)
			{
				text = Find.ActiveLanguageWorker.WithDefiniteArticle(text, false, false);
			}
			Messages.Message("MessageShardDropped".Translate(text).CapitalizeFirst(), thing, MessageTypeDefOf.NeutralEvent, true);
		}
	}
}
