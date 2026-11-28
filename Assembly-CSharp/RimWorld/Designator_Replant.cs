using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C6D RID: 7277
	public class Designator_Replant : Designator_Install
	{
		// Token: 0x170021A3 RID: 8611
		// (get) Token: 0x0600B9D4 RID: 47572 RVA: 0x00418FD4 File Offset: 0x004171D4
		public override string Label
		{
			get
			{
				return "CommandReplant".Translate();
			}
		}

		// Token: 0x170021A4 RID: 8612
		// (get) Token: 0x0600B9D5 RID: 47573 RVA: 0x00418FE5 File Offset: 0x004171E5
		public override string Desc
		{
			get
			{
				return "CommandReplantDesc".Translate();
			}
		}

		// Token: 0x0600B9D6 RID: 47574 RVA: 0x00418FF6 File Offset: 0x004171F6
		public Designator_Replant()
		{
			this.icon = TexCommand.Replant;
			this.soundSucceeded = SoundDefOf.Designate_ExtractTree;
		}

		// Token: 0x0600B9D7 RID: 47575 RVA: 0x00419014 File Offset: 0x00417214
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			Plant plant = (Plant)base.ThingToInstall;
			Thing thing;
			AcceptanceReport report = plant.def.CanEverPlantAt(c, base.Map, out thing, true, true, false);
			if (!report)
			{
				return new AcceptanceReport("CannotBePlantedHere".Translate() + ": " + report.Reason.CapitalizeFirst());
			}
			if (plant.def.plant.interferesWithRoof && c.Roofed(base.Map))
			{
				return "CannotBePlantedHere".Translate() + ": " + "BlockedByRoof".Translate().CapitalizeFirst();
			}
			if (!plant.def.CanNowPlantAt(c, base.Map, true))
			{
				return new AcceptanceReport("CannotBePlantedHere".Translate());
			}
			foreach (Thing thing2 in c.GetThingList(base.Map))
			{
				Blueprint_Install blueprint_Install = thing2 as Blueprint_Install;
				if (blueprint_Install != null && blueprint_Install.ThingToInstall.def.plant != null && blueprint_Install.ThingToInstall.def.plant.IsTree)
				{
					return "IdenticalThingExists".Translate();
				}
			}
			return base.CanDesignateCell(c);
		}
	}
}
