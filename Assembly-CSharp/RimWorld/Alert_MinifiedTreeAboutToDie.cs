using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0B RID: 7179
	public class Alert_MinifiedTreeAboutToDie : Alert
	{
		// Token: 0x1700211B RID: 8475
		// (get) Token: 0x0600B755 RID: 46933 RVA: 0x00409F68 File Offset: 0x00408168
		private List<Pawn> ColonistsBotheredByTreesDying
		{
			get
			{
				this.colonistsBotheredByTreesDyingResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
					{
						if (pawn.Ideo != null && pawn.Ideo.WarnPlayerOnDesignateChopTree)
						{
							this.colonistsBotheredByTreesDyingResult.Add(pawn);
						}
					}
				}
				return this.colonistsBotheredByTreesDyingResult;
			}
		}

		// Token: 0x1700211C RID: 8476
		// (get) Token: 0x0600B756 RID: 46934 RVA: 0x0040A008 File Offset: 0x00408208
		private List<Thing> TreesAboutToDie
		{
			get
			{
				this.treesAboutToDieResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					this.<get_TreesAboutToDie>g__CheckForTrees|6_0(maps[i].listerThings.ThingsOfDef(ThingDefOf.MinifiedTree));
					foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder))
					{
						ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
						if (thingOwner != null)
						{
							this.<get_TreesAboutToDie>g__CheckForTrees|6_0(thingOwner);
						}
					}
				}
				foreach (Caravan caravan in Find.WorldObjects.Caravans)
				{
					this.<get_TreesAboutToDie>g__CheckForTrees|6_0(CaravanInventoryUtility.AllInventoryItems(caravan));
				}
				return this.treesAboutToDieResult;
			}
		}

		// Token: 0x0600B757 RID: 46935 RVA: 0x0040A104 File Offset: 0x00408304
		public override string GetLabel()
		{
			return "AlertMinifiedTreeAboutToDie".Translate(this.TreesAboutToDie.Count.ToStringCached());
		}

		// Token: 0x0600B758 RID: 46936 RVA: 0x0040A12C File Offset: 0x0040832C
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.ColonistsBotheredByTreesDying)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertMinifiedTreeAboutToDieDesc".Translate(this.TreesAboutToDie.Count.ToStringCached()) + ":\n" + this.sb.ToString().TrimEndNewlines();
		}

		// Token: 0x0600B759 RID: 46937 RVA: 0x0040A1E8 File Offset: 0x004083E8
		public override AlertReport GetReport()
		{
			List<Thing> treesAboutToDie = this.TreesAboutToDie;
			if (treesAboutToDie.Count == 0 || this.ColonistsBotheredByTreesDying.Count == 0)
			{
				return false;
			}
			return AlertReport.CulpritsAre(treesAboutToDie);
		}

		// Token: 0x0600B75B RID: 46939 RVA: 0x0040A248 File Offset: 0x00408448
		[CompilerGenerated]
		private void <get_TreesAboutToDie>g__CheckForTrees|6_0(IList<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				MinifiedTree minifiedTree = things[i] as MinifiedTree;
				if (minifiedTree != null && minifiedTree.TicksTillDeath <= 60000)
				{
					this.treesAboutToDieResult.Add(minifiedTree);
				}
			}
		}

		// Token: 0x04006929 RID: 26921
		public const int TicksLeftThreshold = 60000;

		// Token: 0x0400692A RID: 26922
		private List<Pawn> colonistsBotheredByTreesDyingResult = new List<Pawn>();

		// Token: 0x0400692B RID: 26923
		private List<Thing> treesAboutToDieResult = new List<Thing>();

		// Token: 0x0400692C RID: 26924
		private StringBuilder sb = new StringBuilder();
	}
}
