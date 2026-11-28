using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE1 RID: 7137
	public class Alert_TimedMakeFactionHostile : Alert
	{
		// Token: 0x17002104 RID: 8452
		// (get) Token: 0x0600B6AF RID: 46767 RVA: 0x00405AA8 File Offset: 0x00403CA8
		private List<GlobalTargetInfo> WorldObjects
		{
			get
			{
				this.worldObjectsResult.Clear();
				foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
				{
					TimedMakeFactionHostile component = worldObject.GetComponent<TimedMakeFactionHostile>();
					if (component != null && component.TicksLeft != null)
					{
						this.worldObjectsResult.Add(worldObject);
					}
				}
				return this.worldObjectsResult;
			}
		}

		// Token: 0x0600B6B0 RID: 46768 RVA: 0x00405B38 File Offset: 0x00403D38
		public Alert_TimedMakeFactionHostile()
		{
			this.defaultLabel = "FactionWillBecomeHostileIfNotLeavingWithin".Translate();
		}

		// Token: 0x0600B6B1 RID: 46769 RVA: 0x00405B6C File Offset: 0x00403D6C
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (GlobalTargetInfo globalTargetInfo in this.worldObjectsResult)
			{
				this.sb.Append("  - ");
				this.sb.Append(globalTargetInfo.Label);
				this.sb.Append(" (");
				this.sb.Append(globalTargetInfo.WorldObject.GetComponent<TimedMakeFactionHostile>().TicksLeft.Value.ToStringTicksToPeriodVerbose(true, true));
				this.sb.AppendLine(")");
			}
			return "FactionWillBecomeHostileIfNotLeavingWithinDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B6B2 RID: 46770 RVA: 0x00405C58 File Offset: 0x00403E58
		public override AlertReport GetReport()
		{
			List<GlobalTargetInfo> worldObjects = this.WorldObjects;
			Map currentMap = Find.CurrentMap;
			List<Pawn> culprits;
			if (!WorldRendererUtility.WorldSelected && currentMap != null && worldObjects.Contains(currentMap.Parent) && !(culprits = currentMap.mapPawns.FreeHumanlikesSpawnedOfFaction(currentMap.ParentFaction)).NullOrEmpty<Pawn>())
			{
				return AlertReport.CulpritsAre(culprits);
			}
			if (worldObjects.Count > 0)
			{
				return AlertReport.CulpritsAre(worldObjects);
			}
			return false;
		}

		// Token: 0x040068E4 RID: 26852
		private List<GlobalTargetInfo> worldObjectsResult = new List<GlobalTargetInfo>();

		// Token: 0x040068E5 RID: 26853
		private StringBuilder sb = new StringBuilder();
	}
}
