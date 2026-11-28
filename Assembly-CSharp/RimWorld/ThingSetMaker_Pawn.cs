using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B67 RID: 7015
	public class ThingSetMaker_Pawn : ThingSetMaker
	{
		// Token: 0x0600B412 RID: 46098 RVA: 0x003F78B8 File Offset: 0x003F5AB8
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.pawnKind, null, PawnGenerationContext.NonPlayer, parms.tile, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			outThings.Add(pawn);
			if (this.alive != null && !this.alive.Value)
			{
				pawn.Kill(null, null);
				if (this.corpseAgeRangeDays != null)
				{
					int num = Mathf.RoundToInt(this.corpseAgeRangeDays.Value.RandomInRange * 60000f);
					pawn.Corpse.timeOfDeath = Find.TickManager.TicksGame - num;
					CompRottable compRottable = pawn.Corpse.TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						compRottable.RotProgress += (float)num;
					}
				}
			}
		}

		// Token: 0x0600B413 RID: 46099 RVA: 0x003F79F2 File Offset: 0x003F5BF2
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_Pawn.<AllGeneratableThingsDebugSub>d__4 <AllGeneratableThingsDebugSub>d__ = new ThingSetMaker_Pawn.<AllGeneratableThingsDebugSub>d__4(-2);
			<AllGeneratableThingsDebugSub>d__.<>4__this = this;
			return <AllGeneratableThingsDebugSub>d__;
		}

		// Token: 0x0600B414 RID: 46100 RVA: 0x003F7A02 File Offset: 0x003F5C02
		public override IEnumerable<string> ConfigErrors()
		{
			ThingSetMaker_Pawn.<ConfigErrors>d__5 <ConfigErrors>d__ = new ThingSetMaker_Pawn.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040067C6 RID: 26566
		public PawnKindDef pawnKind;

		// Token: 0x040067C7 RID: 26567
		public bool? alive;

		// Token: 0x040067C8 RID: 26568
		public FloatRange? corpseAgeRangeDays;
	}
}
