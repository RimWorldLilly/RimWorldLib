using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017DE RID: 6110
	public class Tale_SinglePawn : Tale
	{
		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x06009884 RID: 39044 RVA: 0x00374607 File Offset: 0x00372807
		public override Pawn DominantPawn
		{
			get
			{
				return this.pawnData.pawn;
			}
		}

		// Token: 0x170019E7 RID: 6631
		// (get) Token: 0x06009885 RID: 39045 RVA: 0x00374614 File Offset: 0x00372814
		public override string ShortSummary
		{
			get
			{
				string str = this.def.LabelCap + ": ";
				Name name = this.pawnData.name;
				return str + ((name != null) ? name.ToString() : null);
			}
		}

		// Token: 0x06009886 RID: 39046 RVA: 0x0037464C File Offset: 0x0037284C
		public Tale_SinglePawn()
		{
		}

		// Token: 0x06009887 RID: 39047 RVA: 0x00374654 File Offset: 0x00372854
		public Tale_SinglePawn(Pawn pawn)
		{
			this.pawnData = TaleData_Pawn.GenerateFrom(pawn);
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				this.surroundings = TaleData_Surroundings.GenerateFrom(pawn.PositionHeld, pawn.MapHeld);
			}
		}

		// Token: 0x06009888 RID: 39048 RVA: 0x00374687 File Offset: 0x00372887
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.pawnData.pawn == th;
		}

		// Token: 0x06009889 RID: 39049 RVA: 0x003746A2 File Offset: 0x003728A2
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.pawnData.faction == faction)
			{
				this.pawnData.faction = null;
			}
		}

		// Token: 0x0600988A RID: 39050 RVA: 0x003746BE File Offset: 0x003728BE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Pawn>(ref this.pawnData, "pawnData", Array.Empty<object>());
		}

		// Token: 0x0600988B RID: 39051 RVA: 0x003746DB File Offset: 0x003728DB
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			Tale_SinglePawn.<SpecialTextGenerationRules>d__10 <SpecialTextGenerationRules>d__ = new Tale_SinglePawn.<SpecialTextGenerationRules>d__10(-2);
			<SpecialTextGenerationRules>d__.<>4__this = this;
			<SpecialTextGenerationRules>d__.<>3__outConstants = outConstants;
			return <SpecialTextGenerationRules>d__;
		}

		// Token: 0x0600988C RID: 39052 RVA: 0x003746F2 File Offset: 0x003728F2
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.pawnData = TaleData_Pawn.GenerateRandom(false);
		}

		// Token: 0x04005B2D RID: 23341
		public TaleData_Pawn pawnData;
	}
}
