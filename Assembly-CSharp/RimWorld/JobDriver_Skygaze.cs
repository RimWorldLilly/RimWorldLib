using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009FE RID: 2558
	public class JobDriver_Skygaze : JobDriver
	{
		// Token: 0x06004D65 RID: 19813 RVA: 0x001DA591 File Offset: 0x001D8791
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x001DA594 File Offset: 0x001D8794
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Skygaze.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_Skygaze.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x001DA5A4 File Offset: 0x001D87A4
		public override string GetReport()
		{
			if (base.Map.TileInfo.Layer.Def.isSpace)
			{
				return "Stargazing".Translate();
			}
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
			{
				return "WatchingEclipse".Translate();
			}
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Aurora))
			{
				return "WatchingAurora".Translate();
			}
			float num = GenCelestial.CurCelestialSunGlow(base.Map);
			if (num < 0.1f)
			{
				return "Stargazing".Translate();
			}
			if (num >= 0.65f)
			{
				return "CloudWatching".Translate();
			}
			if (GenLocalDate.DayPercent(this.pawn) < 0.5f)
			{
				return "WatchingSunrise".Translate();
			}
			return "WatchingSunset".Translate();
		}
	}
}
