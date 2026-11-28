using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001723 RID: 5923
	public class ActionOnTick_ReleaseLantern : ActionOnTick
	{
		// Token: 0x060093C8 RID: 37832 RVA: 0x0035E758 File Offset: 0x0035C958
		public override void Apply(LordJob_Ritual ritual)
		{
			if (!ritual.lord.ownedPawns.Contains(this.pawn))
			{
				return;
			}
			if (this.pawn.carryTracker.CarriedCount(ThingDefOf.WoodLog) >= this.woodCost)
			{
				this.pawn.carryTracker.DestroyCarriedThing();
				Thing newThing = ThingMaker.MakeThing(ThingDefOf.SkyLantern, null);
				IntVec3 intVec = this.pawn.Position + new IntVec3(-1, 0, 1);
				if (intVec.InBounds(this.pawn.Map))
				{
					GenSpawn.Spawn(newThing, intVec, this.pawn.Map, WipeMode.Vanish);
					ritual.AddTagForPawn(this.pawn, "LaunchedSkyLantern");
				}
				SoundDefOf.Interact_ReleaseSkylantern.PlayOneShot(new TargetInfo(intVec, this.pawn.Map, false));
			}
		}

		// Token: 0x060093C9 RID: 37833 RVA: 0x0035E82C File Offset: 0x0035CA2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.woodCost, "woodCost", 0, false);
		}

		// Token: 0x04005952 RID: 22866
		public Pawn pawn;

		// Token: 0x04005953 RID: 22867
		public int woodCost = 4;
	}
}
