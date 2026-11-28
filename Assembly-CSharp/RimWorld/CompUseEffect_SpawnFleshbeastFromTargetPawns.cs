using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A48 RID: 6728
	public class CompUseEffect_SpawnFleshbeastFromTargetPawns : CompUseEffect
	{
		// Token: 0x0600A891 RID: 43153 RVA: 0x003BDF4B File Offset: 0x003BC14B
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.targetable = this.parent.GetComp<CompTargetable>();
		}

		// Token: 0x0600A892 RID: 43154 RVA: 0x003BDF68 File Offset: 0x003BC168
		public override void DoEffect(Pawn usedBy)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (this.targetable == null)
			{
				Log.Error("CompUseEffect_SpawnFleshbeastFromTargetPawns requires a CompTargetable");
				return;
			}
			Thing[] array = this.targetable.GetTargets(null).ToArray<Thing>();
			for (int i = 0; i < array.Length; i++)
			{
				Pawn pawn = array[i] as Pawn;
				if (pawn != null)
				{
					FleshbeastUtility.SpawnFleshbeastFromPawn(pawn, false, false, Array.Empty<PawnKindDef>());
				}
			}
		}

		// Token: 0x0600A893 RID: 43155 RVA: 0x003BDFCA File Offset: 0x003BC1CA
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.targetable == null)
			{
				this.targetable = this.parent.GetComp<CompTargetable>();
			}
		}

		// Token: 0x04006302 RID: 25346
		private CompTargetable targetable;
	}
}
