using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023B3 RID: 9139
	public class RoutePlannerWaypoint : WorldObject
	{
		// Token: 0x170027FE RID: 10238
		// (get) Token: 0x0600E3A3 RID: 58275 RVA: 0x0054A52C File Offset: 0x0054872C
		public override string Label
		{
			get
			{
				WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
				if (worldRoutePlanner.Active)
				{
					int num = worldRoutePlanner.waypoints.IndexOf(this);
					if (num >= 0)
					{
						return base.Label + " " + (num + 1).ToString();
					}
				}
				return base.Label;
			}
		}

		// Token: 0x0600E3A4 RID: 58276 RVA: 0x0054A57C File Offset: 0x0054877C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			if (worldRoutePlanner.Active)
			{
				int num = worldRoutePlanner.waypoints.IndexOf(this);
				if (num >= 1)
				{
					int ticksToWaypoint = worldRoutePlanner.GetTicksToWaypoint(num);
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("EstimatedTimeToWaypoint".Translate(ticksToWaypoint.ToStringTicksToDays("0.#")));
					if (num >= 2)
					{
						int ticksToWaypoint2 = worldRoutePlanner.GetTicksToWaypoint(num - 1);
						stringBuilder.AppendLine();
						stringBuilder.Append("EstimatedTimeToWaypointFromPrevious".Translate((ticksToWaypoint - ticksToWaypoint2).ToStringTicksToDays("0.#")));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600E3A5 RID: 58277 RVA: 0x0054A640 File Offset: 0x00548840
		public override IEnumerable<Gizmo> GetGizmos()
		{
			RoutePlannerWaypoint.<GetGizmos>d__3 <GetGizmos>d__ = new RoutePlannerWaypoint.<GetGizmos>d__3(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}
	}
}
