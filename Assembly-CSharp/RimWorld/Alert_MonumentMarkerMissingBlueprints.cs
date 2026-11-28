using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF8 RID: 7160
	public class Alert_MonumentMarkerMissingBlueprints : Alert
	{
		// Token: 0x17002117 RID: 8471
		// (get) Token: 0x0600B705 RID: 46853 RVA: 0x00407D84 File Offset: 0x00405F84
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (!monumentMarker.complete)
						{
							SketchEntity firstEntityWithMissingBlueprint = monumentMarker.FirstEntityWithMissingBlueprint;
							if (firstEntityWithMissingBlueprint != null)
							{
								this.targets.Add(new GlobalTargetInfo(firstEntityWithMissingBlueprint.pos + monumentMarker.Position, maps[i], false));
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600B706 RID: 46854 RVA: 0x00407E34 File Offset: 0x00406034
		public Alert_MonumentMarkerMissingBlueprints()
		{
			this.defaultLabel = "MonumentMarkerMissingBlueprints".Translate();
			this.defaultExplanation = "MonumentMarkerMissingBlueprintsDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0600B707 RID: 46855 RVA: 0x00407E83 File Offset: 0x00406083
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04006900 RID: 26880
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
